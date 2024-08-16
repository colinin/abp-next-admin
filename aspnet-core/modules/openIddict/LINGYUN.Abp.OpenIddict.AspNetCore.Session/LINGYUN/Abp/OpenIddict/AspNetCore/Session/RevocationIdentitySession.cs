using LINGYUN.Abp.Identity.Session;
using OpenIddict.Server;
using System;
using System.Security.Principal;
using System.Threading.Tasks;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Abp.OpenIddict.AspNetCore.Session;
/// <summary>
/// 令牌撤销终止用户会话
/// </summary>
public class RevocationIdentitySession : IOpenIddictServerHandler<OpenIddictServerEvents.HandleRevocationRequestContext>
{
    protected ICurrentTenant CurrentTenant { get; }
    protected IIdentitySessionManager IdentitySessionManager { get; }

    public static OpenIddictServerHandlerDescriptor Descriptor { get; }
        = OpenIddictServerHandlerDescriptor.CreateBuilder<OpenIddictServerEvents.HandleRevocationRequestContext>()
            .AddFilter<OpenIddictServerHandlerFilters.RequireRevocationRequest>()
            .UseScopedHandler<RevocationIdentitySession>()
            .SetOrder(OpenIddictServerHandlers.Revocation.RevokeToken.Descriptor.Order + 1_000)
            .SetType(OpenIddictServerHandlerType.Custom)
            .Build();

    public RevocationIdentitySession(
        ICurrentTenant currentTenant,
        IIdentitySessionManager identitySessionManager)
    {
        CurrentTenant = currentTenant;
        IdentitySessionManager = identitySessionManager;
    }

    public async virtual ValueTask HandleAsync(OpenIddictServerEvents.HandleRevocationRequestContext context)
    {
        var tenantId = context.Principal.FindTenantId();
        var sessionId = context.Principal.FindSessionId();
        using (CurrentTenant.Change(tenantId))
        {
            if (!sessionId.IsNullOrWhiteSpace())
            {
                await IdentitySessionManager.RevokeSessionAsync(sessionId);
            }
        }
    }
}
