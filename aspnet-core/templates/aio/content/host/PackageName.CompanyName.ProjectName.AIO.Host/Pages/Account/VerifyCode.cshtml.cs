using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Account.Localization;
using Volo.Abp.Account.Web.Pages.Account;

namespace PackageName.CompanyName.ProjectName.AIO.Host.Pages.Account
{
    public class VerifyCodeModel : AccountPageModel
    {
        [BindProperty]
        public VerifyCodeInputModel Input { get; set; }
        /// <summary>
        /// ˫������֤�ṩ����
        /// </summary>
        [HiddenInput]
        [BindProperty(SupportsGet = true)]
        public string Provider { get; set; }
        /// <summary>
        /// �ض���Url
        /// </summary>
        [HiddenInput]
        [BindProperty(SupportsGet = true)]
        public string ReturnUrl { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [HiddenInput]
        [BindProperty(SupportsGet = true)]
        public string ReturnUrlHash { get; set; }
        /// <summary>
        /// �Ƿ��ס��¼״̬
        /// </summary>
        [HiddenInput]
        [BindProperty(SupportsGet = true)]
        public bool RememberMe { get; set; }

        public VerifyCodeModel()
        {
            LocalizationResourceType = typeof(AccountResource);
        }

        public virtual IActionResult OnGet()
        {
            Input = new VerifyCodeInputModel();

            return Page();
        }

        public virtual async Task<IActionResult> OnPostAsync()
        {
            // ��֤�û���¼״̬
            var user = await SignInManager.GetTwoFactorAuthenticationUserAsync();
            if (user == null)
            {
                Alerts.Warning(L["TwoFactorAuthenticationInvaidUser"]);
                return Page();
            }
            // ˫���ص�¼
            var result = await SignInManager.TwoFactorSignInAsync(Provider, Input.VerifyCode, RememberMe, Input.RememberBrowser);
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

    public class VerifyCodeInputModel
    {
        /// <summary>
        /// �Ƿ���������м�ס��¼״̬
        /// </summary>
        public bool RememberBrowser { get; set; }
        /// <summary>
        /// ���͵���֤��
        /// </summary>
        [Required]
        public string VerifyCode { get; set; }
    }
}
