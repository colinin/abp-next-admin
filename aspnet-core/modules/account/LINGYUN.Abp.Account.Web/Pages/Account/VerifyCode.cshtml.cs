using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Volo.Abp.Account.Web.Pages.Account;

namespace LINGYUN.Abp.Account.Web.Pages.Account
{
    public class VerifyCodeModel : AccountPageModel
    {
        [BindProperty]
        public VerifyCodeInputModel Input { get; set; }

        public virtual IActionResult OnGet()
        {
            Input = new VerifyCodeInputModel();

            return Page();
        }

        public async virtual Task<IActionResult> OnPostAsync()
        {
            var user = await SignInManager.GetTwoFactorAuthenticationUserAsync();
            if (user == null)
            {
                Alerts.Warning("˫������֤ʧ��,�û�δ��¼����ʧЧ!");
                return Page();
            }

            var result = await SignInManager.TwoFactorSignInAsync(Input.Provider, Input.Code, Input.RememberMe, Input.RememberBrowser);
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

    public class VerifyCodeInputModel
    {
        [Required]
        public string Provider { get; set; }

        [Required]
        public string Code { get; set; }

        public string ReturnUrl { get; set; }

        [Display(Name = "Remember this browser?")]
        public bool RememberBrowser { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }
}
