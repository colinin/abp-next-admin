using LINGYUN.Abp.Account.Dto;
using LINGYUN.Abp.Account.Web.ExternalProviders;
using LINGYUN.Abp.Account.Web.Models;
using LINGYUN.Abp.Identity.QrCode;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Account.Settings;
using Volo.Abp.Account.Web;
using Volo.Abp.Account.Web.Pages.Account;
using Volo.Abp.Identity;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Reflection;
using Volo.Abp.Security.Claims;
using Volo.Abp.Settings;

namespace LINGYUN.Abp.Account.Web.Pages.Account
{
    public class ScanQrCodeLoginModel : AccountPageModel
    {
        [HiddenInput]
        [BindProperty(SupportsGet = true)]
        public string? ReturnUrl { get; set; }

        [HiddenInput]
        [BindProperty(SupportsGet = true)]
        public string? ReturnUrlHash { get; set; }

        [BindProperty(SupportsGet = true)]
        public QrCodeLoginInputModel Input { get; set; } = default!;

        [HiddenInput]
        [BindProperty(SupportsGet = true)]
        public Guid? LinkUserId { get; set; }

        [HiddenInput]
        [BindProperty(SupportsGet = true)]
        public Guid? LinkTenantId { get; set; }

        [HiddenInput]
        [BindProperty(SupportsGet = true)]
        public string? LinkToken { get; set; }

        public bool IsLinkLogin { get; set; }

        public bool EnableLocalLogin { get; set; }
        public IEnumerable<ExternalLoginProviderModel> ExternalProviders { get; set; } = default!;
        public bool IsExternalLoginOnly => EnableLocalLogin == false && ExternalProviders?.Count() == 1;
        public IEnumerable<ExternalLoginProviderModel> VisibleExternalProviders => ExternalProviders.Where(x => !x.DisplayName.IsNullOrWhiteSpace());
        protected IdentityDynamicClaimsPrincipalContributorCache IdentityDynamicClaimsPrincipalContributorCache => LazyServiceProvider.LazyGetRequiredService<IdentityDynamicClaimsPrincipalContributorCache>();
        public IIdentityLinkUserAppService IdentityLinkUserAppService => LazyServiceProvider.LazyGetRequiredService<IIdentityLinkUserAppService>();
        protected ICurrentPrincipalAccessor CurrentPrincipalAccessor => LazyServiceProvider.LazyGetRequiredService<ICurrentPrincipalAccessor>();
        protected IExternalProviderService ExternalProviderService => LazyServiceProvider.LazyGetRequiredService<IExternalProviderService>();
        protected IAuthenticationSchemeProvider SchemeProvider => LazyServiceProvider.LazyGetRequiredService<IAuthenticationSchemeProvider>();
        protected AbpAccountOptions AccountOptions => LazyServiceProvider.LazyGetRequiredService<IOptions<AbpAccountOptions>>().Value;
        protected IQrCodeLoginProvider QrCodeLoginProvider => LazyServiceProvider.LazyGetRequiredService<IQrCodeLoginProvider>();

        public virtual async Task<IActionResult> OnGetAsync()
        {
            Input = new QrCodeLoginInputModel();

            ExternalProviders = await GetExternalProviders();
            EnableLocalLogin = await SettingProvider.IsTrueAsync(AccountSettingNames.EnableLocalLogin);

            if (IsExternalLoginOnly)
            {
                return await OnPostExternalLogin(ExternalProviders.First().AuthenticationScheme);
            }

            IsLinkLogin = await VerifyLinkTokenAsync();
            if (IsLinkLogin && CurrentUser.IsAuthenticated)
            {
                await IdentitySecurityLogManager.SaveAsync(new IdentitySecurityLogContext
                {
                    Identity = IdentitySecurityLogIdentityConsts.Identity,
                    Action = IdentitySecurityLogActionConsts.Logout
                });

                await SignInManager.SignOutAsync();

                return Redirect(HttpContext.Request.GetDisplayUrl());
            }

            return Page();
        }

