using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Volo.Abp.Account.Settings;
using Volo.Abp.Account.Web;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Identity;
using Volo.Abp.Identity.AspNetCore;
using Volo.Abp.Security.Claims;
using Volo.Abp.Settings;
using IdentityUser = Volo.Abp.Identity.IdentityUser;

namespace LINGYUN.Abp.Account.Web.Pages.Account;

[ExposeServices(typeof(Volo.Abp.Account.Web.Pages.Account.LoginModel))]
public class LoginModel : Volo.Abp.Account.Web.Pages.Account.LoginModel
{
    public LoginModel(
        IAuthenticationSchemeProvider schemeProvider,
        IOptions<AbpAccountOptions> accountOptions,
        IOptions<IdentityOptions> identityOptions,
        IdentityDynamicClaimsPrincipalContributorCache identityDynamicClaimsPrincipalContributorCache) 
        : base(schemeProvider, accountOptions, identityOptions, identityDynamicClaimsPrincipalContributorCache)
    {
    }

    public async override Task<IActionResult> OnPostAsync(string action)
    {
        await CheckLocalLoginAsync();

        ValidateModel();

        ExternalProviders = await GetExternalProviders();

        EnableLocalLogin = await SettingProvider.IsTrueAsync(AccountSettingNames.EnableLocalLogin);

        await ReplaceEmailToUsernameOfInputIfNeeds();

        await IdentityOptions.SetAsync();

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
            UserName = LoginInput.UserNameOrEmailAddress
        });

        if (result.RequiresTwoFactor)
        {
            return await TwoFactorLoginResultAsync();
        }

        if (result.IsLockedOut)
        {
            Alerts.Warning(L["UserLockedOutMessage"]);
            return Page();
        }

        if (result.IsNotAllowed)
        {
            var notAllowedUser = await GetIdentityUserAsync(LoginInput.UserNameOrEmailAddress);
            if (await UserManager.CheckPasswordAsync(notAllowedUser, LoginInput.Password))
            {
                // 用户必须修改密码
                if (notAllowedUser.ShouldChangePasswordOnNextLogin || await UserManager.ShouldPeriodicallyChangePasswordAsync(notAllowedUser))
                {
                    var changePwdIdentity = new ClaimsIdentity(AbpAccountAuthenticationTypes.ShouldChangePassword);
                    changePwdIdentity.AddClaim(new Claim(AbpClaimTypes.UserId, notAllowedUser.Id.ToString()));
                    if (notAllowedUser.TenantId.HasValue)
                    {
                        changePwdIdentity.AddClaim(new Claim(AbpClaimTypes.TenantId, notAllowedUser.TenantId.ToString()));
                    }

                    await HttpContext.SignInAsync(AbpAccountAuthenticationTypes.ShouldChangePassword, new ClaimsPrincipal(changePwdIdentity));

                    return RedirectToPage("ChangePassword", new
                    {
                        returnUrl = ReturnUrl,
                        returnUrlHash = ReturnUrlHash,
                        rememberMe = LoginInput.RememberMe
                    });
                }
            }

            Alerts.Warning(L["LoginIsNotAllowed"]);
            return Page();
        }

        if (!result.Succeeded)
        {
            Alerts.Danger(L["InvalidUserNameOrPassword"]);
            return Page();
        }

        //TODO: Find a way of getting user's id from the logged in user and do not query it again like that!
        var user = await GetIdentityUserAsync(LoginInput.UserNameOrEmailAddress);

        Debug.Assert(user != null, nameof(user) + " != null");

        // Clear the dynamic claims cache.
        await IdentityDynamicClaimsPrincipalContributorCache.ClearAsync(user.Id, user.TenantId);

        return await RedirectSafelyAsync(ReturnUrl, ReturnUrlHash);
    }

    protected override Task<IActionResult> TwoFactorLoginResultAsync()
    {
        // 重定向双因素认证页面
        return Task.FromResult<IActionResult>(RedirectToPage("SendCode", new
        {
            returnUrl = ReturnUrl,
            returnUrlHash = ReturnUrlHash,
            rememberMe = LoginInput.RememberMe
        }));
    }

    protected virtual async Task<IdentityUser> GetIdentityUserAsync(string userNameOrEmailAddress)
    {
        return await UserManager.FindByNameAsync(LoginInput.UserNameOrEmailAddress) ??
            await UserManager.FindByEmailAsync(LoginInput.UserNameOrEmailAddress);
    }

    protected async override Task<List<ExternalProviderModel>> GetExternalProviders()
    {
        var schemes = await SchemeProvider.GetAllSchemesAsync();

        var providers = schemes
            .Where(x => x.DisplayName != null || x.Name.Equals(AccountOptions.WindowsAuthenticationSchemeName, StringComparison.OrdinalIgnoreCase))
            .Select(x => new ExternalProviderModel
            {
                DisplayName = x.DisplayName,
                AuthenticationScheme = x.Name
            })
            .ToList();

        foreach (var provider in providers)
        {
            var localizedDisplayName = L[provider.DisplayName];
            if (localizedDisplayName.ResourceNotFound)
            {
                localizedDisplayName = L["AuthenticationScheme:" + provider.DisplayName];
            }

            if (!localizedDisplayName.ResourceNotFound)
            {
                provider.DisplayName = localizedDisplayName.Value;
            }
        }

        return providers;
    }
}
