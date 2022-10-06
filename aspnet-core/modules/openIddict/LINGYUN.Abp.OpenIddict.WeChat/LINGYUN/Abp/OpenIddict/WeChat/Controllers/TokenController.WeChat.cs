using LINGYUN.Abp.WeChat;
using LINGYUN.Abp.WeChat.Localization;
using LINGYUN.Abp.WeChat.OpenId;
using LINGYUN.Abp.WeChat.Settings;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using OpenIddict.Abstractions;
using OpenIddict.Server.AspNetCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Identity;
using Volo.Abp.OpenIddict;
using Volo.Abp.OpenIddict.Controllers;
using Volo.Abp.OpenIddict.ExtensionGrantTypes;
using Volo.Abp.Settings;
using IdentityResource = Volo.Abp.Identity.Localization.IdentityResource;
using IdentityUser = Volo.Abp.Identity.IdentityUser;

namespace LINGYUN.Abp.OpenIddict.WeChat.Controllers;

[IgnoreAntiforgeryToken]
[ApiExplorerSettings(IgnoreApi = true)]
public abstract class WeChatTokenController : AbpOpenIdDictControllerBase, ITokenExtensionGrant
{
    public abstract string Name { get; }
    public abstract string LoginProvider { get; }
    public abstract string AuthenticationMethod { get; }

    protected abstract Task<WeChatOpenId> FindOpenIdAsync(string code);

    protected IWeChatOpenIdFinder WeChatOpenIdFinder => LazyServiceProvider.LazyGetRequiredService<IWeChatOpenIdFinder>();

    protected ISettingProvider SettingProvider => LazyServiceProvider.LazyGetRequiredService<ISettingProvider>();
    protected IdentitySecurityLogManager IdentitySecurityLogManager => LazyServiceProvider.LazyGetRequiredService<IdentitySecurityLogManager>();
    protected IStringLocalizer<IdentityResource> IdentityLocalizer => LazyServiceProvider.LazyGetRequiredService<IStringLocalizer<IdentityResource>>();

    public async virtual Task<IActionResult> HandleAsync(ExtensionGrantContext context)
    {
        LazyServiceProvider = context.HttpContext.RequestServices.GetRequiredService<IAbpLazyServiceProvider>();

        await CheckFeatureAsync(context);

        return await HandleWeChatAsync(context);
    }

    protected async virtual Task<IActionResult> HandleWeChatAsync(ExtensionGrantContext context)
    {
        var wechatCodeParam = context.Request.GetParameter(AbpWeChatGlobalConsts.TokenName);
        var wechatCode = wechatCodeParam.ToString();
        if (wechatCode.IsNullOrWhiteSpace())
        {
            Logger.LogWarning("Invalid grant type: wechat code not found");

            var properties = new AuthenticationProperties(new Dictionary<string, string>
            {
                [OpenIddictServerAspNetCoreConstants.Properties.Error] = OpenIddictConstants.Errors.InvalidGrant,
                [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] = L["InvalidGrant:WeChatCodeNotFound"]
            });

            return Forbid(properties, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
        }

        WeChatOpenId wechatOpenId;
        try
        {
            wechatOpenId = await FindOpenIdAsync(wechatCode);
        }
        catch (AbpWeChatException e)
        {
            var properties = new AuthenticationProperties(new Dictionary<string, string>
            {
                [OpenIddictServerAspNetCoreConstants.Properties.Error] = OpenIddictConstants.Errors.InvalidGrant,
                [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] = e.Message
            });

            return Forbid(properties, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
        }
        var currentUser = await UserManager.FindByLoginAsync(LoginProvider, wechatOpenId.OpenId);
        if (currentUser == null)
        {
            // TODO 检查启用用户注册是否有必要引用账户模块
            if (!await SettingProvider.IsTrueAsync("Abp.Account.IsSelfRegistrationEnabled") ||
                !await SettingProvider.IsTrueAsync(WeChatSettingNames.EnabledQuickLogin))
            {
                Logger.LogWarning("Invalid grant type: wechat openid not register", wechatOpenId.OpenId);

                var properties = new AuthenticationProperties(new Dictionary<string, string>
                {
                    [OpenIddictServerAspNetCoreConstants.Properties.Error] = OpenIddictConstants.Errors.InvalidGrant,
                    [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] = L["InvalidGrant:WeChatNotRegister"]
                });

                return Forbid(properties, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
            }
            var userName = "wxid-" + wechatOpenId.OpenId.ToMd5().ToLower();
            var userEmail = $"{userName}@{CurrentTenant.Name ?? "default"}.io";
            currentUser = new IdentityUser(GuidGenerator.Create(), userName, userEmail, CurrentTenant.Id);
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

            var properties = new AuthenticationProperties(new Dictionary<string, string>
            {
                [OpenIddictServerAspNetCoreConstants.Properties.Error] = OpenIddictConstants.Errors.InvalidGrant,
                [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] = IdentityLocalizer["Volo.Abp.Identity:UserLockedOut"]
            });

            await SaveSecurityLogAsync(context, currentUser, wechatOpenId, OpenIddictSecurityLogActionConsts.LoginLockedout);

            return Forbid(properties, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
        }

        // 登录之后需要更新安全令牌
        (await UserManager.UpdateSecurityStampAsync(currentUser)).CheckErrors();

        await SetSuccessResultAsync(context, currentUser, wechatOpenId);

        return await SetSuccessResultAsync(context, currentUser, wechatOpenId);
    }

    protected virtual Task CheckFeatureAsync(ExtensionGrantContext context)
    {
        return Task.CompletedTask;
    }

    protected async virtual Task<IActionResult> SetSuccessResultAsync(ExtensionGrantContext context, IdentityUser user, WeChatOpenId wechatOpenId)
    {
        Logger.LogInformation("Credentials validated for username: {username}", user.UserName);

        var principal = await SignInManager.CreateUserPrincipalAsync(user);

        principal.SetScopes(context.Request.GetScopes());
        principal.SetResources(await GetResourcesAsync(context.Request.GetScopes()));

        await SetClaimsDestinationsAsync(principal);

        await SaveSecurityLogAsync(
            context,
            user,
            wechatOpenId,
            OpenIddictSecurityLogActionConsts.LoginSucceeded);

        return SignIn(principal, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
    }

    protected async virtual Task SaveSecurityLogAsync(
        ExtensionGrantContext context,
        IdentityUser user,
        WeChatOpenId wechatOpenId,
        string action)
    {
        var logContext = new IdentitySecurityLogContext
        {
            Identity = OpenIddictSecurityLogIdentityConsts.OpenIddict,
            Action = action,
            UserName = user.UserName,
            ClientId = await FindClientIdAsync(context)
        };
        logContext.WithProperty("GrantType", Name);
        logContext.WithProperty("Provider", LoginProvider);
        logContext.WithProperty("Method", AuthenticationMethod);

        await IdentitySecurityLogManager.SaveAsync(logContext);
    }

    protected virtual Task<string> FindClientIdAsync(ExtensionGrantContext context)
    {
        return Task.FromResult(context.Request?.ClientId);
    }
}
