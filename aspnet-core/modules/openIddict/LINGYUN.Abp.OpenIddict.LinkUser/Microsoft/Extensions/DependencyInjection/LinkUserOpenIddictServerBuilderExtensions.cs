using LINGYUN.Abp.OpenIddict.LinkUser;

namespace Microsoft.Extensions.DependencyInjection;

public static class LinkUserOpenIddictServerBuilderExtensions
{
    public static OpenIddictServerBuilder AllowLinkUserFlow(this OpenIddictServerBuilder builder)
    {
        return builder.AllowCustomFlow(LinkUserTokenExtensionGrantConsts.GrantType);
    }
}
