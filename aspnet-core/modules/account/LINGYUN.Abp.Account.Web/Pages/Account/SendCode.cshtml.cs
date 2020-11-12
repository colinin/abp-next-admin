using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Account.Web.Pages.Account;
using Volo.Abp.Emailing;
using Volo.Abp.Sms;

namespace LINGYUN.Abp.Account.Web.Pages.Account
{
    public class SendCodeModel : AccountPageModel
    {
        [BindProperty]
        public SendCodeInputModel SendCodeInput { get; set; }

        protected ISmsSender SmsSender { get; }
        protected IEmailSender EmailSender { get; }

        public virtual IActionResult OnGet()
        {
            SendCodeInput = new SendCodeInputModel();

            return Page();
        }

        public virtual async Task<IActionResult> OnPostAsync()
        {
            var user = await SignInManager.GetTwoFactorAuthenticationUserAsync();
            if (user == null)
            {
                Alerts.Warning("双因素认证失败,用户未登录!");
                return Page();
            }

            if (SendCodeInput.SelectedProvider == "Authenticator")
            {
                var verifyAuthenticatorCodeInput = new VerifyAuthenticatorCodeInputModel
                {
                    ReturnUrl = SendCodeInput.ReturnUrl,
                    RememberMe = SendCodeInput.RememberMe
                };
                return RedirectToPage("VerifyAuthenticatorCode", verifyAuthenticatorCodeInput);
            }

            var code = await UserManager.GenerateTwoFactorTokenAsync(user, SendCodeInput.SelectedProvider);
            if (string.IsNullOrWhiteSpace(code))
            {
                Alerts.Warning("验证码生成失败,请联系系统管理员!");
                return Page();
            }

            var message = "Your security code is: " + code;
            if (SendCodeInput.SelectedProvider == "Email")
            {
                await EmailSender.SendAsync(await UserManager.GetEmailAsync(user), "Security Code", message);
            }
            else if (SendCodeInput.SelectedProvider == "Phone")
            {
                await SmsSender.SendAsync(await UserManager.GetPhoneNumberAsync(user), message);
            }

            var verifyCodeInput = new VerifyCodeInputModel
            {
                Provider = SendCodeInput.SelectedProvider,
                ReturnUrl = SendCodeInput.ReturnUrl,
                RememberMe = SendCodeInput.RememberMe
            };

            return RedirectToPage("VerifyCode", verifyCodeInput);
        }
    }

    public class SendCodeInputModel
    {
        public string SelectedProvider { get; set; }

        public ICollection<SelectListItem> Providers { get; set; }

        public string ReturnUrl { get; set; }

        public bool RememberMe { get; set; }
    }

    public class TwoFactorInputModel
    {
        public string SelectedProvider { get; set; }

        public ICollection<SelectListItem> Providers { get; set; }

        public string ReturnUrl { get; set; }

        public bool RememberMe { get; set; }
    }
}