        public async virtual Task<IActionResult> OnPostAsync()
        {
            ValidateModel();
            await IdentityOptions.SetAsync();

            await CheckLocalLoginAsync();
            ExternalProviders = await GetExternalProviders();
            EnableLocalLogin = await SettingProvider.IsTrueAsync(AccountSettingNames.EnableLocalLogin);

            var qrCodeInfo = await QrCodeLoginProvider.GetCodeAsync(Input.Key);
            // 二维码扫描后用户Id不为空
            if (qrCodeInfo == null || qrCodeInfo.Token.IsNullOrWhiteSpace() == true)
            {
                Alerts.Danger(L["QrCode:Invalid"]);
                return Page();
            }

            EnsureTenantCookie(qrCodeInfo.TenantId);
            using (CurrentTenant.Change(qrCodeInfo.TenantId))
            {
                var user = await UserManager.FindByIdAsync(qrCodeInfo.UserId!);
                if (user == null)
                {
                    // TODO: 用户验证无效?
                    Alerts.Danger(L["QrCode:Invalid"]);
                    return Page();
                }

                if (!await UserManager.VerifyUserTokenAsync(user, QrCodeLoginProviderConsts.Name, QrCodeLoginProviderConsts.Purpose, qrCodeInfo.Token))
                {
                    Alerts.Danger(L["QrCode:Invalid"]);
                    return Page();
                }

                if (IsLinkLogin)
                {
                    return await HandleLinkUserLogin(user);
                }

                // TODO: 记住登录
                await SignInManager.SignInAsync(user, true);

                await QrCodeLoginProvider.RemoveAsync(Input.Key);

                await IdentitySecurityLogManager.SaveAsync(new IdentitySecurityLogContext()
                {
                    Identity = QrCodeLoginProviderConsts.Purpose,
                    Action = IdentitySecurityLogActionConsts.LoginSucceeded,
                    UserName = user.UserName
                });

                // Clear the dynamic claims cache.
                await IdentityDynamicClaimsPrincipalContributorCache.ClearAsync(user.Id, user.TenantId);

                return await RedirectSafelyAsync(ReturnUrl!, ReturnUrlHash);
            }
        }

        public virtual async Task<IActionResult> OnPostExternalLogin(string provider)
        {
            var redirectUrl = Url.Page("./Login", pageHandler: "ExternalLoginCallback", values: new { ReturnUrl, ReturnUrlHash });
            var properties = SignInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            properties.Items["scheme"] = provider;

            return await Task.FromResult(Challenge(properties, provider));
        }

        protected virtual async Task CheckLocalLoginAsync()
        {
            if (!await SettingProvider.IsTrueAsync(AccountSettingNames.EnableLocalLogin))
            {
                throw new UserFriendlyException(L["LocalLoginDisabledMessage"]);
            }
        }

        protected async virtual Task<List<ExternalLoginProviderModel>> GetExternalProviders()
        {
            var schemes = await SchemeProvider.GetAllSchemesAsync();
            var externalProviders = await ExternalProviderService.GetAllAsync();

            var externalProviderModels = new List<ExternalLoginProviderModel>();
            foreach (var scheme in schemes)
            {
                if (TryGetExternalLoginProvider(scheme, externalProviders, out var externalLoginProvider) ||
                    scheme.Name.Equals(AccountOptions.WindowsAuthenticationSchemeName, StringComparison.OrdinalIgnoreCase))
                {
                    externalProviderModels.Add(new ExternalLoginProviderModel
                    {
                        Name = externalLoginProvider!.Name,
                        AuthenticationScheme = scheme.Name,
                        DisplayName = externalLoginProvider.DisplayName,
                        ComponentType = externalLoginProvider.ComponentType,
                    });
                }
            }

            return externalProviderModels;
        }

        protected virtual bool TryGetExternalLoginProvider(AuthenticationScheme scheme, List<ExternalLoginProviderModel> externalProviders, out ExternalLoginProviderModel? externalLoginProvider)
        {
            if (ReflectionHelper.IsAssignableToGenericType(scheme.HandlerType, typeof(RemoteAuthenticationHandler<>)))
            {
                externalLoginProvider = externalProviders.FirstOrDefault(x => x.Name == scheme.Name);
                return externalLoginProvider != null;
            }

            externalLoginProvider = null;
            return false;
        }

