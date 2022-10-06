using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OpenIddict.Abstractions;
using OpenIddict.Server.AspNetCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Identity;
using Volo.Abp.OpenIddict;
using Volo.Abp.OpenIddict.Controllers;
using Volo.Abp.OpenIddict.ExtensionGrantTypes;

using IdentityUser = Volo.Abp.Identity.IdentityUser;
using IIdentityUserRepository = LINGYUN.Abp.Identity.IIdentityUserRepository;

namespace LINGYUN.Abp.OpenIddict.Sms;

[IgnoreAntiforgeryToken]
[ApiExplorerSettings(IgnoreApi = true)]
public class SmsTokenController : AbpOpenIdDictControllerBase, ITokenExtensionGrant
{
    protected IOptions<IdentityOptions> IdentityOptions => LazyServiceProvider.LazyGetRequiredService<IOptions<IdentityOptions>>();
    protected IIdentityUserRepository UserRepository => LazyServiceProvider.LazyGetRequiredService<IIdentityUserRepository>();
    protected IdentitySecurityLogManager IdentitySecurityLogManager => LazyServiceProvider.LazyGetRequiredService<IdentitySecurityLogManager>();

    public string Name => SmsTokenExtensionGrantConsts.GrantType;

    public async virtual Task<IActionResult> HandleAsync(ExtensionGrantContext context)
    {
        LazyServiceProvider = context.HttpContext.RequestServices.GetRequiredService<IAbpLazyServiceProvider>();

        var phoneNumberParam = context.Request.GetParameter(SmsTokenExtensionGrantConsts.ParamName);
        var phoneTokenParam = context.Request.GetParameter(SmsTokenExtensionGrantConsts.TokenName);

        if (!phoneNumberParam.HasValue || !phoneTokenParam.HasValue)
        {
            Logger.LogInformation("Invalid grant type: phone number or token code not found");

            var properties = new AuthenticationProperties(new Dictionary<string, string>
            {
                [OpenIddictServerAspNetCoreConstants.Properties.Error] = OpenIddictConstants.Errors.InvalidGrant,
                [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] = L["InvalidGrant:PhoneOrTokenCodeNotFound"]
            });

            return Forbid(properties, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
        }

        var phoneToken = phoneTokenParam.Value.ToString();
        var phoneNumber = phoneNumberParam.Value.ToString();

        await IdentityOptions.SetAsync();

        var currentUser = await UserRepository.FindByPhoneNumberAsync(phoneNumber);
        if (currentUser == null)
        {
            Logger.LogInformation("Invalid grant type: phone number not register");

            var properties = new AuthenticationProperties(new Dictionary<string, string>
            {
                [OpenIddictServerAspNetCoreConstants.Properties.Error] = OpenIddictConstants.Errors.InvalidGrant,
                [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] = L["InvalidGrant:PhoneNumberNotRegister"]
            });

            return Forbid(properties, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
        }

        if (await UserManager.IsLockedOutAsync(currentUser))
        {
            Logger.LogInformation("Authentication failed for username: {username}, reason: locked out", currentUser.UserName);

            var properties = new AuthenticationProperties(new Dictionary<string, string>
            {
                [OpenIddictServerAspNetCoreConstants.Properties.Error] = OpenIddictConstants.Errors.InvalidGrant,
                [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] = L["Volo.Abp.Identity:UserLockedOut"]
            });

            await SaveSecurityLogAsync(context, currentUser, OpenIddictSecurityLogActionConsts.LoginLockedout);

            return Forbid(properties, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
        }

        var validResult = await UserManager.VerifyTwoFactorTokenAsync(currentUser, TokenOptions.DefaultPhoneProvider, phoneToken);
        if (!validResult)
        {
            Logger.LogWarning("Authentication failed for token: {0}, reason: invalid token", phoneToken);
            string errorDescription;

            var identityResult = await UserManager.AccessFailedAsync(currentUser);
            if (identityResult.Succeeded)
            {
                errorDescription = L["InvalidGrant:PhoneVerifyInvalid"];
            }
            else
            {
                Logger.LogInformation("Authentication failed for username: {username}, reason: access failed", currentUser.UserName);


                errorDescription = identityResult.LocalizeErrors(L);
            }

            await SaveSecurityLogAsync(context, currentUser, SmsTokenExtensionGrantConsts.SecurityCodeFailed);

            var properties = new AuthenticationProperties(new Dictionary<string, string>
            {
                [OpenIddictServerAspNetCoreConstants.Properties.Error] = OpenIddictConstants.Errors.InvalidGrant,
                [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] = errorDescription
            });

            return Forbid(properties, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
        }

        (await UserManager.UpdateSecurityStampAsync(currentUser)).CheckErrors();

        return await SetSuccessResultAsync(context, currentUser);
    }

    protected virtual async Task<IActionResult> SetSuccessResultAsync(ExtensionGrantContext context, IdentityUser user)
    {
        Logger.LogInformation("Credentials validated for username: {username}", user.UserName);

        var principal = await SignInManager.CreateUserPrincipalAsync(user);

        principal.SetScopes(context.Request.GetScopes());
        principal.SetResources(await GetResourcesAsync(context.Request.GetScopes()));

        await SetClaimsDestinationsAsync(principal);

        await SaveSecurityLogAsync(
            context,
            user,
            OpenIddictSecurityLogActionConsts.LoginSucceeded);

        return SignIn(principal, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
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

        await IdentitySecurityLogManager.SaveAsync(logContext);
    }

    protected virtual Task<string> FindClientIdAsync(ExtensionGrantContext context)
    {
        return Task.FromResult(context.Request.ClientId);
    }
}
