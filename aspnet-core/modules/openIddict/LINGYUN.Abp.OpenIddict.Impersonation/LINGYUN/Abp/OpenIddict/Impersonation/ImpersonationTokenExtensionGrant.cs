using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OpenIddict.Abstractions;
using OpenIddict.Server;
using OpenIddict.Server.AspNetCore;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Identity;
using Volo.Abp.MultiTenancy;
using Volo.Abp.OpenIddict;
using Volo.Abp.OpenIddict.ExtensionGrantTypes;
using Volo.Abp.OpenIddict.Localization;
using Volo.Abp.Security.Claims;
using Volo.Abp.Users;
using IdentityUser = Volo.Abp.Identity.IdentityUser;
using SignInResult = Microsoft.AspNetCore.Mvc.SignInResult;

namespace LINGYUN.Abp.OpenIddict.Impersonation;

public class ImpersonationTokenExtensionGrant : ITokenExtensionGrant
{
    public string Name => ImpersonationTokenExtensionGrantConsts.GrantType;

    public async virtual Task<IActionResult> HandleAsync(ExtensionGrantContext context)
    {
        var localizer = GetRequiredService<IStringLocalizer<AbpOpenIddictResource>>(context);

        var principal = await ValidateAccessTokenAsync(context);
        if (principal == null)
        {
            return ForbidWithError(
                OpenIddictConstants.Errors.InvalidToken,
                localizer["InvalidAccessToken"]);
        }

        var currentPrincipalAccessor = GetRequiredService<ICurrentPrincipalAccessor>(context);
        using (currentPrincipalAccessor.Change(principal))
        {
            return await ProcessImpersonationRequestAsync(context, principal);
        }
    }

    protected async virtual Task<ClaimsPrincipal?> ValidateAccessTokenAsync(ExtensionGrantContext context)
    {
        var factory = GetRequiredService<IOpenIddictServerFactory>(context);
        var dispatcher = GetRequiredService<IOpenIddictServerDispatcher>(context);
        var logger = GetRequiredService<ILogger<ImpersonationTokenExtensionGrant>>(context);

        var transaction = await factory.CreateTransactionAsync();
        transaction.EndpointType = OpenIddictServerEndpointType.Introspection;
        transaction.Request = new OpenIddictRequest
        {
            ClientId = context.Request.ClientId,
            ClientSecret = context.Request.ClientSecret,
            Token = context.Request.AccessToken,
            TokenTypeHint = OpenIddictConstants.TokenTypeHints.AccessToken
        };

        var authContext = new OpenIddictServerEvents.ProcessAuthenticationContext(transaction);
        await dispatcher.DispatchAsync(authContext);

        if (authContext.IsRejected)
        {
            logger.LogWarning("Token introspection rejected: {Error}", authContext.Error);
            return null;
        }

        return authContext.GenericTokenPrincipal;
    }

    protected async virtual Task<IActionResult> ProcessImpersonationRequestAsync(
        ExtensionGrantContext context,
        ClaimsPrincipal principal)
    {
        var request = context.Request;
        var localizer = GetRequiredService<IStringLocalizer<AbpOpenIddictResource>>(context);
        var currentPrincipalAccessor = GetRequiredService<ICurrentPrincipalAccessor>(context);

        var isCurrentlyImpersonating = currentPrincipalAccessor.Principal.IsImpersonating();

        var impersonationRequest = ParseImpersonationRequest(request);

        if (isCurrentlyImpersonating && !impersonationRequest.HasImpersonationTarget())
        {
            return await RevertToOriginalUserAsync(context, principal);
        }

        if (isCurrentlyImpersonating && impersonationRequest.HasImpersonationTarget())
        {
            return ForbidWithError(
                OpenIddictConstants.Errors.InvalidRequest,
                localizer["NestedImpersonationNotAllowed"]);
        }

        if (impersonationRequest.UserDelegationId.HasValue)
        {
            return await HandleDelegatedImpersonationAsync(context, principal, impersonationRequest.UserDelegationId.Value);
        }

        if (impersonationRequest.IsTenantImpersonation())
        {
            return await HandleTenantImpersonationAsync(context, principal, impersonationRequest);
        }

        if (impersonationRequest.IsUserImpersonation())
        {
            return await HandleUserImpersonationAsync(context, principal, impersonationRequest);
        }

        return ForbidWithError(
            OpenIddictConstants.Errors.InvalidRequest,
            localizer["InvalidImpersonationRequest"]);
    }

