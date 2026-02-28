using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Application;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.IdentityServer;

[DependsOn(
    typeof(AbpIdentityServerApplicationContractsModule),
    typeof(AbpIdentityServerDomainModule),
    typeof(AbpDddApplicationModule)
    )]
public class AbpIdentityServerApplicationModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddMapperlyObjectMapper<AbpIdentityServerApplicationModule>();
    }
}
