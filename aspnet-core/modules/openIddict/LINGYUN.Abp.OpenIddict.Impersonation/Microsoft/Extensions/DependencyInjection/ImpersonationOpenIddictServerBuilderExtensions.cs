using LINGYUN.Abp.OpenIddict.Impersonation;

namespace Microsoft.Extensions.DependencyInjection;

public static class ImpersonationOpenIddictServerBuilderExtensions
{
    public static OpenIddictServerBuilder AllowImpersonationFlow(this OpenIddictServerBuilder builder)
    {
        return builder.AllowCustomFlow(ImpersonationTokenExtensionGrantConsts.GrantType);
    }
}
