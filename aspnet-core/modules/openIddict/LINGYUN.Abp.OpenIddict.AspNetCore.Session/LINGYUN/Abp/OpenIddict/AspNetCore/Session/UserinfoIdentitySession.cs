using LINGYUN.Abp.Identity.Session;
using OpenIddict.Server;
using System;
using System.Security.Principal;
using System.Threading.Tasks;
using Volo.Abp.MultiTenancy;
using static OpenIddict.Abstractions.OpenIddictConstants;
using static OpenIddict.Server.OpenIddictServerHandlers.Userinfo;

namespace LINGYUN.Abp.OpenIddict.AspNetCore.Session;
/// <summary>
/// UserInfoEndpoint 检查用户会话
/// </summary>
public class UserinfoIdentitySession : IOpenIddictServerHandler<OpenIddictServerEvents.HandleUserinfoRequestContext>
{
    protected ICurrentTenant CurrentTenant { get; }
    protected IIdentitySessionChecker IdentitySessionChecker { get; }

    public static OpenIddictServerHandlerDescriptor Descriptor { get; }
        = OpenIddictServerHandlerDescriptor.CreateBuilder<OpenIddictServerEvents.HandleUserinfoRequestContext>()
            .AddFilter<OpenIddictServerHandlerFilters.RequireUserinfoRequest>()
            .UseScopedHandler<UserinfoIdentitySession>()
            .SetOrder(ValidateUserinfoRequest.Descriptor.Order + 2_000)
            .SetType(OpenIddictServerHandlerType.Custom)
            .Build();

    public UserinfoIdentitySession(
        ICurrentTenant currentTenant,
        IIdentitySessionChecker identitySessionChecker)
    {
        CurrentTenant = currentTenant;
        IdentitySessionChecker = identitySessionChecker;
    }

    public async virtual ValueTask HandleAsync(OpenIddictServerEvents.HandleUserinfoRequestContext context)
    {
        var tenantId = context.Principal.FindTenantId();
        var sessionId = context.Principal.FindSessionId();
        using (CurrentTenant.Change(tenantId))
        {
            if (sessionId.IsNullOrWhiteSpace() ||
            !await IdentitySessionChecker.ValidateSessionAsync(sessionId))
            {
                // Errors.InvalidToken --->  401
                // Errors.ExpiredToken --->  400
                context.Reject(Errors.InvalidToken, "The user session has expired.");
            }
        }
    }
}
