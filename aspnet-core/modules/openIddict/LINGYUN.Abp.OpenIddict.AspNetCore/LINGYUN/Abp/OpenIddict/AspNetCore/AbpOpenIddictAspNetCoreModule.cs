using LINGYUN.Abp.Identity;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;
using VoloAbpOpenIddictAspNetCoreModule = Volo.Abp.OpenIddict.AbpOpenIddictAspNetCoreModule;

namespace LINGYUN.Abp.OpenIddict.AspNetCore;

[DependsOn(
    typeof(AbpIdentityDomainSharedModule),
    typeof(VoloAbpOpenIddictAspNetCoreModule))]
public class AbpOpenIddictAspNetCoreModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<OpenIddictServerBuilder>(builder =>
        {
            builder.RegisterClaims(new[] { IdentityConsts.ClaimType.Avatar.Name } );
        });
    }
}
