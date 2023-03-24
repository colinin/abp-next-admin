using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using OpenIddict.Abstractions;
using OpenIddict.Server;
using OpenIddict.Server.AspNetCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using Volo.Abp.Identity;
using Volo.Abp.MultiTenancy;
using Volo.Abp.OpenIddict;
using Volo.Abp.OpenIddict.ExtensionGrantTypes;
using Volo.Abp.OpenIddict.Localization;
using Volo.Abp.Security.Claims;
using IdentityUser = Volo.Abp.Identity.IdentityUser;
using SignInResult = Microsoft.AspNetCore.Mvc.SignInResult;

namespace LINGYUN.Abp.OpenIddict.LinkUser;
public class LinkUserTokenExtensionGrant : ITokenExtensionGrant
{
    public string Name => LinkUserTokenExtensionGrantConsts.GrantType;

    public async virtual Task<IActionResult> HandleAsync(ExtensionGrantContext context)
    {
        var localizer = GetRequiredService<IStringLocalizer<AbpOpenIddictResource>>(context);
        // 用户需要传递身份令牌
        var accessTokenParam = context.Request.GetParameter("access_token");
        var accessToken = accessTokenParam.ToString();
        if (accessToken.IsNullOrWhiteSpace())
        {
            var properties = new AuthenticationProperties(new Dictionary<string, string>
            {
                [OpenIddictServerAspNetCoreConstants.Properties.Error] = OpenIddictConstants.Errors.InvalidGrant,
                [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] = localizer["InvalidAccessToken"]
            });

            return Forbid(properties, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
        }

        // 通过身份令牌得到用户信息
        var transaction = await GetRequiredService<IOpenIddictServerFactory>(context).CreateTransactionAsync();
        transaction.EndpointType = OpenIddictServerEndpointType.Userinfo;
        transaction.Request = new OpenIddictRequest
        {
            ClientId = context.Request.ClientId,
            ClientSecret = context.Request.ClientSecret,
            AccessToken = accessToken
        };

        var notification = new OpenIddictServerEvents.ProcessAuthenticationContext(transaction);
        var dispatcher = GetRequiredService<IOpenIddictServerDispatcher>(context);
        await dispatcher.DispatchAsync(notification);

        if (notification.IsRejected)
        {
            return Forbid(
                new AuthenticationProperties(new Dictionary<string, string>
                {
                    [OpenIddictServerAspNetCoreConstants.Properties.Error] = notification.Error ?? OpenIddictConstants.Errors.InvalidRequest,
                    [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] = notification.ErrorDescription,
                    [OpenIddictServerAspNetCoreConstants.Properties.ErrorUri] = notification.ErrorUri
                }),
                OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
        }

        var principal = notification.AccessTokenPrincipal;
        if (principal == null)
        {
            return Forbid(
                new AuthenticationProperties(new Dictionary<string, string>
                {
                    [OpenIddictServerAspNetCoreConstants.Properties.Error] = notification.Error ?? OpenIddictConstants.Errors.InvalidRequest,
                    [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] = notification.ErrorDescription,
                    [OpenIddictServerAspNetCoreConstants.Properties.ErrorUri] = notification.ErrorUri
                }),
                OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
        }

        var userId = principal.FindUserId();
        
        var currentTenant = GetRequiredService<ICurrentTenant>(context);
        var currentPrincipalAccessor = GetRequiredService<ICurrentPrincipalAccessor>(context);
        // 交换令牌
        using (currentPrincipalAccessor.Change(principal))
        {
            var linkUserIdParam = context.Request.GetParameter("LinkUserId");
            if (!linkUserIdParam.HasValue || !Guid.TryParse(linkUserIdParam.Value.ToString(), out var linkUserId))
            {
                var properties = new AuthenticationProperties(new Dictionary<string, string>
                {
                    [OpenIddictServerAspNetCoreConstants.Properties.Error] = OpenIddictConstants.Errors.InvalidGrant,
                    [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] = localizer["InvalidLinkUserId"]
                });

                return Forbid(properties, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
            }

            Guid? linkTenantId = null;
            var linkTenantIdParam = context.Request.GetParameter("LinkTenantId");
            if (linkTenantIdParam.HasValue)
            {
                if (!Guid.TryParse(linkTenantIdParam.Value.ToString(), out var parsedGuid))
                {
                    var properties = new AuthenticationProperties(new Dictionary<string, string>
                    {
                        [OpenIddictServerAspNetCoreConstants.Properties.Error] = OpenIddictConstants.Errors.InvalidGrant,
                        [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] = localizer["InvalidLinkTenantId"]
                    });

                    return Forbid(properties, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
                }

                linkTenantId = parsedGuid;
            }

            var userManager = GetRequiredService<IdentityUserManager>(context);
            var linkUserManager = GetRequiredService<IdentityLinkUserManager>(context);

            var isLinked = await linkUserManager.IsLinkedAsync(
                new IdentityLinkUserInfo(userId.Value, currentTenant.Id),
                new IdentityLinkUserInfo(linkUserId, linkTenantId));

            if (isLinked)
            {
                using (currentTenant.Change(linkTenantId))
                {
                    var linkUser = await userManager.GetByIdAsync(linkUserId);

                    return await SetSuccessResultAsync(context, linkUser);
                }
            }
            else
            {
                var properties = new AuthenticationProperties(new Dictionary<string, string>
                {
                    [OpenIddictServerAspNetCoreConstants.Properties.Error] = OpenIddictConstants.Errors.InvalidGrant,
                    [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] = localizer["TheTargetUserIsNotLinkedToYou"]
                });

                return Forbid(properties, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
            }
        }
    }

    protected virtual T GetRequiredService<T>(ExtensionGrantContext context)
    {
        return context.HttpContext.RequestServices.GetRequiredService<T>();
    }

    protected async virtual Task<IActionResult> SetSuccessResultAsync(ExtensionGrantContext context, IdentityUser user)
    {
        var logger = GetRequiredService<ILogger<LinkUserTokenExtensionGrant>>(context);

        logger.LogInformation("Credentials validated for username: {username}", user.UserName);

        var signInManager = GetRequiredService<SignInManager<IdentityUser>>(context);

        var principal = await signInManager.CreateUserPrincipalAsync(user);

        principal.SetScopes(context.Request.GetScopes());
        principal.SetResources(await GetResourcesAsync(context));

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

        var identitySecurityLogManager = GetRequiredService<IdentitySecurityLogManager>(context);

        await identitySecurityLogManager.SaveAsync(logContext);
    }

    protected virtual Task<string> FindClientIdAsync(ExtensionGrantContext context)
    {
        return Task.FromResult(context.Request.ClientId);
    }

    protected async virtual Task SetClaimsDestinationsAsync(ExtensionGrantContext context, ClaimsPrincipal principal)
    {
        var claimDestinationsManager = GetRequiredService<AbpOpenIddictClaimDestinationsManager>(context);

        await claimDestinationsManager.SetAsync(principal);
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
