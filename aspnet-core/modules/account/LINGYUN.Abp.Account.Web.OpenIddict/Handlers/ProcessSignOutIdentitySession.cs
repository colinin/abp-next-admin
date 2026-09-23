using LINGYUN.Abp.Identity.Session;
using OpenIddict.Server;
using System;
using System.Security.Principal;
using System.Threading.Tasks;
using Volo.Abp.Users;

namespace LINGYUN.Abp.Account.Web.OpenIddict.Handlers;

/// <summary>
/// 用户退出登录终止会话
/// </summary>
public class ProcessSignOutIdentitySession : IOpenIddictServerHandler<OpenIddictServerEvents.ProcessSignOutContext>
{
    protected ICurrentUser CurrentUser { get; }
    protected IIdentitySessionManager IdentitySessionManager { get; }

    public static OpenIddictServerHandlerDescriptor Descriptor { get; }
        = OpenIddictServerHandlerDescriptor.CreateBuilder<OpenIddictServerEvents.ProcessSignOutContext>()
            .AddFilter<OpenIddictServerHandlerFilters.RequireEndSessionRequest>()
            .UseScopedHandler<ProcessSignOutIdentitySession>()
            .SetOrder(OpenIddictServerHandlers.ValidateSignOutDemand.Descriptor.Order + 1_000)
            .SetType(OpenIddictServerHandlerType.Custom)
            .Build();

    public ProcessSignOutIdentitySession(
        ICurrentUser currentUser,
        IIdentitySessionManager identitySessionManager)
    {
        CurrentUser = currentUser;
        IdentitySessionManager = identitySessionManager;
    }

    public async virtual ValueTask HandleAsync(OpenIddictServerEvents.ProcessSignOutContext context)
    {
        var sessionId = CurrentUser.FindSessionId();
        if (!sessionId.IsNullOrWhiteSpace())
        {
            await IdentitySessionManager.RevokeSessionAsync(sessionId);
        }
    }
}

