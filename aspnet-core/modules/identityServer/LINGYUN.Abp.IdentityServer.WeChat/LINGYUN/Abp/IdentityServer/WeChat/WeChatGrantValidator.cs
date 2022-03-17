using IdentityModel;
using IdentityServer4.Events;
using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Validation;
using LINGYUN.Abp.WeChat;
using LINGYUN.Abp.WeChat.Localization;
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
using Volo.Abp.IdentityServer;
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
        protected IdentitySecurityLogManager IdentitySecurityLogManager { get; }
        protected UserManager<IdentityUser> UserManager { get; }
        protected IStringLocalizer<WeChatResource> WeChatLocalizer { get; }
        protected IStringLocalizer<IdentityResource> IdentityLocalizer { get; }
        protected IStringLocalizer<AbpIdentityServerResource> IdentityServerLocalizer { get; }

        public WeChatGrantValidator(
            IEventService eventService,
            IWeChatOpenIdFinder weChatOpenIdFinder,
            UserManager<IdentityUser> userManager,
            IIdentityUserRepository userRepository,
            IdentitySecurityLogManager identitySecurityLogManager,
            IStringLocalizer<WeChatResource> wechatLocalizer,
            IStringLocalizer<IdentityResource> identityLocalizer,
            IStringLocalizer<AbpIdentityServerResource> identityServerLocalizer)
        {
            EventService = eventService;
            UserManager = userManager;
            UserRepository = userRepository;
            IdentitySecurityLogManager = identitySecurityLogManager;
            WeChatOpenIdFinder = weChatOpenIdFinder;
            WeChatLocalizer = wechatLocalizer;
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

            var wechatCode = raw.Get(AbpWeChatGlobalConsts.TokenName);
            if (wechatCode.IsNullOrWhiteSpace() || wechatCode.IsNullOrWhiteSpace())
            {
                Logger.LogWarning("Invalid grant type: wechat code not found");
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, IdentityServerLocalizer["InvalidGrant:WeChatCodeNotFound"]);
                return;
            }

            WeChatOpenId wechatOpenId;
            try
            {
                wechatOpenId = await FindOpenIdAsync(wechatCode);
            }
            catch(AbpWeChatException e)
            {
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, e.Message);
                return;
            }
            var currentUser = await UserManager.FindByLoginAsync(LoginProvider, wechatOpenId.OpenId);
            if (currentUser == null)
            {
                // 检查是否允许自注册
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

            // 检查是否已锁定
            if (await UserManager.IsLockedOutAsync(currentUser))
            {
                Logger.LogInformation("Authentication failed for username: {username}, reason: locked out", currentUser.UserName);
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, IdentityLocalizer["Volo.Abp.Identity:UserLockedOut"]);

                await SaveSecurityLogAsync(context, currentUser, wechatOpenId, IdentityServerSecurityLogActionConsts.LoginLockedout);

                return;
            }

            await EventService.RaiseAsync(new UserLoginSuccessEvent(currentUser.UserName, wechatOpenId.OpenId, null));

            // 登录之后需要更新安全令牌
            (await UserManager.UpdateSecurityStampAsync(currentUser)).CheckErrors();

            await SetSuccessResultAsync(context, currentUser, wechatOpenId);
        }

        protected virtual Task<bool> CheckFeatureAsync(ExtensionGrantValidationContext context)
        {
            return Task.FromResult(true);
        }

        protected virtual async Task SetSuccessResultAsync(ExtensionGrantValidationContext context, IdentityUser user, WeChatOpenId wechatOpenId)
        {
            var sub = await UserManager.GetUserIdAsync(user);

            Logger.LogInformation("Credentials validated for username: {username}", user.UserName);

            var additionalClaims = new List<Claim>();

            await AddCustomClaimsAsync(additionalClaims, user, wechatOpenId, context);

            context.Result = new GrantValidationResult(
                sub,
                AuthenticationMethod,
                additionalClaims.ToArray()
            );

            await SaveSecurityLogAsync(
                context,
                user,
                wechatOpenId,
                IdentityServerSecurityLogActionConsts.LoginSucceeded);
        }

        protected virtual async Task SaveSecurityLogAsync(
            ExtensionGrantValidationContext context,
            IdentityUser user,
            WeChatOpenId wechatOpenId,
            string action)
        {
            var logContext = new IdentitySecurityLogContext
            {
                Identity = IdentityServerSecurityLogIdentityConsts.IdentityServer,
                Action = action,
                UserName = user.UserName,
                ClientId = await FindClientIdAsync(context)
            };
            logContext.WithProperty("GrantType", GrantType);
            logContext.WithProperty("Provider", LoginProvider);
            logContext.WithProperty("Method", AuthenticationMethod);

            await IdentitySecurityLogManager.SaveAsync(logContext);
        }

        protected virtual Task<string> FindClientIdAsync(ExtensionGrantValidationContext context)
        {
            return Task.FromResult(context.Request?.Client?.ClientId);
        }

        protected virtual Task AddCustomClaimsAsync(
            List<Claim> customClaims, 
            IdentityUser user, 
            WeChatOpenId wechatOpenId,
            ExtensionGrantValidationContext context)
        {
            if (user.TenantId.HasValue)
            {
                customClaims.Add(
                    new Claim(
                        AbpClaimTypes.TenantId,
                        user.TenantId?.ToString()
                    )
                );
            }

            customClaims.Add(
                new Claim(
                    AbpWeChatClaimTypes.OpenId,
                    wechatOpenId.OpenId));

            if (!wechatOpenId.UnionId.IsNullOrWhiteSpace())
            {
                customClaims.Add(
                    new Claim(
                        AbpWeChatClaimTypes.UnionId, 
                        wechatOpenId.UnionId));
            }

            return Task.CompletedTask;
        }
    }
}
