using LINGYUN.Abp.Account.Emailing;
using LINGYUN.Abp.Identity.Settings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp;
using Volo.Abp.Account.Localization;
using Volo.Abp.Account.Web.Pages.Account;
using Volo.Abp.Sms;

namespace PackageName.CompanyName.ProjectName.AIO.Host.Pages.Account
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
                // ˫������Ϣ��֤ʧ��,һ�㶼�ǳ�ʱ�˻����û���Ϣ���
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
                // �û�ͨ���ʼ�/�������ӽ�����Ȩҳ��
                return RedirectToPage("VerifyAuthenticatorCode", new
                {
                    returnUrl = ReturnUrl,
                    returnUrlHash = ReturnUrlHash,
                    rememberMe = RememberMe
                });
            }
            // ������֤��
            var code = await UserManager.GenerateTwoFactorTokenAsync(user, Input.SelectedProvider);
            if (string.IsNullOrWhiteSpace(code))
            {
                Alerts.Warning(L["InvaidGenerateTwoFactorToken"]);
                return Page();
            }

            if (Input.SelectedProvider == "Email")
            {
                await AccountEmailVerifySender
                    .SendMailLoginVerifyCodeAsync(
                        code,
                        user.UserName,
                        user.Email);
            }
            else if (Input.SelectedProvider == "Phone")
            {
                var phoneNumber = await UserManager.GetPhoneNumberAsync(user);
                var templateCode = await SettingProvider.GetOrNullAsync(IdentitySettingNames.User.SmsUserSignin);
                Check.NotNullOrWhiteSpace(templateCode, nameof(IdentitySettingNames.User.SmsUserSignin));

                // TODO: �Ժ���չ����ģ�巢��
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
