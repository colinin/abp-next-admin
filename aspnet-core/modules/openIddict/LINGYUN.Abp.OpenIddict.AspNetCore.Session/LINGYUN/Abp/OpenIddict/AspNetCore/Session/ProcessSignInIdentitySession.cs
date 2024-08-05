using LINGYUN.Abp.Identity.Session;
using OpenIddict.Abstractions;
using OpenIddict.Server;
using System.Threading.Tasks;

namespace LINGYUN.Abp.OpenIddict.AspNetCore.Session;
/// <summary>
/// 登录成功持久化用户会话
/// </summary>
public class ProcessSignInIdentitySession : IOpenIddictServerHandler<OpenIddictServerEvents.ProcessSignInContext>
{
    protected IIdentitySessionManager IdentitySessionManager { get; }

    public static OpenIddictServerHandlerDescriptor Descriptor { get; }
        = OpenIddictServerHandlerDescriptor.CreateBuilder<OpenIddictServerEvents.ProcessSignInContext>()
            .AddFilter<OpenIddictServerHandlerFilters.RequireAccessTokenGenerated>()
            .UseScopedHandler<ProcessSignInIdentitySession>()
            .SetOrder(OpenIddictServerHandlers.PrepareAccessTokenPrincipal.Descriptor.Order + 1_000)
            .SetType(OpenIddictServerHandlerType.Custom)
            .Build();

    public ProcessSignInIdentitySession(IIdentitySessionManager identitySessionManager)
    {
        IdentitySessionManager = identitySessionManager;
    }

    public async virtual ValueTask HandleAsync(OpenIddictServerEvents.ProcessSignInContext context)
    {
        if (context.Request.IsPasswordGrantType() && context.Principal != null)
        {
            await IdentitySessionManager.SaveSessionAsync(context.Principal, context.CancellationToken);
        }
    }
}
