using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Volo.Abp.Account.Web.Pages.Account;

namespace LINGYUN.Abp.Account.Web.Pages.Account
{
    public class VerifyAuthenticatorCodeModel : AccountPageModel
    {
        [BindProperty]
        public VerifyAuthenticatorCodeInputModel Input { get; set; }

        public virtual IActionResult OnGet()
        {
            Input = new VerifyAuthenticatorCodeInputModel();

            return Page();
        }

        public async virtual Task<IActionResult> OnPostAsync()
        {
            var result = await SignInManager.TwoFactorAuthenticatorSignInAsync(Input.Code, Input.RememberMe, Input.RememberBrowser);
            if (result.Succeeded)
            {
                return RedirectSafely(Input.ReturnUrl);
            }
            if (result.IsLockedOut)
            {
                Logger.LogWarning(7, "User account locked out.");
                Alerts.Warning(L["UserLockedOutMessage"]);
                return Page();
            }
            else
            {
                Alerts.Danger("��Ȩ����֤��Ч!");
                return Page();
            }
        }
    }

    public class VerifyAuthenticatorCodeInputModel
    {
        [Required]
        public string Code { get; set; }

        public string ReturnUrl { get; set; }

        [Display(Name = "Remember this browser?")]
        public bool RememberBrowser { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }
}
