
using IdentityModel;
using IdentityServer4.Events;
using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Validation;
using LINGYUN.Abp.WeChat;
using LINGYUN.Abp.WeChat.OpenId;
using LINGYUN.Abp.WeChat.Security.Claims;
using LINGYUN.Abp.WeChat.Settings;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;
using Volo.Abp.Identity;
using Volo.Abp.IdentityServer.Localization;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Security.Claims;
using Volo.Abp.Settings;
using Volo.Abp.Uow;
using IdentityResource = Volo.Abp.Identity.Localization.IdentityResource;
using IdentityUser = Volo.Abp.Identity.IdentityUser;

namespace LINGYUN.Abp.IdentityServer.WeChat
{
    public abstract class WeChatGrantValidator : IExtensionGrantValidator
    {
        public abstract string GrantType { get; }
        public abstract string LoginProvider { get; }
        public abstract string AuthenticationMethod { get; }

        public ILogger Logger { protected get; set; }
        public IAbpLazyServiceProvider ServiceProvider { protected get; set; }

        protected IEventService EventService { get; }
        protected IWeChatOpenIdFinder WeChatOpenIdFinder { get; }
        protected IIdentityUserRepository UserRepository { get; }
        protected UserManager<IdentityUser> UserManager { get; }
        protected IStringLocalizer<IdentityResource> IdentityLocalizer { get; }
        protected IStringLocalizer<AbpIdentityServerResource> IdentityServerLocalizer { get; }

        public WeChatGrantValidator(
            IEventService eventService,
            IWeChatOpenIdFinder weChatOpenIdFinder,
            UserManager<IdentityUser> userManager,
            IIdentityUserRepository userRepository,
            IStringLocalizer<IdentityResource> identityLocalizer,
            IStringLocalizer<AbpIdentityServerResource> identityServerLocalizer)
        {
            EventService = eventService;
            UserManager = userManager;
            UserRepository = userRepository;
            WeChatOpenIdFinder = weChatOpenIdFinder;
            IdentityLocalizer = identityLocalizer;
            IdentityServerLocalizer = identityServerLocalizer;

            Logger = NullLogger<WeChatGrantValidator>.Instance;
        }

        protected abstract Task<WeChatOpenId> FindOpenIdAsync(string code);

        [UnitOfWork]
        public async Task ValidateAsync(ExtensionGrantValidationContext context)
        {
            if (!await CheckFeatureAsync(context))
            {
                return;
            }

            var raw = context.Request.Raw;
            var credential = raw.Get(OidcConstants.TokenRequest.GrantType);
            if (credential == null || !credential.Equals(GrantType))
            {
                Logger.LogWarning("Invalid grant type: not allowed");
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, IdentityServerLocalizer["InvalidGrant:GrantTypeInvalid"]);
                return;
            }
            // TODO: 统一命名规范, 微信认证传递的 code 改为 WeChatOpenIdConsts.WeCahtCodeKey
            var wechatCode = raw.Get(AbpWeChatGlobalConsts.TokenName);
            if (wechatCode.IsNullOrWhiteSpace() || wechatCode.IsNullOrWhiteSpace())
            {
                Logger.LogWarning("Invalid grant type: wechat code not found");
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, IdentityServerLocalizer["InvalidGrant:WeChatCodeNotFound"]);
                return;
            }

            var wechatOpenId = await FindOpenIdAsync(wechatCode);
            var currentUser = await UserManager.FindByLoginAsync(LoginProvider, wechatOpenId.OpenId);
            if (currentUser == null)
            {
                var settingProvider = ServiceProvider.LazyGetRequiredService<ISettingProvider>();
                // TODO 检查启用用户注册是否有必要引用账户模块
                if (!await settingProvider.IsTrueAsync("Abp.Account.IsSelfRegistrationEnabled") ||
                    !await settingProvider.IsTrueAsync(WeChatSettingNames.EnabledQuickLogin))
                {
                    Logger.LogWarning("Invalid grant type: wechat openid not register", wechatOpenId.OpenId);
                    context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, IdentityServerLocalizer["InvalidGrant:WeChatNotRegister"]);
                    return;
                }
                var guiGenerator = ServiceProvider.LazyGetRequiredService<IGuidGenerator>();
                var currentTenant = ServiceProvider.LazyGetRequiredService<ICurrentTenant>();
                var userName = "wxid-" + wechatOpenId.OpenId.ToMd5().ToLower();
                var userEmail = $"{userName}@{currentTenant.Name ?? "default"}.io";
                currentUser = new IdentityUser(guiGenerator.Create(), userName, userEmail, currentTenant.Id);
                (await UserManager.CreateAsync(currentUser)).CheckErrors();
                (await UserManager.AddLoginAsync(
                    currentUser,
                    new UserLoginInfo(
                        LoginProvider,
                        wechatOpenId.OpenId,
                        AbpWeChatGlobalConsts.DisplayName))).CheckErrors();
            }

            if (await UserManager.IsLockedOutAsync(currentUser))
            {
                Logger.LogInformation("Authentication failed for username: {username}, reason: locked out", currentUser.UserName);
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, IdentityLocalizer["Volo.Abp.Identity:UserLockedOut"]);
                return;
            }

            var sub = await UserManager.GetUserIdAsync(currentUser);

            var additionalClaims = new List<Claim>();
            if (currentUser.TenantId.HasValue)
            {
                additionalClaims.Add(new Claim(AbpClaimTypes.TenantId, currentUser.TenantId?.ToString()));
            }
            additionalClaims.Add(new Claim(AbpWeChatClaimTypes.OpenId, wechatOpenId.OpenId));
            if (!wechatOpenId.UnionId.IsNullOrWhiteSpace())
            {
                additionalClaims.Add(new Claim(AbpWeChatClaimTypes.UnionId, wechatOpenId.UnionId));
            }

            await EventService.RaiseAsync(new UserLoginSuccessEvent(currentUser.UserName, wechatOpenId.OpenId, null));

            context.Result = new GrantValidationResult(sub, AuthenticationMethod, additionalClaims.ToArray());

            // 登录之后需要更新安全令牌
            (await UserManager.UpdateSecurityStampAsync(currentUser)).CheckErrors();
        }

        protected virtual Task<bool> CheckFeatureAsync(ExtensionGrantValidationContext context)
        {
            return Task.FromResult(true);
        }
    }
}
