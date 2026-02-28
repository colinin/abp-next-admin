using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Application;
using Volo.Abp.Mapperly;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.EntityChange;

[DependsOn(
    typeof(AbpEntityChangeApplicationContractsModule),
    typeof(AbpDddApplicationModule),
    typeof(AbpMapperlyModule))]
public class AbpEntityChangeApplicationModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddMapperlyObjectMapper<AbpEntityChangeApplicationModule>();
    }
}
