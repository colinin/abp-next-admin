using LINGYUN.Platform;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.MultiTenancy;
using Volo.Abp.Modularity;
using Volo.Abp.OpenIddict;
using Volo.Abp.OpenIddict.ExtensionGrantTypes;

namespace LINGYUN.Abp.OpenIddict.Portal;

[DependsOn(
    typeof(AbpOpenIddictAspNetCoreModule),
    typeof(AbpAspNetCoreMultiTenancyModule),
    typeof(PlatformDomainModule))]
public class AbpOpenIddictPortalModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<OpenIddictServerBuilder>(builder =>
        {
            builder.AllowPortalFlow();
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpOpenIddictExtensionGrantsOptions>(options =>
        {
            options.Grants.TryAdd(
                PortalTokenExtensionGrantConsts.GrantType,
                new PortalTokenExtensionGrant());
        });
    }
}
