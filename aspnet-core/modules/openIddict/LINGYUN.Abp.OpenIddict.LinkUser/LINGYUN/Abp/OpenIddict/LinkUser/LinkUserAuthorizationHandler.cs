using Microsoft.AspNetCore;
using OpenIddict.Server;
using System.Linq;
using System.Threading.Tasks;
using static OpenIddict.Server.OpenIddictServerEvents;

namespace LINGYUN.Abp.OpenIddict.LinkUser;

public class LinkUserAuthorizationHandler : IOpenIddictServerHandler<ValidateAuthorizationRequestContext>
{
    public static OpenIddictServerHandlerDescriptor Descriptor { get; }
        = OpenIddictServerHandlerDescriptor.CreateBuilder<ValidateAuthorizationRequestContext>()
            .UseScopedHandler<LinkUserAuthorizationHandler>()
            .SetOrder(2000)
            .SetType(OpenIddictServerHandlerType.Custom)
            .Build();

    public async ValueTask HandleAsync(ValidateAuthorizationRequestContext context)
    {
        var request = context.Transaction.GetHttpRequest();
        if (request == null)
        {
            return;
        }

        var linkUserId = request.Query["LinkUserId"].FirstOrDefault();
        var linkToken = request.Query["LinkToken"].FirstOrDefault();

        if (string.IsNullOrEmpty(linkUserId) || string.IsNullOrEmpty(linkToken))
        {
            return;
        }

        context.Transaction.SetProperty("LinkUserId", linkUserId);
        context.Transaction.SetProperty("LinkToken", linkToken);
    }
}
