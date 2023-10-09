using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OpenIddict.Abstractions;
using OpenIddict.Server.AspNetCore;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Volo.Abp.Identity;
using Volo.Abp.Identity.Localization;
using Volo.Abp.OpenIddict;
using Volo.Abp.OpenIddict.ExtensionGrantTypes;
using Volo.Abp.OpenIddict.Localization;
using IdentityUser = Volo.Abp.Identity.IdentityUser;
using IIdentityUserRepository = LINGYUN.Abp.Identity.IIdentityUserRepository;
using SignInResult = Microsoft.AspNetCore.Mvc.SignInResult;

namespace LINGYUN.Abp.OpenIddict.Sms;
public class SmsTokenExtensionGrant : ITokenExtensionGrant
{
    public string Name => SmsTokenExtensionGrantConsts.GrantType;

    public async virtual Task<IActionResult> HandleAsync(ExtensionGrantContext context)
    {
        var logger = GetRequiredService<ILogger<SmsTokenExtensionGrant>>(context);
        var identityOptions = GetRequiredService<IOptions<IdentityOptions>>(context);
        var localizer = GetRequiredService<IStringLocalizer<AbpOpenIddictResource>>(context);

        var phoneNumberParam = context.Request.GetParameter(SmsTokenExtensionGrantConsts.ParamName);
        var phoneTokenParam = context.Request.GetParameter(SmsTokenExtensionGrantConsts.TokenName);

        if (!phoneNumberParam.HasValue || !phoneTokenParam.HasValue)
        {
            logger.LogInformation("Invalid grant type: phone number or token code not found");

            var properties = new AuthenticationProperties(new Dictionary<string, string>
            {
                [OpenIddictServerAspNetCoreConstants.Properties.Error] = OpenIddictConstants.Errors.InvalidGrant,
                [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] = localizer["InvalidGrant:PhoneOrTokenCodeNotFound"]
            });

            return Forbid(properties, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
        }

        var phoneToken = phoneTokenParam.Value.ToString();
        var phoneNumber = phoneNumberParam.Value.ToString();

        await identityOptions.SetAsync();

        var userRepo = GetRequiredService<IIdentityUserRepository>(context);

        var currentUser = await userRepo.FindByPhoneNumberAsync(phoneNumber);
        if (currentUser == null)
        {
            logger.LogInformation("Invalid grant type: phone number not register");

            var properties = new AuthenticationProperties(new Dictionary<string, string>
            {
                [OpenIddictServerAspNetCoreConstants.Properties.Error] = OpenIddictConstants.Errors.InvalidGrant,
                [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] = localizer["InvalidGrant:PhoneNumberNotRegister"]
            });

            return Forbid(properties, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
        }

        var userManager = GetRequiredService<IdentityUserManager>(context);
        if (await userManager.IsLockedOutAsync(currentUser))
        {
            logger.LogInformation("Authentication failed for username: {username}, reason: locked out", currentUser.UserName);

            var identityLocalizer = GetRequiredService<IStringLocalizer<IdentityResource>>(context);

            var properties = new AuthenticationProperties(new Dictionary<string, string>
            {
                [OpenIddictServerAspNetCoreConstants.Properties.Error] = OpenIddictConstants.Errors.InvalidGrant,
                [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] = identityLocalizer["Volo.Abp.Identity:UserLockedOut"]
            });

            await SaveSecurityLogAsync(context, currentUser, OpenIddictSecurityLogActionConsts.LoginLockedout);

            return Forbid(properties, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
        }

        var validResult = await userManager.VerifyTwoFactorTokenAsync(currentUser, TokenOptions.DefaultPhoneProvider, phoneToken);
        if (!validResult)
        {
            logger.LogWarning("Authentication failed for token: {0}, reason: invalid token", phoneToken);
            string errorDescription;

            var identityResult = await userManager.AccessFailedAsync(currentUser);
            if (identityResult.Succeeded)
            {
                errorDescription = localizer["InvalidGrant:PhoneVerifyInvalid"];
            }
            else
            {
                logger.LogInformation("Authentication failed for username: {username}, reason: access failed", currentUser.UserName);


                errorDescription = identityResult.LocalizeErrors(localizer);
            }

            await SaveSecurityLogAsync(context, currentUser, SmsTokenExtensionGrantConsts.SecurityCodeFailed);

            var properties = new AuthenticationProperties(new Dictionary<string, string>
            {
                [OpenIddictServerAspNetCoreConstants.Properties.Error] = OpenIddictConstants.Errors.InvalidGrant,
                [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] = errorDescription
            });

            return Forbid(properties, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
        }

        return await SetSuccessResultAsync(context, currentUser, logger);
    }

    protected virtual T GetRequiredService<T>(ExtensionGrantContext context)
    {
        return context.HttpContext.RequestServices.GetRequiredService<T>();
    }

    protected async virtual Task<IActionResult> SetSuccessResultAsync(
        ExtensionGrantContext context, 
        IdentityUser user,
        ILogger<SmsTokenExtensionGrant> logger)
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
