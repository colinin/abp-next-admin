using LINGYUN.Abp.Identity.QrCode;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;
using Volo.Abp.OpenIddict;
using Volo.Abp.OpenIddict.ExtensionGrantTypes;

namespace LINGYUN.Abp.OpenIddict.QrCode;

[DependsOn(
    typeof(AbpIdentityQrCodeModule),
    typeof(AbpOpenIddictDomainModule))]
public class AbpOpenIddictQrCodeModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<OpenIddictServerBuilder>(builder =>
        {
            builder.AllowQrCodeFlow();
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpOpenIddictExtensionGrantsOptions>(options =>
        {
            options.Grants.TryAdd(
                QrCodeLoginProviderConsts.GrantType,
                new QrCodeTokenExtensionGrant());
        });
    }
}
