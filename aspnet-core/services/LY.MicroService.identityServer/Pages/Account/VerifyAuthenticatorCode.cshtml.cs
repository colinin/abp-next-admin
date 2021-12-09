using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Volo.Abp.Account.Web.Pages.Account;

namespace LY.MicroService.IdentityServer.Pages.Account
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
                return RedirectSafely(ReturnUrl, ReturnUrlHash);
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
        public string VerifyCode { get; set; }
    }
}
