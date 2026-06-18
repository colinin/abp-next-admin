using LINGYUN.Abp.Account.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Volo.Abp.Account.Localization;
using Volo.Abp.Account.Web.Pages.Account;
using Volo.Abp.Identity;
using Volo.Abp.Security.Claims;

namespace LINGYUN.Abp.Account.Web.Pages.Account
{
    public class VerifyCodeModel : AccountPageModel
    {
        protected IdentityDynamicClaimsPrincipalContributorCache IdentityDynamicClaimsPrincipalContributorCache { get; }

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

        #region LinkUser
        [HiddenInput]
        [BindProperty(SupportsGet = true)]
        public Guid? LinkUserId { get; set; }

        [HiddenInput]
        [BindProperty(SupportsGet = true)]
        public Guid? LinkTenantId { get; set; }

        [HiddenInput]
        [BindProperty(SupportsGet = true)]
        public string LinkToken { get; set; }

        protected ICurrentPrincipalAccessor CurrentPrincipalAccessor => LazyServiceProvider.LazyGetRequiredService<ICurrentPrincipalAccessor>();

        public IIdentityLinkUserAppService IdentityLinkUserAppService => LazyServiceProvider.LazyGetRequiredService<IIdentityLinkUserAppService>();

        #endregion

        public VerifyCodeModel(
            IdentityDynamicClaimsPrincipalContributorCache identityDynamicClaimsPrincipalContributorCache)
        {
            IdentityDynamicClaimsPrincipalContributorCache = identityDynamicClaimsPrincipalContributorCache;

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
                if (await VerifyLinkTokenAsync())
                {
                    await HandleLinkUserLogin(user);
                }
                // Clear the dynamic claims cache.
                await IdentityDynamicClaimsPrincipalContributorCache.ClearAsync(user.Id, user.TenantId);

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
                Alerts.Danger(L["TwoFactorAuthenticationInvaidUser"]);// TODO: 更多状态码的解读
                return Page();
            }
        }

        #region LinkUser
        protected async virtual Task HandleLinkUserLogin(IdentityUser user)
        {
            using (CurrentPrincipalAccessor.Change(await SignInManager.CreateUserPrincipalAsync(user)))
            {
                await IdentityLinkUserAppService.LinkAsync(new LinkUserInput
                {
                    UserId = LinkUserId.Value,
                    TenantId = LinkTenantId,
                    Token = LinkToken
                });

                await IdentitySecurityLogManager.SaveAsync(new IdentitySecurityLogContext()
                {
                    Identity = IdentitySecurityLogIdentityConsts.Identity,
                    UserName = user.UserName,
                    Action = "LinkUser",
                    ExtraProperties =
                {
                    { "LinkTenantId",  LinkTenantId },
                    { "LinkUserId", LinkUserId }
                }
                });

                using (CurrentTenant.Change(LinkTenantId))
                {
                    var targetUser = await UserManager.GetByIdAsync(LinkUserId.Value);
                    using (CurrentPrincipalAccessor.Change(await SignInManager.CreateUserPrincipalAsync(targetUser)))
                    {
                        await IdentitySecurityLogManager.SaveAsync(new IdentitySecurityLogContext()
                        {
                            Identity = IdentitySecurityLogIdentityConsts.Identity,
                            UserName = targetUser.UserName,
                            Action = "LinkUser",
                            ExtraProperties =
                        {
                            { "LinkTenantId",  LinkTenantId },
                            { "LinkUserId", LinkUserId }
                        }
                        });
                    }
                }
            }
        }

        protected virtual async Task<bool> VerifyLinkTokenAsync()
        {
            return !LinkToken.IsNullOrWhiteSpace() && LinkUserId != null
                && await IdentityLinkUserAppService.VerifyLinkTokenAsync(new VerifyLinkTokenInput
                {
                    UserId = LinkUserId.Value,
                    TenantId = LinkTenantId,
                    Token = LinkToken
                });
        }
        #endregion
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
