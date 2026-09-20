using LINGYUN.Abp.Identity;
using LINGYUN.Abp.Identity.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Account.Web.Pages.Account;
using Volo.Abp.Auditing;
using Volo.Abp.Data;
using Volo.Abp.Identity;
using Volo.Abp.Identity.AspNetCore;
using Volo.Abp.Validation;

namespace LINGYUN.Abp.Account.Web.Pages.Account;

public class TwoFactorAuthModel : AccountPageModel
{
    [BindProperty]
    public TwoFactorInputModel Input { get; set; } = default!;

    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public string? AuthenticatorUri { get; set; }

    [BindProperty(SupportsGet = true)]
    public string? SharedKey { get; set; }

    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public string? ReturnUrl { get; set; }

    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public string? ReturnUrlHash { get; set; }

    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public bool RememberMe { get; set; }

    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public Guid? LinkUserId { get; set; }

    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public Guid? LinkTenantId { get; set; }

    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public string? LinkToken { get; set; }

    protected IAuthenticatorUriGenerator AuthenticatorUriGenerator { get; }

    public TwoFactorAuthModel(IAuthenticatorUriGenerator authenticatorUriGenerator)
    {
        AuthenticatorUriGenerator = authenticatorUriGenerator;
    }

    public async virtual Task<IActionResult> OnGetAsync()
    {
        Input = new TwoFactorInputModel();

        var user = await SignInManager.GetTwoFactorAuthenticationUserAsync();
        if (user == null)
        {
            Alerts.Warning(L["TwoFactorAuthenticationInvaidUser"]);
            return Page();
        }

        var userEmail = await UserManager.GetEmailAsync(user);
        var unformattedKey = await UserManager.GetAuthenticatorKeyAsync(user);
        if (string.IsNullOrEmpty(unformattedKey))
        {
            await UserManager.ResetAuthenticatorKeyAsync(user);
            unformattedKey = await UserManager.GetAuthenticatorKeyAsync(user);
        }

        SharedKey = FormatKey(unformattedKey!)!;
        AuthenticatorUri = AuthenticatorUriGenerator.Generate(userEmail!, unformattedKey!);

        return Page();
    }

    public async virtual Task<IActionResult> OnPostAsync()
    {
        var user = await SignInManager.GetTwoFactorAuthenticationUserAsync();
        if (user == null)
        {
            Alerts.Warning(L["TwoFactorAuthenticationInvaidUser"]);
            return Page();
        }

        if (!await UserManager.CheckPasswordAsync(user, Input.Password))
        {
            Alerts.Warning(L["InvalidUserNameOrPassword"]);
            return Page();
        }

        var result = await SignInManager.TwoFactorAuthenticatorSignInAsync(Input.Code, RememberMe, Input.RememberBrowser);
        if (!result.Succeeded)
        {
            await IdentitySecurityLogManager.SaveAsync(new IdentitySecurityLogContext()
            {
                Identity = IdentitySecurityLogIdentityConsts.Identity,
                Action = IdentitySecurityLogExtendActionConsts.LoginTwoFactorFailed,
                UserName = user.UserName
            });
            Alerts.Danger(L["InvalidAuthenticatorCode"]);
            return Page();
        }

        var recoveryCodes = await UserManager.GenerateNewTwoFactorRecoveryCodesAsync(user, 10);

        user.SetProperty(UserManager.Options.Tokens.AuthenticatorTokenProvider, true);

        (await UserManager.UpdateAsync(user)).CheckErrors();

        await IdentitySecurityLogManager.SaveAsync(new IdentitySecurityLogContext()
        {
            Identity = IdentitySecurityLogIdentityConsts.Identity,
            Action = IdentitySecurityLogExtendActionConsts.LoginTwoFactorSucceeded,
            UserName = user.UserName
        });

        return RedirectToPage("TwoFactorRecoveryCodes", new
        {
            returnUrl = ReturnUrl,
            returnUrlHash = ReturnUrlHash,
            recoveryCodes = recoveryCodes?.JoinAsString(Environment.NewLine),
        });
    }

    private static string? FormatKey(string? unformattedKey)
    {
        if (unformattedKey.IsNullOrWhiteSpace())
        {
            return null;
        }
        var result = new StringBuilder();
        var currentPosition = 0;
        while (currentPosition + 4 < unformattedKey.Length)
        {
            result.Append(unformattedKey.AsSpan(currentPosition, 4)).Append(' ');
            currentPosition += 4;
        }
        if (currentPosition < unformattedKey.Length)
        {
            result.Append(unformattedKey.AsSpan(currentPosition));
        }

        return result.ToString().ToLowerInvariant();
    }

    public class TwoFactorInputModel
    {
        [Required]
        [DisableAuditing]
        [DataType(DataType.Password)]
        [DisplayName("DisplayName:UserPassword")]
        [DynamicStringLength(typeof(IdentityUserConsts), nameof(IdentityUserConsts.MaxPasswordLength))]
        public string Password { get; set; } = default!;

        [Required]
        [DisplayName("DisplayName:AuthenticatorCode")]
        public string Code { get; set; } = default!;

        public bool RememberBrowser { get; set; }
    }
}
