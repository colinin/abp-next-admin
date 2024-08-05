using LINGYUN.Abp.Identity.Session;
using OpenIddict.Server;
using System;
using System.Security.Principal;
using System.Threading.Tasks;
using static OpenIddict.Abstractions.OpenIddictConstants;
using static OpenIddict.Server.OpenIddictServerHandlers.Userinfo;

namespace LINGYUN.Abp.OpenIddict.AspNetCore.Session;
/// <summary>
/// UserInfoEndpoint 检查用户会话
/// </summary>
public class UserinfoIdentitySession : IOpenIddictServerHandler<OpenIddictServerEvents.HandleUserinfoRequestContext>
{
    protected IIdentitySessionChecker IdentitySessionChecker { get; }

    public static OpenIddictServerHandlerDescriptor Descriptor { get; }
        = OpenIddictServerHandlerDescriptor.CreateBuilder<OpenIddictServerEvents.HandleUserinfoRequestContext>()
            .AddFilter<OpenIddictServerHandlerFilters.RequireUserinfoRequest>()
            .UseScopedHandler<UserinfoIdentitySession>()
            .SetOrder(ValidateAccessTokenParameter.Descriptor.Order + 2_000)
            .SetType(OpenIddictServerHandlerType.Custom)
            .Build();

    public UserinfoIdentitySession(IIdentitySessionChecker identitySessionChecker)
    {
        IdentitySessionChecker = identitySessionChecker;
    }

    public async virtual ValueTask HandleAsync(OpenIddictServerEvents.HandleUserinfoRequestContext context)
    {
        var sessionId = context.Principal.FindSessionId();
        if (sessionId.IsNullOrWhiteSpace() ||
            !await IdentitySessionChecker.ValidateSessionAsync(sessionId))
        {
            // Errors.InvalidToken --->  401
            // Errors.ExpiredToken --->  400
            context.Reject(Errors.InvalidToken, "The user session has expired.");
        }
    }
}
