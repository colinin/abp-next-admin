using LINGYUN.Abp.Identity.Session;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using OpenIddict.Server;
using System.Security.Principal;
using System.Threading.Tasks;
using Volo.Abp.MultiTenancy;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace LINGYUN.Abp.OpenIddict.AspNetCore.Session;
public class ServerValidationTokenCheckIdentitySession : IOpenIddictServerHandler<OpenIddictServerEvents.ValidateTokenContext>
{
    public ILogger<ServerValidationTokenCheckIdentitySession> Logger { protected get; set; }
    protected ICurrentTenant CurrentTenant { get; }
    protected IIdentitySessionChecker IdentitySessionChecker { get; }
    protected AbpOpenIddictAspNetCoreSessionOptions AbpOpenIddictAspNetCoreSessionOptions { get; }

    public static OpenIddictServerHandlerDescriptor Descriptor { get; } =
        OpenIddictServerHandlerDescriptor.CreateBuilder<OpenIddictServerEvents.ValidateTokenContext>()
        .UseSingletonHandler<ServerValidationTokenCheckIdentitySession>()
        .SetOrder(OpenIddictServerHandlers.Protection.ValidatePrincipal.Descriptor.Order + 2_000)
        .SetType(OpenIddictServerHandlerType.Custom).Build();

    public ServerValidationTokenCheckIdentitySession(
        ICurrentTenant currentTenant, 
        IIdentitySessionChecker identitySessionChecker,
        IOptions<AbpOpenIddictAspNetCoreSessionOptions> abpOpenIddictAspNetCoreSessionOptions)
    {
        CurrentTenant = currentTenant;
        IdentitySessionChecker = identitySessionChecker;
        AbpOpenIddictAspNetCoreSessionOptions = abpOpenIddictAspNetCoreSessionOptions.Value;

        Logger = NullLogger<ServerValidationTokenCheckIdentitySession>.Instance;
    }

    public async virtual ValueTask HandleAsync(OpenIddictServerEvents.ValidateTokenContext context)
    {
        Logger.LogInformation("Server Validate Token: {endpointType} - {requestUri}", context.EndpointType, context.RequestUri);

        if (!AbpOpenIddictAspNetCoreSessionOptions.ValidationSessionEndpointTypes.Contains(context.EndpointType))
        {
            Logger.LogDebug("Endpoint '{endpointType}' is not in validation whitelist, skipping session validation.", context.EndpointType);
            return;
        }

        if (context.Principal == null || context.Principal.Identity?.IsAuthenticated == false)
        {
            Logger.LogWarning("Principal is null or not authenticated for endpoint '{endpointType}', skipping session validation.", context.EndpointType);
            return;
        }

        var tenantId = context.Principal?.FindTenantId();
        using (CurrentTenant.Change(tenantId))
        {
            if (!await IdentitySessionChecker.ValidateSessionAsync(context.Principal!))
            {
                Logger.LogWarning("The token is no longer valid because the user's session expired.");
                // Errors.InvalidToken --->  401
                // Errors.ExpiredToken --->  400
                context.Reject(Errors.InvalidToken, "The user session has expired.");
            }
        }
    }
}
