using LINGYUN.Abp.WeChat.Work;
using LINGYUN.Abp.WeChat.Work.Authorize;
using LINGYUN.Abp.WeChat.Work.Security.Claims;
using LINGYUN.Abp.WeChat.Work.Settings;
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
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Volo.Abp.Guids;
using Volo.Abp.Identity;
using Volo.Abp.Identity.Localization;
using Volo.Abp.MultiTenancy;
using Volo.Abp.OpenIddict;
using Volo.Abp.OpenIddict.ExtensionGrantTypes;
using Volo.Abp.OpenIddict.Localization;
using Volo.Abp.Settings;
using IdentityUser = Volo.Abp.Identity.IdentityUser;
using SignInResult = Microsoft.AspNetCore.Mvc.SignInResult;

namespace LINGYUN.Abp.OpenIddict.WeChat.Work;

public class WeChatWorkTokenExtensionGrant : ITokenExtensionGrant
{
    public string Name => AbpWeChatWorkGlobalConsts.GrantType;

    public async virtual Task<IActionResult> HandleAsync(ExtensionGrantContext context)
    {
        await CheckFeatureAsync(context);

        return await HandleWeChatAsync(context);
    }

    protected async virtual Task<IActionResult> HandleWeChatAsync(ExtensionGrantContext context)
    {
        var logger = GetRequiredService<ILogger<WeChatWorkTokenExtensionGrant>>(context);
        var localizer = GetRequiredService<IStringLocalizer<AbpOpenIddictResource>>(context);

        var agentId = context.Request.GetParameter(AbpWeChatWorkGlobalConsts.AgentId)?.ToString();
        var code = context.Request.GetParameter(AbpWeChatWorkGlobalConsts.Code)?.ToString();
        if (agentId.IsNullOrWhiteSpace() || code.IsNullOrWhiteSpace())
        {
            logger.LogWarning("Invalid grant type: agentId or code not found");

            var properties = new AuthenticationProperties(new Dictionary<string, string>
            {
                [OpenIddictServerAspNetCoreConstants.Properties.Error] = OpenIddictConstants.Errors.InvalidGrant,
                [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] = localizer["InvalidGrant:AgentIdOrCodeNotFound"]
            });

            return Forbid(properties, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
        }

        var userFinder = GetRequiredService<IWeChatWorkUserFinder>(context);

        try
        {
            var userInfo = await userFinder.GetUserInfoAsync(agentId, code);

            var userManager = GetRequiredService<IdentityUserManager>(context);
            var currentUser = await userManager.FindByLoginAsync(AbpWeChatWorkGlobalConsts.ProviderName, userInfo.UserId);

            if (currentUser == null)
            {
                var currentTenant = GetRequiredService<ICurrentTenant>(context);
                var settingProvider = GetRequiredService<ISettingProvider>(context);
                var guidGenerator = GetRequiredService<IGuidGenerator>(context);
                // TODO 检查启用用户注册是否有必要引用账户模块
                if (!await settingProvider.IsTrueAsync("Abp.Account.IsSelfRegistrationEnabled") ||
                    !await settingProvider.IsTrueAsync(WeChatWorkSettingNames.EnabledQuickLogin))
                {
                    logger.LogWarning("Invalid grant type: wechat work user {userId} not register", userInfo.UserId);

                    var properties = new AuthenticationProperties(new Dictionary<string, string>
                    {
                        [OpenIddictServerAspNetCoreConstants.Properties.Error] = OpenIddictConstants.Errors.InvalidGrant,
                        [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] = localizer["InvalidGrant:UserIdNotRegister"]
                    });

                    return Forbid(properties, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
                }
                var userName = "wxid-work" + userInfo.UserId.ToMd5().ToLower();
                var userEmail = $"{userName}@{currentTenant.Name ?? "default"}.io";

                currentUser = new IdentityUser(guidGenerator.Create(), userName, userEmail, currentTenant.Id);

                (await userManager.CreateAsync(currentUser)).CheckErrors();
                (await userManager.AddLoginAsync(
                    currentUser,
                    new UserLoginInfo(
                        AbpWeChatWorkGlobalConsts.ProviderName,
                        userInfo.UserId,
                        AbpWeChatWorkGlobalConsts.DisplayName))).CheckErrors();
            }

            // 检查是否已锁定
            if (await userManager.IsLockedOutAsync(currentUser))
            {
                var identityLocalizer = GetRequiredService<IStringLocalizer<IdentityResource>>(context);

                logger.LogInformation("Authentication failed for username: {username}, reason: locked out", currentUser.UserName);

                var properties = new AuthenticationProperties(new Dictionary<string, string>
                {
                    [OpenIddictServerAspNetCoreConstants.Properties.Error] = OpenIddictConstants.Errors.InvalidGrant,
                    [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] = identityLocalizer["Volo.Abp.Identity:UserLockedOut"]
                });

                await SaveSecurityLogAsync(context, currentUser, OpenIddictSecurityLogActionConsts.LoginLockedout);

                return Forbid(properties, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
            }

            return await SetSuccessResultAsync(context, currentUser, userInfo.UserId, logger);
        }
        catch (AbpWeChatWorkException wwe)
        {
            var properties = new AuthenticationProperties(new Dictionary<string, string>
            {
                [OpenIddictServerAspNetCoreConstants.Properties.Error] = OpenIddictConstants.Errors.InvalidGrant,
                [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] = wwe.Code
            });

            return Forbid(properties, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
        }
    }

    protected virtual Task CheckFeatureAsync(ExtensionGrantContext context)
    {
        return Task.CompletedTask;
    }

    protected virtual T GetRequiredService<T>(ExtensionGrantContext context)
    {
        return context.HttpContext.RequestServices.GetRequiredService<T>();
    }

    protected async virtual Task<IActionResult> SetSuccessResultAsync(
        ExtensionGrantContext context,
        IdentityUser user,
        string userId,
        ILogger<WeChatWorkTokenExtensionGrant> logger)
    {
        logger.LogInformation("Credentials validated for username: {username}", user.UserName);

        var signInManager = GetRequiredService<SignInManager<IdentityUser>>(context);

        var principal = await signInManager.CreateUserPrincipalAsync(user);

        principal.SetScopes(context.Request.GetScopes());
        principal.SetResources(await GetResourcesAsync(context));

        principal.AddClaim(AbpWeChatWorkClaimTypes.UserId, userId);

        await SetClaimsDestinationsAsync(context, principal);

        await SaveSecurityLogAsync(
            context,
            user,
            OpenIddictSecurityLogActionConsts.LoginSucceeded);

        return new SignInResult(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme, principal);
    }

    protected async virtual Task SaveSecurityLogAsync(
        ExtensionGrantContext context,
        IdentityUser user,
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
        logContext.WithProperty("Provider", AbpWeChatWorkGlobalConsts.ProviderName);
        logContext.WithProperty("Method", AbpWeChatWorkGlobalConsts.AuthenticationMethod);

        var identitySecurityLogManager = GetRequiredService<IdentitySecurityLogManager>(context);

        await identitySecurityLogManager.SaveAsync(logContext);
    }

    protected virtual Task<string> FindClientIdAsync(ExtensionGrantContext context)
    {
        return Task.FromResult(context.Request.ClientId);
    }

    protected async virtual Task SetClaimsDestinationsAsync(ExtensionGrantContext context, ClaimsPrincipal principal)
    {
        var openIddictClaimsPrincipalManager = GetRequiredService<AbpOpenIddictClaimsPrincipalManager>(context);

        await openIddictClaimsPrincipalManager.HandleAsync(context.Request, principal);
    }

    protected async virtual Task<IEnumerable<string>> GetResourcesAsync(ExtensionGrantContext context)
    {
        var scopes = context.Request.GetScopes();
        var resources = new List<string>();
        if (!scopes.Any())
        {
            return resources;
        }

        var scopeManager = GetRequiredService<IOpenIddictScopeManager>(context);

        await foreach (var resource in scopeManager.ListResourcesAsync(scopes))
        {
            resources.Add(resource);
        }
        return resources;
    }

    public virtual ForbidResult Forbid(AuthenticationProperties properties, params string[] authenticationSchemes)
    {
        return new ForbidResult(
            authenticationSchemes,
            properties);
    }
}
