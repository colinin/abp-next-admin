using LY.MicroService.IdentityServer.Emailing;
using LINGYUN.Abp.Identity.Settings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Account.Localization;
using Volo.Abp.Account.Web.Pages.Account;
using Volo.Abp.Sms;

namespace LY.MicroService.IdentityServer.Pages.Account
{
    public class SendCodeModel : AccountPageModel
    {
        [BindProperty]
        public SendCodeInputModel Input { get; set; }

        [HiddenInput]
        [BindProperty(SupportsGet = true)]
        public string ReturnUrl { get; set; }

        [HiddenInput]
        [BindProperty(SupportsGet = true)]
        public string ReturnUrlHash { get; set; }

        [HiddenInput]
        [BindProperty(SupportsGet = true)]
        public bool RememberMe { get; set; }

        public IEnumerable<SelectListItem> Providers { get; set; }

        protected ISmsSender SmsSender { get; }

        protected IAccountEmailVerifySender AccountEmailVerifySender { get; }

        public SendCodeModel(
            ISmsSender smsSender,
            IAccountEmailVerifySender accountEmailVerifySender)
        {
            SmsSender = smsSender;
            AccountEmailVerifySender = accountEmailVerifySender;

            LocalizationResourceType = typeof(AccountResource);
        }

        public virtual async Task<IActionResult> OnGetAsync()
        {
            Input = new SendCodeInputModel();

            var user = await SignInManager.GetTwoFactorAuthenticationUserAsync();
            if (user == null)
            {
                // 双因素信息验证失败,一般都是超时了或者用户信息变更
                Alerts.Warning(L["TwoFactorAuthenticationInvaidUser"]);
                return Page();
            }
            var userFactors = await UserManager.GetValidTwoFactorProvidersAsync(user);
            Providers = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();

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

            if (Input.SelectedProvider == "Authenticator")
            {
                // 用户通过邮件/短信链接进入授权页面
                return RedirectToPage("VerifyAuthenticatorCode", new
                {
                    returnUrl = ReturnUrl,
                    returnUrlHash = ReturnUrlHash,
                    rememberMe = RememberMe
                });
            }
            // 生成验证码
            var code = await UserManager.GenerateTwoFactorTokenAsync(user, Input.SelectedProvider);
            if (string.IsNullOrWhiteSpace(code))
            {
                Alerts.Warning(L["InvaidGenerateTwoFactorToken"]);
                return Page();
            }

            if (Input.SelectedProvider == "Email")
            {
                var appName = "MVC"; // TODO: 跟随Abp框架的意思变动
                await AccountEmailVerifySender
                    .SendMailLoginVerifyLinkAsync(
                        user, code, appName,
                        Input.SelectedProvider, 
                        RememberMe, ReturnUrl, ReturnUrlHash);
            }
            else if (Input.SelectedProvider == "Phone")
            {
                var phoneNumber = await UserManager.GetPhoneNumberAsync(user);
                var templateCode = await SettingProvider.GetOrNullAsync(IdentitySettingNames.User.SmsUserSignin);
                Check.NotNullOrWhiteSpace(templateCode, nameof(IdentitySettingNames.User.SmsUserSignin));

                // TODO: 以后扩展短信模板发送
                var smsMessage = new SmsMessage(phoneNumber, code);
                smsMessage.Properties.Add("code", code);
                smsMessage.Properties.Add("TemplateCode", templateCode);

                await SmsSender.SendAsync(smsMessage);
            }

            return RedirectToPage("VerifyCode", new
            {
                provider = Input.SelectedProvider,
                returnUrl = ReturnUrl,
                returnUrlHash = ReturnUrlHash,
                rememberMe = RememberMe
            });
        }
    }

    public class SendCodeInputModel
    {
        public string SelectedProvider { get; set; }
    }
}
