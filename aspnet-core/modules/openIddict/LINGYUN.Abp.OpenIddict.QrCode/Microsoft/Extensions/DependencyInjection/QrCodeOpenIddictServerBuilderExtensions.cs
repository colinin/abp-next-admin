using LINGYUN.Abp.Identity.QrCode;

namespace Microsoft.Extensions.DependencyInjection;

public static class QrCodeOpenIddictServerBuilderExtensions
{
    public static OpenIddictServerBuilder AllowQrCodeFlow(this OpenIddictServerBuilder builder)
    {
        return builder.AllowCustomFlow(QrCodeLoginProviderConsts.GrantType);
    }
}
