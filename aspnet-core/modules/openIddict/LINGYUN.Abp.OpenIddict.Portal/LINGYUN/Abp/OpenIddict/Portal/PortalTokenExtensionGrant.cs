using LINGYUN.Platform.Portal;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using OpenIddict.Abstractions;
using OpenIddict.Server.AspNetCore;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.MultiTenancy;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Identity;
using Volo.Abp.MultiTenancy;
using Volo.Abp.OpenIddict;
using Volo.Abp.OpenIddict.ExtensionGrantTypes;
using Volo.Abp.Uow;
using Volo.Abp.Validation;
using static Volo.Abp.OpenIddict.Controllers.TokenController;
using IdentityUser = Volo.Abp.Identity.IdentityUser;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace LINGYUN.Abp.OpenIddict.Portal;
public class PortalTokenExtensionGrant : ITokenExtensionGrant
{
    public string Name => PortalTokenExtensionGrantConsts.GrantType;

    protected IAbpLazyServiceProvider LazyServiceProvider { get; set; }
    protected ICurrentTenant CurrentTenant => LazyServiceProvider.LazyGetRequiredService<ICurrentTenant>();
    protected IEnterpriseRepository EnterpriseRepository => LazyServiceProvider.LazyGetRequiredService<IEnterpriseRepository>();
    protected SignInManager<IdentityUser> SignInManager => LazyServiceProvider.LazyGetRequiredService<SignInManager<IdentityUser>>();
    protected IdentityUserManager UserManager => LazyServiceProvider.LazyGetRequiredService<IdentityUserManager>();
    protected IOpenIddictScopeManager ScopeManager => LazyServiceProvider.LazyGetRequiredService<IOpenIddictScopeManager>();
    protected AbpOpenIddictClaimsPrincipalManager OpenIddictClaimsPrincipalManager => LazyServiceProvider.LazyGetRequiredService<AbpOpenIddictClaimsPrincipalManager>();
    protected ILoggerFactory LoggerFactory => LazyServiceProvider.LazyGetRequiredService<ILoggerFactory>();
    protected ILogger Logger => LazyServiceProvider.LazyGetService<ILogger>(provider => LoggerFactory?.CreateLogger(GetType().FullName) ?? NullLogger.Instance);
    protected IServiceScopeFactory ServiceScopeFactory => LazyServiceProvider.LazyGetRequiredService<IServiceScopeFactory>();
    protected IOptions<AbpIdentityOptions> AbpIdentityOptions => LazyServiceProvider.LazyGetRequiredService<IOptions<AbpIdentityOptions>>();
    protected IOptions<IdentityOptions> IdentityOptions => LazyServiceProvider.LazyGetRequiredService<IOptions<IdentityOptions>>();
    protected IOptions<AbpAspNetCoreMultiTenancyOptions> MultiTenancyOptions => LazyServiceProvider.LazyGetRequiredService<IOptions<AbpAspNetCoreMultiTenancyOptions>>();
    protected IdentitySecurityLogManager IdentitySecurityLogManager => LazyServiceProvider.LazyGetRequiredService<IdentitySecurityLogManager>();

