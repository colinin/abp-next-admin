using IdentityServer4.Events;
using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using LINGYUN.Abp.Account.Web.ExternalProviders;
using LINGYUN.Abp.Account.Web.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Account.Settings;
using Volo.Abp.Account.Web;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Identity;
using Volo.Abp.IdentityServer.AspNetIdentity;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Settings;
using static Volo.Abp.Account.Web.Pages.Account.LoginModel;
using IdentityOptions = Microsoft.AspNetCore.Identity.IdentityOptions;

namespace LINGYUN.Abp.Account.Web.IdentityServer.Pages.Account
{
    /// <summary>
    /// 重写登录模型,实现双因素登录
    /// </summary>
    [Dependency(ReplaceServices = true)]
    [ExposeServices(
        typeof(LINGYUN.Abp.Account.Web.Pages.Account.LoginModel), 
        typeof(IdentityServerLoginModel))]
    public class IdentityServerLoginModel : LINGYUN.Abp.Account.Web.Pages.Account.LoginModel
    {
        protected IIdentityServerInteractionService Interaction { get; }
        protected IEventService IdentityServerEvents { get; }
        protected IClientStore ClientStore { get; }
        public IdentityServerLoginModel(
            IExternalProviderService externalProviderService,
            IAuthenticationSchemeProvider schemeProvider, 
            IOptions<AbpAccountOptions> accountOptions, 
            IOptions<IdentityOptions> identityOptions,
            IdentityDynamicClaimsPrincipalContributorCache identityDynamicClaimsPrincipalContributorCache,
            IIdentityServerInteractionService interaction,
            IEventService identityServerEvents,
            IClientStore clientStore) 
            : base(externalProviderService, schemeProvider, accountOptions, identityOptions, identityDynamicClaimsPrincipalContributorCache)
        {
            Interaction = interaction;
            ClientStore = clientStore;
            IdentityServerEvents = identityServerEvents;
        }

        public override async Task<IActionResult> OnGetAsync()
        {
            LoginInput = new LoginInputModel();

            var context = await Interaction.GetAuthorizationContextAsync(ReturnUrl);

            if (context != null)
            {
                // TODO: Find a proper cancel way.
                // ShowCancelButton = true;

                LoginInput.UserNameOrEmailAddress = context.LoginHint;

                //TODO: Reference AspNetCore MultiTenancy module and use options to get the tenant key!
                var tenant = context.Parameters[TenantResolverConsts.DefaultTenantKey];
                if (!string.IsNullOrEmpty(tenant))
                {
                    CurrentTenant.Change(Guid.Parse(tenant));
                    Response.Cookies.Append(TenantResolverConsts.DefaultTenantKey, tenant);
                }
            }

            if (context?.IdP != null)
            {
                LoginInput.UserNameOrEmailAddress = context.LoginHint;
                ExternalProviders = new[] { new ExternalLoginProviderModel { AuthenticationScheme = context.IdP } };
                return Page();
            }

            var providers = await GetExternalProviders();
            ExternalProviders = providers.ToList();

            EnableLocalLogin = await SettingProvider.IsTrueAsync(AccountSettingNames.EnableLocalLogin);

            if (context?.Client?.ClientId != null)
            {
                var client = await ClientStore.FindEnabledClientByIdAsync(context?.Client?.ClientId);
                if (client != null)
                {
                    EnableLocalLogin = client.EnableLocalLogin;

                    if (client.IdentityProviderRestrictions != null && client.IdentityProviderRestrictions.Any())
                    {
                        providers = providers.Where(provider => client.IdentityProviderRestrictions.Contains(provider.AuthenticationScheme)).ToList();
                    }
                }
            }

            if (IsExternalLoginOnly)
            {
                return await base.OnPostExternalLogin(providers.First().AuthenticationScheme);
            }

            return Page();
        }

        public override async Task<IActionResult> OnPostAsync(string action)
        {
            var context = await Interaction.GetAuthorizationContextAsync(ReturnUrl);
            if (action == "Cancel")
            {
                if (context == null)
                {
                    return Redirect("~/");
                }

                await Interaction.GrantConsentAsync(context, new ConsentResponse()
                {
                    Error = AuthorizationError.AccessDenied
                });

                return Redirect(ReturnUrl);
            }

            await CheckLocalLoginAsync();

            ValidateModel();

            await IdentityOptions.SetAsync();

            ExternalProviders = await GetExternalProviders();

            EnableLocalLogin = await SettingProvider.IsTrueAsync(AccountSettingNames.EnableLocalLogin);

            await ReplaceEmailToUsernameOfInputIfNeeds();

            var result = await SignInManager.PasswordSignInAsync(
                LoginInput.UserNameOrEmailAddress,
                LoginInput.Password,
                LoginInput.RememberMe,
                true
            );

            await IdentitySecurityLogManager.SaveAsync(new IdentitySecurityLogContext()
            {
                Identity = IdentitySecurityLogIdentityConsts.Identity,
                Action = result.ToIdentitySecurityLogAction(),
                UserName = LoginInput.UserNameOrEmailAddress,
                ClientId = context?.Client?.ClientId
            });

            if (result.RequiresTwoFactor)
            {
                return await TwoFactorLoginResultAsync();
            }

            if (result.IsLockedOut)
            {
                return await HandleUserLockedOut();
            }

            if (result.IsNotAllowed)
            {
                return await HandleUserNotAllowed();
            }

            if (!result.Succeeded)
            {
                return await HandleUserNameOrPasswordInvalid();
            }

            //TODO: Find a way of getting user's id from the logged in user and do not query it again like that!
            var user = await UserManager.FindByNameAsync(LoginInput.UserNameOrEmailAddress) ??
                       await UserManager.FindByEmailAsync(LoginInput.UserNameOrEmailAddress);

            Debug.Assert(user != null, nameof(user) + " != null");
            await IdentityServerEvents.RaiseAsync(new UserLoginSuccessEvent(user.UserName, user.Id.ToString(), user.UserName)); //TODO: Use user's name once implemented

            // Clear the dynamic claims cache.
            await IdentityDynamicClaimsPrincipalContributorCache.ClearAsync(user.Id, user.TenantId);

            return await RedirectSafelyAsync(ReturnUrl, ReturnUrlHash);
        }
    }
}
