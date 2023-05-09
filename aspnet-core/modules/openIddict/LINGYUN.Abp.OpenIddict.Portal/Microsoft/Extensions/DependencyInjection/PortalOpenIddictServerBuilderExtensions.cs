using LINGYUN.Abp.OpenIddict.Portal;

namespace Microsoft.Extensions.DependencyInjection;

public static class PortalOpenIddictServerBuilderExtensions
{
    public static OpenIddictServerBuilder AllowPortalFlow(this OpenIddictServerBuilder builder)
    {
        return builder.AllowCustomFlow(PortalTokenExtensionGrantConsts.GrantType);
    }
}