    [UnitOfWork]
    public async virtual Task<IActionResult> HandleAsync(ExtensionGrantContext context)
    {
        LazyServiceProvider = context.HttpContext.RequestServices.GetRequiredService<IAbpLazyServiceProvider>();

        var enterprise = context.Request.GetParameter("EnterpriseId")?.ToString();

        Guid? tenantId = null;
        using (CurrentTenant.Change(null))
        {
            if (enterprise.IsNullOrWhiteSpace() || !Guid.TryParse(enterprise, out var enterpriseId))
            {
                // TODO: configurabled
                var enterprises = await EnterpriseRepository.GetEnterprisesInTenantListAsync(25);

                var properties = new AuthenticationProperties(
                    new Dictionary<string, string>
                    {
                        [OpenIddictServerAspNetCoreConstants.Properties.Error] = OpenIddictConstants.Errors.InvalidGrant,
                        [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] = "invalid_enterprise"
                    },
                    new Dictionary<string, object>
                    {
                        // 是否可直接选择的模式
                        { "Enterprises", JsonConvert.SerializeObject(enterprises.Select(x => new { Id = x.Id, Name = x.Name, Logo = x.Logo })) },
                    }
                );
                return Forbid(properties, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
            }

            tenantId = await EnterpriseRepository.GetEnterpriseInTenantAsync(enterpriseId);
        }

        using (CurrentTenant.Change(tenantId))
        {
            AbpMultiTenancyCookieHelper.SetTenantCookie(
                context.HttpContext,
                tenantId,
                MultiTenancyOptions.Value.TenantKey);
            return await HandlePasswordAsync(context);
        }
    }

    protected async virtual Task<IActionResult> HandlePasswordAsync(ExtensionGrantContext context)
    {
        using var scope = ServiceScopeFactory.CreateScope();
        await ReplaceEmailToUsernameOfInputIfNeeds(context.Request);

        IdentityUser user = null;

        if (AbpIdentityOptions.Value.ExternalLoginProviders.Any())
        {
            foreach (var externalLoginProviderInfo in AbpIdentityOptions.Value.ExternalLoginProviders.Values)
            {
                var externalLoginProvider = (IExternalLoginProvider)scope.ServiceProvider
                    .GetRequiredService(externalLoginProviderInfo.Type);

                if (await externalLoginProvider.TryAuthenticateAsync(context.Request.Username, context.Request.Password))
                {
                    user = await UserManager.FindByNameAsync(context.Request.Username);
                    if (user == null)
                    {
                        user = await externalLoginProvider.CreateUserAsync(context.Request.Username, externalLoginProviderInfo.Name);
                    }
                    else
                    {
                        await externalLoginProvider.UpdateUserAsync(user, externalLoginProviderInfo.Name);
                    }

                    return await SetSuccessResultAsync(context, user);
                }
            }
        }

        await IdentityOptions.SetAsync();

        user = await UserManager.FindByNameAsync(context.Request.Username);
        if (user == null)
        {
            Logger.LogInformation("No user found matching username: {username}", context.Request.Username);

            var properties = new AuthenticationProperties(new Dictionary<string, string>
            {
                [OpenIddictServerAspNetCoreConstants.Properties.Error] = OpenIddictConstants.Errors.InvalidGrant,
                [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] = "Invalid username or password!"
            });

            await IdentitySecurityLogManager.SaveAsync(new IdentitySecurityLogContext()
            {
                Identity = OpenIddictSecurityLogIdentityConsts.OpenIddict,
                Action = OpenIddictSecurityLogActionConsts.LoginInvalidUserName,
                UserName = context.Request.Username,
                ClientId = context.Request.ClientId
            });

            return Forbid(properties, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
        }

        var result = await SignInManager.CheckPasswordSignInAsync(user, context.Request.Password, true);
        if (!result.Succeeded)
        {
            await IdentitySecurityLogManager.SaveAsync(new IdentitySecurityLogContext
            {
                Identity = OpenIddictSecurityLogIdentityConsts.OpenIddict,
                Action = result.ToIdentitySecurityLogAction(),
                UserName = context.Request.Username,
                ClientId = context.Request.ClientId
            });

            string errorDescription;
            if (result.IsLockedOut)
            {
                Logger.LogInformation("Authentication failed for username: {username}, reason: locked out", context.Request.Username);
                errorDescription = "The user account has been locked out due to invalid login attempts. Please wait a while and try again.";
            }
            else if (result.IsNotAllowed)
            {
                Logger.LogInformation("Authentication failed for username: {username}, reason: not allowed", context.Request.Username);

                if (user.ShouldChangePasswordOnNextLogin)
                {
                    return await HandleShouldChangePasswordOnNextLoginAsync(context, user, context.Request.Password);
                }

                if (await UserManager.ShouldPeriodicallyChangePasswordAsync(user))
                {
                    return await HandlePeriodicallyChangePasswordAsync(context, user, context.Request.Password);
                }

                errorDescription = "You are not allowed to login! Your account is inactive or needs to confirm your email/phone number.";
            }
            else
            {
                Logger.LogInformation("Authentication failed for username: {username}, reason: invalid credentials", context.Request.Username);
                errorDescription = "Invalid username or password!";
            }

            var properties = new AuthenticationProperties(new Dictionary<string, string>
            {
                [OpenIddictServerAspNetCoreConstants.Properties.Error] = OpenIddictConstants.Errors.InvalidGrant,
                [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] = errorDescription
            });

            return Forbid(properties, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
        }

        if (await IsTfaEnabledAsync(user))
        {
            return await HandleTwoFactorLoginAsync(context, user);
        }

        return await SetSuccessResultAsync(context, user);
    }

    protected virtual async Task ReplaceEmailToUsernameOfInputIfNeeds(OpenIddictRequest request)
    {
        if (!ValidationHelper.IsValidEmailAddress(request.Username))
        {
            return;
        }

        var userByUsername = await UserManager.FindByNameAsync(request.Username);
        if (userByUsername != null)
        {
            return;
        }

        var userByEmail = await UserManager.FindByEmailAsync(request.Username);
        if (userByEmail == null)
        {
            return;
        }

        request.Username = userByEmail.UserName;
    }

    protected virtual async Task<IActionResult> HandleTwoFactorLoginAsync(ExtensionGrantContext context, IdentityUser user)
    {
        var twoFactorProvider = context.Request.GetParameter("TwoFactorProvider")?.ToString();
        var twoFactorCode = context.Request.GetParameter("TwoFactorCode")?.ToString(); ;
        if (!twoFactorProvider.IsNullOrWhiteSpace() && !twoFactorCode.IsNullOrWhiteSpace())
        {
            var providers = await UserManager.GetValidTwoFactorProvidersAsync(user);
            if (providers.Contains(twoFactorProvider) && await UserManager.VerifyTwoFactorTokenAsync(user, twoFactorProvider, twoFactorCode))
            {
                return await SetSuccessResultAsync(context, user);
            }

            Logger.LogInformation("Authentication failed for username: {username}, reason: InvalidAuthenticatorCode", context.Request.Username);

            var properties = new AuthenticationProperties(new Dictionary<string, string>
            {
                [OpenIddictServerAspNetCoreConstants.Properties.Error] = OpenIddictConstants.Errors.InvalidGrant,
                [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] = "Invalid authenticator code!"
            });

            return Forbid(properties, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
        }
        else
        {
            Logger.LogInformation("Authentication failed for username: {username}, reason: RequiresTwoFactor", context.Request.Username);
            var twoFactorToken = await UserManager.GenerateUserTokenAsync(user, TokenOptions.DefaultProvider, nameof(SignInResult.RequiresTwoFactor));

            await IdentitySecurityLogManager.SaveAsync(new IdentitySecurityLogContext
            {
                Identity = OpenIddictSecurityLogIdentityConsts.OpenIddict,
                Action = OpenIddictSecurityLogActionConsts.LoginRequiresTwoFactor,
                UserName = context.Request.Username,
                ClientId = context.Request.ClientId
            });

            var properties = new AuthenticationProperties(
                items: new Dictionary<string, string>
                {
                    [OpenIddictServerAspNetCoreConstants.Properties.Error] = OpenIddictConstants.Errors.InvalidGrant,
                    [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] =
                        nameof(SignInResult.RequiresTwoFactor),
                },
                parameters: new Dictionary<string, object>
                {
                    ["userId"] = user.Id.ToString("N"),
                    ["twoFactorToken"] = twoFactorToken
                });

            return Forbid(properties, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
        }
    }

    protected virtual async Task<IActionResult> HandleShouldChangePasswordOnNextLoginAsync(ExtensionGrantContext context, IdentityUser user, string currentPassword)
    {
        return await HandleChangePasswordAsync(context, user, currentPassword, ChangePasswordType.ShouldChangePasswordOnNextLogin);
    }

    protected virtual async Task<IActionResult> HandlePeriodicallyChangePasswordAsync(ExtensionGrantContext context, IdentityUser user, string currentPassword)
    {
        return await HandleChangePasswordAsync(context, user, currentPassword, ChangePasswordType.PeriodicallyChangePassword);
    }

    protected virtual async Task<IActionResult> HandleChangePasswordAsync(ExtensionGrantContext context, IdentityUser user, string currentPassword, ChangePasswordType changePasswordType)
    {
        var changePasswordToken = context.Request.GetParameter("ChangePasswordToken")?.ToString();
        var newPassword = context.Request.GetParameter("NewPassword")?.ToString();
        if (!changePasswordToken.IsNullOrWhiteSpace() && !currentPassword.IsNullOrWhiteSpace() && !newPassword.IsNullOrWhiteSpace())
        {
            if (await UserManager.VerifyUserTokenAsync(user, TokenOptions.DefaultProvider, changePasswordType.ToString(), changePasswordToken))
            {
                var changePasswordResult = await UserManager.ChangePasswordAsync(user, currentPassword, newPassword);
                if (changePasswordResult.Succeeded)
                {
                    await IdentitySecurityLogManager.SaveAsync(new IdentitySecurityLogContext
                    {
                        Identity = OpenIddictSecurityLogIdentityConsts.OpenIddict,
                        Action = IdentitySecurityLogActionConsts.ChangePassword,
                        UserName = context.Request.Username,
                        ClientId = context.Request.ClientId
                    });

                    if (changePasswordType == ChangePasswordType.ShouldChangePasswordOnNextLogin)
                    {
                        user.SetShouldChangePasswordOnNextLogin(false);
                    }

                    await UserManager.UpdateAsync(user);
                    return await SetSuccessResultAsync(context, user);
                }
                else
                {
                    Logger.LogInformation("ChangePassword failed for username: {username}, reason: {changePasswordResult}", context.Request.Username, changePasswordResult.Errors.Select(x => x.Description).JoinAsString(", "));

                    var properties = new AuthenticationProperties(new Dictionary<string, string>
                    {
                        [OpenIddictServerAspNetCoreConstants.Properties.Error] = OpenIddictConstants.Errors.InvalidGrant,
                        [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] = changePasswordResult.Errors.Select(x => x.Description).JoinAsString(", ")
                    });
                    return Forbid(properties, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
                }
            }
            else
            {
                Logger.LogInformation("Authentication failed for username: {username}, reason: InvalidAuthenticatorCode", context.Request.Username);

                var properties = new AuthenticationProperties(new Dictionary<string, string>
                {
                    [OpenIddictServerAspNetCoreConstants.Properties.Error] = OpenIddictConstants.Errors.InvalidGrant,
                    [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] = "Invalid authenticator code!"
                });

                return Forbid(properties, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
            }
        }
        else
        {
            Logger.LogInformation($"Authentication failed for username: {{{context.Request.Username}}}, reason: {{{changePasswordType.ToString()}}}");

            await IdentitySecurityLogManager.SaveAsync(new IdentitySecurityLogContext
            {
                Identity = OpenIddictSecurityLogIdentityConsts.OpenIddict,
                Action = OpenIddictSecurityLogActionConsts.LoginNotAllowed,
                UserName = context.Request.Username,
                ClientId = context.Request.ClientId
            });

            var properties = new AuthenticationProperties(
                items: new Dictionary<string, string>
                {
                    [OpenIddictServerAspNetCoreConstants.Properties.Error] = OpenIddictConstants.Errors.InvalidGrant,
                    [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] = changePasswordType.ToString()
                },
                parameters: new Dictionary<string, object>
                {
                    ["userId"] = user.Id.ToString("N"),
                    ["changePasswordToken"] = await UserManager.GenerateUserTokenAsync(user, TokenOptions.DefaultProvider, changePasswordType.ToString())
                });

            return Forbid(properties, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
        }
    }

    protected virtual async Task<IActionResult> SetSuccessResultAsync(ExtensionGrantContext context, IdentityUser user)
    {
        // Create a new ClaimsPrincipal containing the claims that
        // will be used to create an id_token, a token or a code.
        var principal = await SignInManager.CreateUserPrincipalAsync(user);

        principal.SetScopes(context.Request.GetScopes());
        principal.SetResources(await GetResourcesAsync(context.Request.GetScopes()));

        await SetClaimsDestinationsAsync(context, principal);

        await IdentitySecurityLogManager.SaveAsync(
            new IdentitySecurityLogContext
            {
                Identity = OpenIddictSecurityLogIdentityConsts.OpenIddict,
                Action = OpenIddictSecurityLogActionConsts.LoginSucceeded,
                UserName = context.Request.Username,
                ClientId = context.Request.ClientId
            }
        );

        return new Microsoft.AspNetCore.Mvc.SignInResult(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme, principal);
    }

    protected virtual async Task<bool> IsTfaEnabledAsync(IdentityUser user)
    {
        return UserManager.SupportsUserTwoFactor &&
               await UserManager.GetTwoFactorEnabledAsync(user) &&
               (await UserManager.GetValidTwoFactorProvidersAsync(user)).Count > 0;
    }

    protected virtual async Task<IEnumerable<string>> GetResourcesAsync(ImmutableArray<string> scopes)
    {
        var resources = new List<string>();
        if (!scopes.Any())
        {
            return resources;
        }

        await foreach (var resource in ScopeManager.ListResourcesAsync(scopes))
        {
            resources.Add(resource);
        }
        return resources;
    }

    protected virtual async Task SetClaimsDestinationsAsync(ExtensionGrantContext context, ClaimsPrincipal principal)
    {
        await OpenIddictClaimsPrincipalManager.HandleAsync(context.Request, principal);
    }

    public virtual ForbidResult Forbid(AuthenticationProperties properties, params string[] authenticationSchemes)
    {
        return new ForbidResult(
            authenticationSchemes,
            properties);
    }
}
