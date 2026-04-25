using IdentityServer4.Events;
using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using LINGYUN.Abp.Account.Web.ExternalProviders;
using LINGYUN.Abp.Account.Web.Models;
using LINGYUN.Abp.Account.Web.Pages.Account;
using LINGYUN.Abp.Identity.QrCode;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Volo.Abp.Account.Settings;
using Volo.Abp.Account.Web;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Identity;
using Volo.Abp.IdentityServer.AspNetIdentity;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Settings;
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
            PasswordLoginInput = new PasswordLoginInputModel();

            var context = await Interaction.GetAuthorizationContextAsync(ReturnUrl);

            if (context != null)
            {
                // TODO: Find a proper cancel way.
                // ShowCancelButton = true;

                PasswordLoginInput.UserNameOrEmailAddress = context.LoginHint;

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
                PasswordLoginInput.UserNameOrEmailAddress = context.LoginHint;
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

        public override async Task<IActionResult> OnPostPasswordLogin(string action)
        {
            var context = await Interaction.GetAuthorizationContextAsync(ReturnUrl);
            if (action == "Cancel")
            {
                return await OnCancelLogin(context);
            }

            LoginType = LoginType.Password;

            await CheckLocalLoginAsync();

            ExternalProviders = await GetExternalProviders();

            EnableLocalLogin = await SettingProvider.IsTrueAsync(AccountSettingNames.EnableLocalLogin);

            ModelState.RemoveModelErrors(nameof(PhoneLoginInput));
            ModelState.RemoveModelErrors(nameof(QrCodeLoginInput));
            if (!TryValidateModel(PasswordLoginInput, nameof(PasswordLoginInput)))
            {
                return Page();
            }

            await ReplaceEmailToUsernameOfInputIfNeeds();

            await IdentityOptions.SetAsync();

            var result = await SignInManager.PasswordSignInAsync(
                PasswordLoginInput.UserNameOrEmailAddress,
                PasswordLoginInput.Password,
                PasswordLoginInput.RememberMe,
                true
            );

            await IdentitySecurityLogManager.SaveAsync(new IdentitySecurityLogContext()
            {
                Identity = IdentitySecurityLogIdentityConsts.Identity,
                Action = result.ToIdentitySecurityLogAction(),
                UserName = PasswordLoginInput.UserNameOrEmailAddress
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
            var user = await GetIdentityUserAsync(PasswordLoginInput.UserNameOrEmailAddress);

            Debug.Assert(user != null, nameof(user) + " != null");
            await IdentityServerEvents.RaiseAsync(new UserLoginSuccessEvent(user.UserName, user.Id.ToString(), user.UserName)); //TODO: Use user's name once implemented

            // Clear the dynamic claims cache.
            await IdentityDynamicClaimsPrincipalContributorCache.ClearAsync(user.Id, user.TenantId);

            return await RedirectSafelyAsync(PasswordLoginInput.ReturnUrl, PasswordLoginInput.ReturnUrlHash);
        }

        public override async Task<IActionResult> OnPostPhoneNumberLogin(string action)
        {
            var context = await Interaction.GetAuthorizationContextAsync(ReturnUrl);
            if (action == "Cancel")
            {
                return await OnCancelLogin(context);
            }

            LoginType = LoginType.PhoneNumber;

            await CheckLocalLoginAsync();

            ExternalProviders = await GetExternalProviders();

            EnableLocalLogin = await SettingProvider.IsTrueAsync(AccountSettingNames.EnableLocalLogin);

            ModelState.RemoveModelErrors(nameof(QrCodeLoginInput));
            ModelState.RemoveModelErrors(nameof(PasswordLoginInput));
            if (!TryValidateModel(PhoneLoginInput, nameof(PhoneLoginInput)))
            {
                return Page();
            }

            var user = await UserRepository.FindByPhoneNumberAsync(PhoneLoginInput.PhoneNumber);
            if (user == null)
            {
                Logger.LogInformation("the user phone number is not registed!");
                Alerts.Danger(L["InvalidPhoneNumber"]);
                return Page();
            }

            var result = await UserManager.VerifyTwoFactorTokenAsync(user, TokenOptions.DefaultPhoneProvider, PhoneLoginInput.Code);
            if (!result)
            {
                Alerts.Danger(L["InvalidVerifyCode"]);
                return Page();
            }

            await SignInManager.SignInAsync(user, PhoneLoginInput.RememberMe);

            await IdentitySecurityLogManager.SaveAsync(new IdentitySecurityLogContext()
            {
                Identity = IdentitySecurityLogIdentityConsts.IdentityTwoFactor,
                Action = IdentitySecurityLogActionConsts.LoginSucceeded,
                UserName = user.UserName
            });

            await IdentityServerEvents.RaiseAsync(new UserLoginSuccessEvent(user.UserName, user.Id.ToString(), user.UserName)); //TODO: Use user's name once implemented

            // Clear the dynamic claims cache.
            await IdentityDynamicClaimsPrincipalContributorCache.ClearAsync(user.Id, user.TenantId);

            return await RedirectSafelyAsync(PhoneLoginInput.ReturnUrl, PhoneLoginInput.ReturnUrlHash);
        }

        public override async Task<IActionResult> OnPostQrCodeLogin(string action)
        {
            var context = await Interaction.GetAuthorizationContextAsync(ReturnUrl);
            if (action == "Cancel")
            {
                return await OnCancelLogin(context);
            }

            LoginType = LoginType.QrCode;

            await CheckLocalLoginAsync();

            ExternalProviders = await GetExternalProviders();

            EnableLocalLogin = await SettingProvider.IsTrueAsync(AccountSettingNames.EnableLocalLogin);

            ModelState.RemoveModelErrors(nameof(PhoneLoginInput));
            ModelState.RemoveModelErrors(nameof(PasswordLoginInput));
            if (!TryValidateModel(QrCodeLoginInput, nameof(QrCodeLoginInput)))
            {
                return Page();
            }

            var qrCodeInfo = await QrCodeLoginProvider.GetCodeAsync(QrCodeLoginInput.Key);
            // 二维码扫描后用户Id不为空
            if (qrCodeInfo == null || qrCodeInfo.Token.IsNullOrWhiteSpace() == true)
            {
                Alerts.Danger(L["QrCode:Invalid"]);
                return Page();
            }

            SetTenantCookies(qrCodeInfo.TenantId);
            using (CurrentTenant.Change(qrCodeInfo.TenantId))
            {
                var user = await UserManager.FindByIdAsync(qrCodeInfo.UserId);
                if (user == null)
                {
                    // TODO: 用户验证无效?
                    Alerts.Danger(L["QrCode:Invalid"]);
                    return Page();
                }

                if (!await UserManager.VerifyUserTokenAsync(user, QrCodeLoginProviderConsts.Name, QrCodeLoginProviderConsts.Purpose, qrCodeInfo.Token))
                {
                    Alerts.Danger(L["QrCode:Invalid"]);
                    return Page();
                }

                // TODO: 记住登录
                await SignInManager.SignInAsync(user, true);

                await QrCodeLoginProvider.RemoveAsync(QrCodeLoginInput.Key);

                await IdentitySecurityLogManager.SaveAsync(new IdentitySecurityLogContext()
                {
                    Identity = QrCodeLoginProviderConsts.Purpose,
                    Action = IdentitySecurityLogActionConsts.LoginSucceeded,
                    UserName = user.UserName
                });

                await IdentityServerEvents.RaiseAsync(new UserLoginSuccessEvent(user.UserName, user.Id.ToString(), user.UserName)); //TODO: Use user's name once implemented

                // Clear the dynamic claims cache.
                await IdentityDynamicClaimsPrincipalContributorCache.ClearAsync(user.Id, user.TenantId);

                return await RedirectSafelyAsync(QrCodeLoginInput.ReturnUrl, QrCodeLoginInput.ReturnUrlHash);
            }
        }

        public override async Task<IActionResult> OnPostExternalLogin(string provider)
        {
            if (AccountOptions.WindowsAuthenticationSchemeName == provider)
            {
                return await ProcessWindowsLoginAsync();
            }

            return await base.OnPostExternalLogin(provider);
        }

        protected async virtual Task<IActionResult> OnCancelLogin(AuthorizationRequest context)
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

        protected virtual async Task<IActionResult> ProcessWindowsLoginAsync()
        {
            var result = await HttpContext.AuthenticateAsync(AccountOptions.WindowsAuthenticationSchemeName);
            if (result.Succeeded)
            {
                var props = new AuthenticationProperties()
                {
                    RedirectUri = Url.Page("./Login", pageHandler: "ExternalLoginCallback", values: new { ReturnUrl, ReturnUrlHash }),
                    Items =
                    {
                        {
                            "LoginProvider", AccountOptions.WindowsAuthenticationSchemeName
                        },
                    }
                };

                var id = new ClaimsIdentity(AccountOptions.WindowsAuthenticationSchemeName);
                id.AddClaim(new Claim(ClaimTypes.NameIdentifier, result.Principal.FindFirstValue(ClaimTypes.PrimarySid)));
                id.AddClaim(new Claim(ClaimTypes.Name, result.Principal.FindFirstValue(ClaimTypes.Name)));

                await HttpContext.SignInAsync(IdentityConstants.ExternalScheme, new ClaimsPrincipal(id), props);

                return Redirect(props.RedirectUri);
            }

            return Challenge(AccountOptions.WindowsAuthenticationSchemeName);
        }
    }
}