    protected virtual ImpersonationRequest ParseImpersonationRequest(OpenIddictRequest request)
    {
        return new ImpersonationRequest
        {
            UserId = ParseGuidParameter(request, "UserId"),
            TenantId = ParseGuidParameter(request, "TenantId"),
            UserDelegationId = ParseGuidParameter(request, "UserDelegationId"),
            TenantUserName = request.GetParameter("TenantUserName")?.ToString()
        };
    }

    protected async virtual Task<IActionResult> HandleUserImpersonationAsync(
        ExtensionGrantContext context,
        ClaimsPrincipal principal,
        ImpersonationRequest request)
    {
        var localizer = GetRequiredService<IStringLocalizer<AbpOpenIddictResource>>(context);
        var currentUser = GetRequiredService<ICurrentUser>(context);
        var currentTenant = GetRequiredService<ICurrentTenant>(context);
        var userManager = GetRequiredService<IdentityUserManager>(context);
        var permissionChecker = GetRequiredService<IPermissionChecker>(context);
        var options = GetRequiredService<IOptions<OpenIddictImpersonationOptions>>(context);

        using (currentTenant.Change(request.TenantId))
        {
            if (currentUser.Id == request.UserId)
            {
                return ForbidWithError(
                    OpenIddictConstants.Errors.InvalidRequest,
                    localizer["CannotImpersonateYourself"]);
            }

            var impersonationPermission = options.Value.ImpersonationPermission;
            if (!impersonationPermission.IsNullOrWhiteSpace() &&
                !await permissionChecker.IsGrantedAsync(impersonationPermission))
            {
                return ForbidWithError(
                    OpenIddictConstants.Errors.InsufficientAccess,
                    localizer["InsufficientImpersonationPermission"]);
            }

            var targetUser = await userManager.FindByIdAsync(request.UserId!.Value.ToString()!);
            if (targetUser == null)
            {
                return ForbidWithError(
                    OpenIddictConstants.Errors.InvalidRequest,
                    localizer["TargetUserNotFound", request.UserId.Value]);
            }

            return await CreateImpersonatedSignInResult(context, principal, targetUser, request.TenantId);
        }
    }

    protected async virtual Task<IActionResult> HandleTenantImpersonationAsync(
        ExtensionGrantContext context,
        ClaimsPrincipal principal,
        ImpersonationRequest request)
    {
        var localizer = GetRequiredService<IStringLocalizer<AbpOpenIddictResource>>(context);
        var currentTenant = GetRequiredService<ICurrentTenant>(context);
        var userManager = GetRequiredService<IdentityUserManager>(context);
        var permissionChecker = GetRequiredService<IPermissionChecker>(context);
        var options = GetRequiredService<IOptions<OpenIddictImpersonationOptions>>(context);

        using (currentTenant.Change(null))
        {
            var impersonationPermission = options.Value.ImpersonationTenantPermission;
            if (!impersonationPermission.IsNullOrWhiteSpace() &&
                !await permissionChecker.IsGrantedAsync(impersonationPermission))
            {
                return ForbidWithError(
                    OpenIddictConstants.Errors.InsufficientAccess,
                    localizer["RequirePermissionToImpersonateUser"]);
            }
        }

        using (currentTenant.Change(request.TenantId))
        {
            var adminUserName = request.TenantUserName ?? options.Value.DefaultTenantAdminUserName;
            if (adminUserName.IsNullOrWhiteSpace())
            {
                return ForbidWithError(
                    OpenIddictConstants.Errors.InvalidRequest,
                    localizer["TenantAdminUserNameNotSpecified"]);
            }

            var adminUser = await userManager.FindByNameAsync(adminUserName);
            if (adminUser == null)
            {
                return ForbidWithError(
                    OpenIddictConstants.Errors.InvalidRequest,
                    localizer["TenantAdminUserNotFound", adminUserName]);
            }

            return await CreateImpersonatedSignInResult(context, principal, adminUser, request.TenantId);
        }
    }

