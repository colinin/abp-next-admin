using LINGYUN.Abp.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Volo.Abp.Account.Web.Pages.Account;
using Volo.Abp.Identity;

namespace LINGYUN.Abp.Account.Web.Pages.Account
{
    public class VerifyAuthenticatorCodeModel : AccountPageModel
    {
        [BindProperty]
        public VerifyAuthenticatorCodeInputModel Input { get; set; } = default!;

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

        public virtual IActionResult OnGet()
        {
            Input = new VerifyAuthenticatorCodeInputModel();

            return Page();
        }

        public virtual async Task<IActionResult> OnPostAsync()
        {
            var user = await SignInManager.GetTwoFactorAuthenticationUserAsync();
            if (user == null)
            {
                Alerts.Warning(L["TwoFactorAuthenticationInvaidUser"]);
                return Page();
            }

            var result = await SignInManager.TwoFactorAuthenticatorSignInAsync(Input.VerifyCode, RememberMe, Input.RememberBrowser);
            if (result.Succeeded)
            {
                await IdentitySecurityLogManager.SaveAsync(new IdentitySecurityLogContext()
                {
                    Identity = IdentitySecurityLogIdentityConsts.Identity,
                    Action = IdentitySecurityLogExtendActionConsts.LoginTwoFactorSucceeded,
                    UserName = user.UserName
                });

                return await RedirectSafelyAsync(ReturnUrl!, ReturnUrlHash);
            }
            if (result.IsLockedOut)
            {
                Logger.LogWarning(7, "User account locked out.");
                Alerts.Warning(L["UserLockedOutMessage"]);
                return Page();
            }
            else
            {
                Alerts.Danger(L["TwoFactorAuthenticationInvaidUser"]);// TODO: 更多状态码的解读
                return Page();
            }
        }
    }

    public class VerifyAuthenticatorCodeInputModel
    {
        [Required]
        public string VerifyCode { get; set; } = default!;

        public bool RememberBrowser { get; set; }
    }
}
