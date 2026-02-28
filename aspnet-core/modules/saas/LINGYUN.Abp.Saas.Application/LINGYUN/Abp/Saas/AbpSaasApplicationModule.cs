using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.Saas;

[DependsOn(typeof(AbpSaasDomainModule))]
[DependsOn(typeof(AbpSaasApplicationContractsModule))]
public class AbpSaasApplicationModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddMapperlyObjectMapper<AbpSaasApplicationModule>();
    }
}