        protected virtual void EnsureTenantCookie(Guid? tenantId = null)
        {
            if (tenantId.HasValue)
            {
                Response.Cookies.Append(
                   TenantResolverConsts.DefaultTenantKey,
                   tenantId.Value.ToString(),
                   new CookieOptions
                   {
                       Path = "/",
                       HttpOnly = false,
                       IsEssential = true,
                       Expires = DateTimeOffset.Now.AddYears(10)
                   }
               );
            }
            else
            {
                Response.Cookies.Delete(TenantResolverConsts.DefaultTenantKey);
            }
        }

        public async virtual Task<string> GetWithoutLinkReturnUrlAsync(string? returnUrl, string? returnUrlHash = null)
        {
            var redirectUrl = await base.GetRedirectUrlAsync(returnUrl!, returnUrlHash);

            // 使用正则表达式移除 LinkUser 参数
            redirectUrl = Regex.Replace(redirectUrl, @"[&?]LinkToken=[^&]*", "");
            redirectUrl = Regex.Replace(redirectUrl, @"[&?]LinkUserId=[^&]*", "");
            redirectUrl = Regex.Replace(redirectUrl, @"[&?]LinkTenantId=[^&]*", "");
            redirectUrl = Regex.Replace(redirectUrl, @"[&?]linkUserId=[^&]*", "");
            redirectUrl = Regex.Replace(redirectUrl, @"[&?]linkToken=[^&]*", "");

            return redirectUrl;
        }

        protected async virtual Task<bool> VerifyLinkTokenAsync()
        {
            if (LinkToken.IsNullOrWhiteSpace() || !LinkUserId.HasValue)
            {
                if (string.IsNullOrWhiteSpace(ReturnUrl))
                {
                    return false;
                }
                var queryString = QueryHelpers.ParseQuery(ReturnUrl);
                queryString.TryGetValue("LinkToken", out var linkTokenVal);
                queryString.TryGetValue("LinkUserId", out var linkUserIdVal);
                queryString.TryGetValue("LinkTenantId", out var linkTenantIdVal);
                if (!linkTokenVal.IsNullOrEmpty() && !linkUserIdVal.IsNullOrEmpty() &&
                    Guid.TryParse(linkUserIdVal.ToString(), out var linkUserId))
                {
                    LinkToken = linkTokenVal.ToString();
                    LinkUserId = linkUserId;
                    if (Guid.TryParse(linkTenantIdVal.ToString(), out var linkTenantId))
                    {
                        LinkTenantId = linkTenantId;
                    }
                }
                else
                {
                    return false;
                }
            }
            // TODO: 替换框架对于空格符号的编码错误
            LinkToken = LinkToken.Replace(" ", "+");
            return await IdentityLinkUserAppService.VerifyLinkTokenAsync(new VerifyLinkTokenInput()
            {
                UserId = LinkUserId.Value,
                TenantId = LinkTenantId,
                Token = LinkToken
            });
        }

        protected async virtual Task<IActionResult> HandleLinkUserLogin(IdentityUser user)
        {
            using (CurrentPrincipalAccessor.Change(await SignInManager.CreateUserPrincipalAsync(user)))
            {
                await IdentityLinkUserAppService.LinkAsync(new LinkUserInput
                {
                    UserId = LinkUserId!.Value,
                    TenantId = LinkTenantId,
                    Token = LinkToken!
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
                    user = await UserManager.GetByIdAsync(LinkUserId.Value);
                    using (CurrentPrincipalAccessor.Change(await SignInManager.CreateUserPrincipalAsync(user)))
                    {
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
                    }
                }

                return RedirectToPage("./LinkLogged", new
                {
                    ReturnUrl,
                    ReturnUrlHash,
                    LinkUserId,
                    LinkTenantId
                });
            }
        }
    }

    public class QrCodeLoginInputModel
    {
        [HiddenInput]
        public string Key { get; set; } = default!;
    }
}
