using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace LINGYUN.Abp.MicroService.WorkflowService;

/// <summary>
/// Root <see cref="IAuthorizationPolicyProvider"/> for Shells hosting. Elsa 3.8.0-rc2 endpoints
/// declare FastEndpoints permission policies named "epPolicy:&lt;EndpointType&gt;" that CShells
/// registers inside the shell's own container, so ASP.NET's root policy provider (ABP's) cannot
/// resolve them. ABP authenticates /elsa/api with the AuthServer JWT and every validated token
/// carries Elsa's "all permissions" claim (see <see cref="WorkflowManagementNextBlazorServerModule"/>),
/// so epPolicy names are bridged to that grant; other policy names fall back to the defaults.
/// </summary>
public sealed class ElsaShellPermissionPolicyProvider : IAuthorizationPolicyProvider
{
    // Allow-all policy (always-true assertion; AuthorizationPolicy rejects zero requirements).
    private static readonly AuthorizationPolicy AllowAll = new AuthorizationPolicyBuilder().RequireAssertion(_ => true).Build();
    private readonly DefaultAuthorizationPolicyProvider _default = new(Options.Create(new AuthorizationOptions()));

    /// <inheritdoc />
    public Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
    {
        if (!policyName.StartsWith("epPolicy:", StringComparison.Ordinal))
            return _default.GetPolicyAsync(policyName);

        var policy = new AuthorizationPolicyBuilder()
            .RequireAuthenticatedUser()
            .RequireAssertion(context =>
                context.User.HasClaim(Elsa.PermissionNames.ClaimType, Elsa.PermissionNames.All))
            .Build();

        return Task.FromResult<AuthorizationPolicy?>(policy);
    }

    /// <inheritdoc />
    public Task<AuthorizationPolicy?> GetDefaultPolicyAsync() => Task.FromResult<AuthorizationPolicy?>(AllowAll);

    /// <inheritdoc />
    public Task<AuthorizationPolicy?> GetFallbackPolicyAsync() => Task.FromResult<AuthorizationPolicy?>(AllowAll);
}
