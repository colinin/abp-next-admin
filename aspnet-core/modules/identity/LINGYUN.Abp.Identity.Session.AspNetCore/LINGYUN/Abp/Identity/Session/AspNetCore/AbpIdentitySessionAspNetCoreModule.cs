using LINGYUN.Abp.IP2Region;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Volo.Abp.AspNetCore;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.Identity.Session.AspNetCore;

[DependsOn(typeof(AbpAspNetCoreModule))]
[DependsOn(typeof(AbpIP2RegionModule))]
[DependsOn(typeof(AbpIdentitySessionModule))]
public class AbpIdentitySessionAspNetCoreModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.Replace(ServiceDescriptor.Singleton<IIpLocationInfoProvider, IP2RegionLocationInfoProvider>());
    }
}
