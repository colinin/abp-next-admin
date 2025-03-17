using LINGYUN.Abp.Identity.QrCode;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OpenIddict.Abstractions;
using OpenIddict.Server.AspNetCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Volo.Abp.Identity;
using Volo.Abp.MultiTenancy;
using Volo.Abp.OpenIddict;
using Volo.Abp.OpenIddict.ExtensionGrantTypes;
using IdentityUser = Volo.Abp.Identity.IdentityUser;
using SignInResult = Microsoft.AspNetCore.Mvc.SignInResult;

namespace LINGYUN.Abp.OpenIddict.QrCode;

public class QrCodeTokenExtensionGrant : ITokenExtensionGrant
{
    public string Name => QrCodeLoginProviderConsts.GrantType;

    public async virtual Task<IActionResult> HandleAsync(ExtensionGrantContext context)
    {
        var logger = GetRequiredService<ILogger<QrCodeTokenExtensionGrant>>(context);

        // 取出二维码Key
        var qrcodeKey = context.Request.GetParameter("qrcode_key")?.ToString();
        if (qrcodeKey.IsNullOrWhiteSpace())
        {
            logger.LogInformation("The user has not passed the QR code Key required for scanning and login.");

            var properties = new AuthenticationProperties(
                new Dictionary<string, string>
                {
                    [OpenIddictServerAspNetCoreConstants.Properties.Error] = OpenIddictConstants.Errors.InvalidGrant,
                    [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] = "The Qr code is invalid."
                }
            );
            return Forbid(properties, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
        }

        var qrCodeProvider = GetRequiredService<IQrCodeLoginProvider>(context);
        var qrCodeInfo = await qrCodeProvider.GetCodeAsync(qrcodeKey);
        // 二维码扫描后用户Id不为空
        if (qrCodeInfo == null || qrCodeInfo.Token.IsNullOrWhiteSpace() == true)
        {
            logger.LogInformation("The QR code Key {0} is invalid or the user has not scanned the QR code.", qrcodeKey);

            var properties = new AuthenticationProperties(
                new Dictionary<string, string>
                {
                    [OpenIddictServerAspNetCoreConstants.Properties.Error] = OpenIddictConstants.Errors.InvalidGrant,
                    [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] = "The Qr code is invalid."
                }
            );
            return Forbid(properties, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
        }

        Guid? tenantId = null;
        var tenantIdValue = context.Request.GetParameter("tenant_id")?.ToString();
        if (!tenantIdValue.IsNullOrWhiteSpace() && Guid.TryParse(tenantIdValue, out var tenantGuid))
        {
            tenantId = tenantGuid;
        }

        var userManager = GetRequiredService<IdentityUserManager>(context);
        var currentTenant = GetRequiredService<ICurrentTenant>(context);

        using (currentTenant.Change(tenantId))
        {
            var user = await userManager.FindByIdAsync(qrCodeInfo.UserId);
            if (user == null)
            {
                var properties = new AuthenticationProperties(
                    new Dictionary<string, string>
                    {
                        [OpenIddictServerAspNetCoreConstants.Properties.Error] = OpenIddictConstants.Errors.InvalidGrant,
                        [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] = "Invalid user id."
                    }
                );
                return Forbid(properties, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
            }

            if (!await userManager.VerifyUserTokenAsync(user, QrCodeLoginProviderConsts.Name, QrCodeLoginProviderConsts.Purpose, qrCodeInfo.Token))
            {
                logger.LogInformation("Authentication failed for username: {username}, reason: the user token is invalid", user.UserName);

                var properties = new AuthenticationProperties(new Dictionary<string, string>
                {
                    [OpenIddictServerAspNetCoreConstants.Properties.Error] = OpenIddictConstants.Errors.InvalidGrant,
                    [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] = OpenIddictResources.GetResourceString(OpenIddictResources.ID2019),
                });

                await SaveSecurityLogAsync(context, user, OpenIddictSecurityLogActionConsts.LoginLockedout);

                return Forbid(properties, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
            }

            // 检查是否已锁定
            if (await userManager.IsLockedOutAsync(user))
            {
                logger.LogInformation("Authentication failed for username: {username}, reason: locked out", user.UserName);

                var properties = new AuthenticationProperties(new Dictionary<string, string>
                {
                    [OpenIddictServerAspNetCoreConstants.Properties.Error] = OpenIddictConstants.Errors.InvalidGrant,
                    [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] = "The user account has been locked out due to invalid login attempts. Please wait a while and try again.",
                });

                await SaveSecurityLogAsync(context, user, OpenIddictSecurityLogActionConsts.LoginLockedout);

                return Forbid(properties, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
            }

            await qrCodeProvider.RemoveAsync(qrcodeKey);

            return await SetSuccessResultAsync(context, user, logger);
        }
    }

    protected virtual T GetRequiredService<T>(ExtensionGrantContext context)
    {
        return context.HttpContext.RequestServices.GetRequiredService<T>();
    }

    protected virtual Task<string> FindClientIdAsync(ExtensionGrantContext context)
    {
        return Task.FromResult(context.Request.ClientId);
    }

    protected async virtual Task<IActionResult> SetSuccessResultAsync(ExtensionGrantContext context, IdentityUser user, ILogger<QrCodeTokenExtensionGrant> logger)
    {
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

    protected async virtual Task SaveSecurityLogAsync(ExtensionGrantContext context, IdentityUser user, string action)
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

    protected async virtual Task SetClaimsDestinationsAsync(ExtensionGrantContext context, ClaimsPrincipal principal)
    {
        var principalManager = GetRequiredService<AbpOpenIddictClaimsPrincipalManager>(context);

        await principalManager.HandleAsync(context.Request, principal);
    }

    public virtual ForbidResult Forbid(AuthenticationProperties properties, params string[] authenticationSchemes)
    {
        return new ForbidResult(
            authenticationSchemes,
            properties);
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
}
