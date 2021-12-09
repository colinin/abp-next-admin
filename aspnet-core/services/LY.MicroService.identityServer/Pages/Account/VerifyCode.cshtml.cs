using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Volo.Abp.Account.Localization;
using Volo.Abp.Account.Web.Pages.Account;

namespace LY.MicroService.IdentityServer.Pages.Account
{
    public class VerifyCodeModel : AccountPageModel
    {
        [BindProperty]
        public VerifyCodeInputModel Input { get; set; }
        /// <summary>
        /// 双因素认证提供程序
        /// </summary>
        [HiddenInput]
        [BindProperty(SupportsGet = true)]
        public string Provider { get; set; }
        /// <summary>
        /// 重定向Url
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
        /// 是否记住登录状态
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
            // 验证用户登录状态
            var user = await SignInManager.GetTwoFactorAuthenticationUserAsync();
            if (user == null)
            {
                Alerts.Warning(L["TwoFactorAuthenticationInvaidUser"]);
                return Page();
            }
            // 双因素登录
            var result = await SignInManager.TwoFactorSignInAsync(Provider, Input.VerifyCode, RememberMe, Input.RememberBrowser);
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

    public class VerifyCodeInputModel
    {
        /// <summary>
        /// 是否在浏览器中记住登录状态
        /// </summary>
        public bool RememberBrowser { get; set; }
        /// <summary>
        /// 发送的验证码
        /// </summary>
        [Required]
        public string VerifyCode { get; set; }
    }
}
