using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Account.Web.Pages.Account;

namespace LY.AIO.Applications.Single.Pages.Account
{
    public class VerifyAuthenticatorCodeModel : AccountPageModel
    {
        [BindProperty]
        public VerifyAuthenticatorCodeInputModel Input { get; set; }

        [HiddenInput]
        [BindProperty(SupportsGet = true)]
        public string ReturnUrl { get; set; }

        [HiddenInput]
        [BindProperty(SupportsGet = true)]
        public string ReturnUrlHash { get; set; }

        [BindProperty(SupportsGet = true)]
        public bool RememberBrowser { get; set; }

        [HiddenInput]
        public bool RememberMe { get; set; }

        public virtual IActionResult OnGet()
        {
            Input = new VerifyAuthenticatorCodeInputModel();

            return Page();
        }

        public virtual async Task<IActionResult> OnPostAsync()
        {
            var result = await SignInManager.TwoFactorAuthenticatorSignInAsync(Input.VerifyCode, RememberMe, RememberBrowser);
            if (result.Succeeded)
            {
                return await RedirectSafelyAsync(ReturnUrl, ReturnUrlHash);
            }
            if (result.IsLockedOut)
            {
                Logger.LogWarning(7, "User account locked out.");
                Alerts.Warning(L["UserLockedOutMessage"]);
                return Page();
            }
            else
            {
                Alerts.Danger(L["TwoFactorAuthenticationInvaidUser"]);// TODO: ����״̬��Ľ��
                return Page();
            }
        }
    }

    public class VerifyAuthenticatorCodeInputModel
    {
        [Required]
        public string VerifyCode { get; set; }
    }
}
