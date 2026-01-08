using LINGYUN.Abp.Identity.Session;
using OpenIddict.Server;
using System;
using System.Security.Principal;
using System.Threading.Tasks;
using Volo.Abp.MultiTenancy;
using static OpenIddict.Abstractions.OpenIddictConstants;
using static OpenIddict.Server.OpenIddictServerHandlers.UserInfo;

namespace LINGYUN.Abp.OpenIddict.AspNetCore.Session;
/// <summary>
/// UserInfoEndpoint 检查用户会话
/// </summary>
public class UserInfoIdentitySession : IOpenIddictServerHandler<OpenIddictServerEvents.HandleUserInfoRequestContext>
{
    protected ICurrentTenant CurrentTenant { get; }
    protected IIdentitySessionChecker IdentitySessionChecker { get; }

    public static OpenIddictServerHandlerDescriptor Descriptor { get; }
        = OpenIddictServerHandlerDescriptor.CreateBuilder<OpenIddictServerEvents.HandleUserInfoRequestContext>()
            .AddFilter<OpenIddictServerHandlerFilters.RequireUserInfoRequest>()
            .UseScopedHandler<UserInfoIdentitySession>()
            .SetOrder(ValidateUserInfoRequest.Descriptor.Order + 2_000)
            .SetType(OpenIddictServerHandlerType.Custom)
            .Build();

    public UserInfoIdentitySession(
        ICurrentTenant currentTenant,
        IIdentitySessionChecker identitySessionChecker)
    {
        CurrentTenant = currentTenant;
        IdentitySessionChecker = identitySessionChecker;
    }

    public async virtual ValueTask HandleAsync(OpenIddictServerEvents.HandleUserInfoRequestContext context)
    {
        var tenantId = context.AccessTokenPrincipal.FindTenantId();
        using (CurrentTenant.Change(tenantId))
        {
            if (!await IdentitySessionChecker.ValidateSessionAsync(context.AccessTokenPrincipal))
            {
                // Errors.InvalidToken --->  401
                // Errors.ExpiredToken --->  400
                context.Reject(Errors.InvalidToken, "The user session has expired.");
            }
        }
    }
}
