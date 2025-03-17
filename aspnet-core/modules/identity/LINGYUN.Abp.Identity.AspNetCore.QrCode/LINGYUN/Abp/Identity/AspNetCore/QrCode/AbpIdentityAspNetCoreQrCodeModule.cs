using LINGYUN.Abp.Identity.QrCode;
using Microsoft.AspNetCore.Identity;
using Volo.Abp.Identity.AspNetCore;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.Identity.AspNetCore.QrCode;

[DependsOn(
    typeof(AbpIdentityQrCodeModule),
    typeof(AbpIdentityAspNetCoreModule))]
public class AbpIdentityAspNetCoreQrCodeModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<IdentityBuilder>(builder =>
        {
            builder.AddTokenProvider<QrCodeUserTokenProvider>(QrCodeUserTokenProvider.ProviderName);
        });
    }
}