    protected async virtual Task<IActionResult> HandleDelegatedImpersonationAsync(
        ExtensionGrantContext context,
        ClaimsPrincipal principal,
        Guid userDelegationId)
    {
        var localizer = GetRequiredService<IStringLocalizer<AbpOpenIddictResource>>(context);
        var currentUser = GetRequiredService<ICurrentUser>(context);
        var currentTenant = GetRequiredService<ICurrentTenant>(context);
        var userManager = GetRequiredService<IdentityUserManager>(context);
        var delegationManager = GetRequiredService<IdentityUserDelegationManager>(context);

        var delegation = await delegationManager.FindActiveDelegationByIdAsync(userDelegationId);
        if (delegation == null)
        {
            return ForbidWithError(
                OpenIddictConstants.Errors.InvalidRequest,
                localizer["InvalidUserDelegation"]);
        }

        if (currentUser.Id != delegation.TargetUserId)
        {
            return ForbidWithError(
                OpenIddictConstants.Errors.InsufficientAccess,
                localizer["NotAuthorizedForDelegation"]);
        }

        if (delegation.SourceUserId == currentUser.Id)
        {
            return ForbidWithError(
                OpenIddictConstants.Errors.InvalidRequest,
                localizer["CannotImpersonateYourself"]);
        }

        var sourceUser = await userManager.FindByIdAsync(delegation.SourceUserId.ToString());
        if (sourceUser == null)
        {
            return ForbidWithError(
                OpenIddictConstants.Errors.InvalidRequest,
                localizer["DelegationSourceUserNotFound"]);
        }

        return await CreateImpersonatedSignInResult(
            context,
            principal,
            sourceUser,
            currentTenant.Id,
            "DelegatedImpersonation");
    }

    protected async virtual Task<IActionResult> RevertToOriginalUserAsync(
        ExtensionGrantContext context,
        ClaimsPrincipal principal)
    {
        var localizer = GetRequiredService<IStringLocalizer<AbpOpenIddictResource>>(context);
        var currentPrincipalAccessor = GetRequiredService<ICurrentPrincipalAccessor>(context);
        var userManager = GetRequiredService<IdentityUserManager>(context);
        var currentTenant = GetRequiredService<ICurrentTenant>(context);

        var impersonatorUserId = currentPrincipalAccessor.Principal.FindImpersonatorUserId();
        var impersonatorTenantId = currentPrincipalAccessor.Principal.FindImpersonatorTenantId();

        if (!impersonatorUserId.HasValue)
        {
            return ForbidWithError(
                OpenIddictConstants.Errors.InvalidRequest,
                localizer["NoImpersonationContext"]);
        }

        using (currentTenant.Change(impersonatorTenantId))
        {
            var originalUser = await userManager.FindByIdAsync(impersonatorUserId.Value.ToString());
            if (originalUser == null)
            {
                return ForbidWithError(
                    OpenIddictConstants.Errors.InvalidRequest,
                    localizer["OriginalUserNotFound"]);
            }

            return await CreateAuthenticatedSignInResult(context, principal, originalUser);
        }
    }

    protected async virtual Task<IActionResult> CreateImpersonatedSignInResult(
        ExtensionGrantContext context,
        ClaimsPrincipal currentPrincipal,
        IdentityUser targetUser,
        Guid? tenantId,
        string action = "ImpersonateUser")
    {
        var userClaimsPrincipalFactory = GetRequiredService<IUserClaimsPrincipalFactory<IdentityUser>>(context);
        var currentUser = GetRequiredService<ICurrentUser>(context);
        var currentTenant = GetRequiredService<ICurrentTenant>(context);
        var currentPrincipalAccessor = GetRequiredService<ICurrentPrincipalAccessor>(context);

        var principal = await userClaimsPrincipalFactory.CreateAsync(targetUser);
        var claims = new List<Claim>();

        if (currentUser.Id.HasValue && currentUser.Id?.ToString() != currentPrincipalAccessor.Principal.FindImpersonatorUserId()?.ToString())
        {
            claims.Add(new Claim(AbpClaimTypes.ImpersonatorUserId, currentUser.Id!.Value.ToString()));
            claims.Add(new Claim(AbpClaimTypes.ImpersonatorUserName, currentUser.UserName!));

            if (currentTenant.IsAvailable)
            {
                claims.Add(new Claim(AbpClaimTypes.ImpersonatorTenantId, currentTenant.Id!.Value.ToString()));
            }
        }

        var rememberMeClaim = currentPrincipal.Claims.FirstOrDefault(c => c.Type == AbpClaimTypes.RememberMe);
        if (rememberMeClaim != null)
        {
            claims.Add(rememberMeClaim);
        }

        var identity = principal.Identities.FirstOrDefault();
        if (identity != null && claims.Count != 0)
        {
            identity.AddClaims(claims);
        }

        return await FinalizeSignInAsync(context, principal, currentPrincipal.GetScopes(), action);
    }

