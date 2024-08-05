using IdentityModel;
using IdentityServer4.Events;
using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Validation;
using LINGYUN.Abp.WeChat.Work;
using LINGYUN.Abp.WeChat.Work.Authorize;
using LINGYUN.Abp.WeChat.Work.Localization;
using LINGYUN.Abp.WeChat.Work.Settings;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Volo.Abp.Guids;
using Volo.Abp.Identity;
using Volo.Abp.IdentityServer;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Security.Claims;
using Volo.Abp.Settings;
using Volo.Abp.Uow;
using IdentityUser = Volo.Abp.Identity.IdentityUser;

namespace LINGYUN.Abp.IdentityServer.WeChat.Work;
public class WeChatWorkGrantValidator : IExtensionGrantValidator
{
    public string GrantType => AbpWeChatWorkGlobalConsts.GrantType;

    protected ILogger<WeChatWorkGrantValidator> Logger { get; }
    protected IEventService EventService { get; }
    protected UserManager<IdentityUser> UserManager { get; }
    protected IdentitySecurityLogManager IdentitySecurityLogManager { get; }
    protected ICurrentTenant CurrentTenant { get; }
    protected ISettingProvider SettingProvider { get; }
    protected IGuidGenerator GuidGenerator { get; }
    protected IStringLocalizer<WeChatWorkResource> WeChatWorkLocalizer { get; }
    protected IWeChatWorkUserFinder WeChatWorkUserFinder { get; }

    public WeChatWorkGrantValidator(
        IEventService eventService,
        ICurrentTenant currentTenant,
        IGuidGenerator guidGenerator,
        ISettingProvider settingProvider,
        UserManager<IdentityUser> userManager,
        IdentitySecurityLogManager identitySecurityLogManager,
        IStringLocalizer<WeChatWorkResource> weChatWorkLocalizer,
        IWeChatWorkUserFinder weChatWorkUserFinder,
        ILogger<WeChatWorkGrantValidator> logger)
    {
        Logger = logger;
        EventService = eventService;
        CurrentTenant = currentTenant;
        GuidGenerator = guidGenerator;
        SettingProvider = settingProvider;
        UserManager = userManager;
        IdentitySecurityLogManager = identitySecurityLogManager;
        WeChatWorkLocalizer = weChatWorkLocalizer;
        WeChatWorkUserFinder = weChatWorkUserFinder;
    }

    [UnitOfWork]
    public async virtual Task ValidateAsync(ExtensionGrantValidationContext context)
    {
        var raw = context.Request.Raw;
        var clientId = raw.Get(OidcConstants.TokenRequest.ClientId);
        var credential = raw.Get(OidcConstants.TokenRequest.GrantType);
        if (credential == null || !credential.Equals(GrantType))
        {
            Logger.LogInformation("Invalid grant type: not allowed");
            context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, WeChatWorkLocalizer["InvalidGrant:GrantTypeInvalid"]);
            return;
        }

        var agentId = raw.Get(AbpWeChatWorkGlobalConsts.AgentId);
        var code = raw.Get(AbpWeChatWorkGlobalConsts.Code);
        if (agentId.IsNullOrWhiteSpace() || code.IsNullOrWhiteSpace())
        {
            Logger.LogInformation("Invalid grant type: agentId or code not found");
            context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, WeChatWorkLocalizer["InvalidGrant:AgentIdOrCodeNotFound"]);
            return;
        }

        try
        {
            var userInfo = await WeChatWorkUserFinder.GetUserInfoAsync(agentId, code);
            var currentUser = await UserManager.FindByLoginAsync(AbpWeChatWorkGlobalConsts.ProviderName, userInfo.UserId);

            if (currentUser == null)
            {
                // TODO 检查启用用户注册是否有必要引用账户模块
                if (!await SettingProvider.IsTrueAsync("Abp.Account.IsSelfRegistrationEnabled") ||
                    !await SettingProvider.IsTrueAsync(WeChatWorkSettingNames.EnabledQuickLogin))
                {
                    Logger.LogWarning("Invalid grant type: wechat work user {userId} not register", userInfo.UserId);
                    context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, WeChatWorkLocalizer["InvalidGrant:UserIdNotRegister"]);

                    return;
                }
                var userName = "wxid-work" + userInfo.UserId.ToMd5().ToLower();
                var userEmail = $"{userName}@{CurrentTenant.Name ?? "default"}.io";

                currentUser = new IdentityUser(GuidGenerator.Create(), userName, userEmail, CurrentTenant.Id);

                (await UserManager.CreateAsync(currentUser)).CheckErrors();
                (await UserManager.AddLoginAsync(
                    currentUser,
                    new UserLoginInfo(
                        AbpWeChatWorkGlobalConsts.ProviderName,
                        userInfo.UserId,
                        AbpWeChatWorkGlobalConsts.DisplayName))).CheckErrors();
            }

            if (await UserManager.IsLockedOutAsync(currentUser))
            {
                Logger.LogInformation("Authentication failed for username: {username}, reason: locked out", currentUser.UserName);
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, WeChatWorkLocalizer["Volo.Abp.Identity:UserLockedOut"]);

                await SaveSecurityLogAsync(context, currentUser, IdentityServerSecurityLogActionConsts.LoginLockedout);

                return;
            }

            await EventService.RaiseAsync(new UserLoginSuccessEvent(currentUser.UserName, currentUser.Id.ToString(), currentUser.Name, clientId: clientId));

            await SetSuccessResultAsync(context, currentUser);
        }
        catch (AbpWeChatWorkException wwe)
        {
            Logger.LogInformation("Invalid get user info: {message}", wwe.Message);
            var error = WeChatWorkLocalizer[wwe.Code];
            context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, error.ResourceNotFound ? wwe.Code : error.Value);
            return;
        }
    }

    protected async virtual Task SetSuccessResultAsync(ExtensionGrantValidationContext context, IdentityUser user)
    {
        var sub = await UserManager.GetUserIdAsync(user);

        Logger.LogInformation("Credentials validated for username: {username}", user.UserName);

        var additionalClaims = new List<Claim>();

        await AddCustomClaimsAsync(additionalClaims, user, context);

        context.Result = new GrantValidationResult(
            sub,
            AbpWeChatWorkGlobalConsts.AuthenticationMethod,
            additionalClaims.ToArray()
        );

        await SaveSecurityLogAsync(
            context,
            user,
            IdentityServerSecurityLogActionConsts.LoginSucceeded);
    }

    protected async virtual Task SaveSecurityLogAsync(
        ExtensionGrantValidationContext context,
        IdentityUser user,
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

        await IdentitySecurityLogManager.SaveAsync(logContext);
    }

    protected virtual Task<string> FindClientIdAsync(ExtensionGrantValidationContext context)
    {
        return Task.FromResult(context.Request?.Client?.ClientId);
    }

    protected virtual Task AddCustomClaimsAsync(
        List<Claim> customClaims,
        IdentityUser user,
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

        return Task.CompletedTask;
    }
}
