using LINGYUN.Abp.Identity.Session;
using OpenIddict.Server;
using System;
using System.Threading.Tasks;

namespace LINGYUN.Abp.OpenIddict.AspNetCore.Session;
/// <summary>
/// 用户退出登录终止会话
/// </summary>
public class ProcessSignOutIdentitySession : IOpenIddictServerHandler<OpenIddictServerEvents.ProcessSignOutContext>
{
    protected ISessionInfoProvider SessionInfoProvider { get; }
    protected IIdentitySessionManager IdentitySessionManager { get; }

    public static OpenIddictServerHandlerDescriptor Descriptor { get; }
        = OpenIddictServerHandlerDescriptor.CreateBuilder<OpenIddictServerEvents.ProcessSignOutContext>()
            .AddFilter<OpenIddictServerHandlerFilters.RequireLogoutRequest>()
            .UseScopedHandler<ProcessSignOutIdentitySession>()
            .SetOrder(OpenIddictServerHandlers.ValidateSignOutDemand.Descriptor.Order + 1_000)
            .SetType(OpenIddictServerHandlerType.Custom)
            .Build();

    public ProcessSignOutIdentitySession(
        ISessionInfoProvider sessionInfoProvider,
        IIdentitySessionManager identitySessionManager)
    {
        SessionInfoProvider = sessionInfoProvider;
        IdentitySessionManager = identitySessionManager;
    }

    public async virtual ValueTask HandleAsync(OpenIddictServerEvents.ProcessSignOutContext context)
    {
        var sessionId = SessionInfoProvider.SessionId;
        if (!sessionId.IsNullOrWhiteSpace())
        {
            await IdentitySessionManager.RevokeSessionAsync(sessionId);
        }
    }
}
