using LINGYUN.Abp.Identity.Session;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using OpenIddict.Validation;
using System.Security.Principal;
using System.Threading.Tasks;
using Volo.Abp.MultiTenancy;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace LINGYUN.Abp.OpenIddict.AspNetCore.Session;
public class ValidationTokenCheckIdentitySession : IOpenIddictValidationHandler<OpenIddictValidationEvents.ValidateTokenContext>
{
    public ILogger<ValidationTokenCheckIdentitySession> Logger { protected get; set; }
    protected ICurrentTenant CurrentTenant { get; }
    protected IIdentitySessionChecker IdentitySessionChecker { get; }

    public static OpenIddictValidationHandlerDescriptor Descriptor { get; }
        = OpenIddictValidationHandlerDescriptor.CreateBuilder<OpenIddictValidationEvents.ValidateTokenContext>()
            .UseSingletonHandler<ValidationTokenCheckIdentitySession>()
            .SetOrder(OpenIddictValidationHandlers.Protection.ValidatePrincipal.Descriptor.Order + 2_000)
            .SetType(OpenIddictValidationHandlerType.Custom)
            .Build();

    public ValidationTokenCheckIdentitySession(
        ICurrentTenant currentTenant,
        IIdentitySessionChecker identitySessionChecker)
    {
        CurrentTenant = currentTenant;
        IdentitySessionChecker = identitySessionChecker;

        Logger = NullLogger<ValidationTokenCheckIdentitySession>.Instance;
    }

    public async virtual ValueTask HandleAsync(OpenIddictValidationEvents.ValidateTokenContext context)
    {
        Logger.LogInformation("Validate Token: {endpointType} - {requestUri}", context.EndpointType, context.RequestUri);

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