    protected async Task<IActionResult> CreateAuthenticatedSignInResult(
        ExtensionGrantContext context,
        ClaimsPrincipal currentPrincipal,
        IdentityUser targetUser)
    {
        var userClaimsPrincipalFactory = GetRequiredService<IUserClaimsPrincipalFactory<IdentityUser>>(context);
        var principal = await userClaimsPrincipalFactory.CreateAsync(targetUser);

        return await FinalizeSignInAsync(context, principal, currentPrincipal.GetScopes(), "RevertImpersonation");
    }

    protected async virtual Task<IActionResult> FinalizeSignInAsync(
        ExtensionGrantContext context,
        ClaimsPrincipal principal,
        IEnumerable<string> scopes,
        string action)
    {
        var currentPrincipalAccessor = GetRequiredService<ICurrentPrincipalAccessor>(context);

        using (currentPrincipalAccessor.Change(principal))
        {
            await SaveSecurityLogAsync(context, principal, action);
        }

        principal.SetScopes(scopes);
        principal.SetResources(await GetResourcesAsync(context, scopes));

        await SetClaimsDestinationsAsync(context, principal);

        return new SignInResult(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme, principal);
    }

    protected async virtual Task SaveSecurityLogAsync(
        ExtensionGrantContext context,
        ClaimsPrincipal principal,
        string action)
    {
        var identitySecurityLogManager = GetRequiredService<IdentitySecurityLogManager>(context);
        var currentUser = GetRequiredService<ICurrentUser>(context);
        var currentTenant = GetRequiredService<ICurrentTenant>(context);

        var logContext = new IdentitySecurityLogContext
        {
            Identity = OpenIddictSecurityLogIdentityConsts.OpenIddict,
            Action = action,
            UserName = principal.FindFirstValue(ClaimTypes.Name),
            ClientId = context.Request.ClientId
        };
        logContext.WithProperty("GrantType", Name);
        logContext.WithProperty("ImpersonatorUserId", currentUser.Id?.ToString());
        logContext.WithProperty("ImpersonatorTenantId", currentTenant.Id?.ToString());

        await identitySecurityLogManager.SaveAsync(logContext);
    }

    protected async virtual Task<IEnumerable<string>> GetResourcesAsync(
        ExtensionGrantContext context,
        IEnumerable<string> scopes)
    {
        var resources = new List<string>();
        if (scopes.Any())
        {
            var scopeManager = GetRequiredService<IOpenIddictScopeManager>(context);

            await foreach (var resource in scopeManager.ListResourcesAsync(scopes.ToImmutableArray()))
            {
                resources.Add(resource);
            }
        }

        return resources;
    }

    protected async virtual Task SetClaimsDestinationsAsync(
        ExtensionGrantContext context,
        ClaimsPrincipal principal)
    {
        var manager = GetRequiredService<AbpOpenIddictClaimsPrincipalManager>(context);
        await manager.HandleAsync(context.Request, principal);
    }

    protected virtual IActionResult ForbidWithError(string error, string description)
    {
        return new ForbidResult(
            new[] { OpenIddictServerAspNetCoreDefaults.AuthenticationScheme },
            new AuthenticationProperties(new Dictionary<string, string?>
            {
                [OpenIddictServerAspNetCoreConstants.Properties.Error] = error,
                [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] = description
            }));
    }

    protected virtual T GetRequiredService<T>(ExtensionGrantContext context) where T : notnull
    {
        return context.HttpContext.RequestServices.GetRequiredService<T>();
    }

    protected static Guid? ParseGuidParameter(OpenIddictRequest request, string name)
    {
        var parameter = request.GetParameter(name);
        if (parameter == null)
        {
            return null;
        }

        return Guid.TryParse(parameter.Value.ToString(), out var result) ? result : null;
    }
}