using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Application;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.TextTemplating;

[DependsOn(
    typeof(AbpTextTemplatingDomainModule),
    typeof(AbpTextTemplatingApplicationContractsModule),
    typeof(AbpDddApplicationModule))]
public class AbpTextTemplatingApplicationModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddMapperlyObjectMapper<AbpTextTemplatingApplicationModule>();
    }
}
