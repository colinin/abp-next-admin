using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Application;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.AIManagement;

[DependsOn(
    typeof(AbpAIManagementApplicationContractsModule),
    typeof(AbpAIManagementDomainModule),
    typeof(AbpDddApplicationModule))]
public class AbpAIManagementApplicationModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddMapperlyObjectMapper<AbpAIManagementApplicationModule>();
    }
}
