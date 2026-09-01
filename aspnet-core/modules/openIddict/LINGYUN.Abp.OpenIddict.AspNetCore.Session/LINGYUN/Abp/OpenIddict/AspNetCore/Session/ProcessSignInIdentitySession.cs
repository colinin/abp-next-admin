using LINGYUN.Abp.Identity.Session;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using OpenIddict.Server;
using System;
using System.Security.Principal;
using System.Threading.Tasks;

namespace LINGYUN.Abp.OpenIddict.AspNetCore.Session;
/// <summary>
/// 登录成功持久化用户会话
/// </summary>
public class ProcessSignInIdentitySession : IOpenIddictServerHandler<OpenIddictServerEvents.ProcessSignInContext>
{
    public ILogger<ProcessSignInIdentitySession> Logger { protected get; set; }

    protected IIdentitySessionManager IdentitySessionManager { get; }
    protected AbpOpenIddictAspNetCoreSessionOptions AbpOpenIddictAspNetCoreSessionOptions { get; }

    public static OpenIddictServerHandlerDescriptor Descriptor { get; }
        = OpenIddictServerHandlerDescriptor.CreateBuilder<OpenIddictServerEvents.ProcessSignInContext>()
            .AddFilter<OpenIddictServerHandlerFilters.RequireAccessTokenGenerated>()
            .UseScopedHandler<ProcessSignInIdentitySession>()
            .SetOrder(OpenIddictServerHandlers.PrepareAccessTokenPrincipal.Descriptor.Order + 1_000)
            .SetType(OpenIddictServerHandlerType.Custom)
            .Build();

    public ProcessSignInIdentitySession(
        IIdentitySessionManager identitySessionManager,
        IOptions<AbpOpenIddictAspNetCoreSessionOptions> abpOpenIddictAspNetCoreSessionOptions)
    {
        IdentitySessionManager = identitySessionManager;
        AbpOpenIddictAspNetCoreSessionOptions = abpOpenIddictAspNetCoreSessionOptions.Value;

        Logger = NullLogger<ProcessSignInIdentitySession>.Instance;
    }

    public async virtual ValueTask HandleAsync(OpenIddictServerEvents.ProcessSignInContext context)
    {
        if (!context.Request.GrantType.IsNullOrWhiteSpace() &&
            AbpOpenIddictAspNetCoreSessionOptions.PersistentSessionGrantTypes.Contains(context.Request.GrantType) &&
            context.Principal != null)
        {
            Logger.LogInformation("Saving session for grant type: {grantType}", context.Request.GrantType);

            await IdentitySessionManager.SaveSessionAsync(context.Principal, context.CancellationToken);
            
            Logger.LogInformation("Session saved successfully: {sessionId}", context.Principal.FindSessionId());
        }
        else
        {
            Logger.LogDebug("Skipping session save for grant type: {grantType}", context.Request.GrantType);
        }
    }
}
