using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Json;
using Volo.Abp.Mapperly;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.PrintDesigner;

[DependsOn(
    typeof(AbpJsonModule),
    typeof(AbpMapperlyModule),
    typeof(AbpPrintDesignerDomainSharedModule))]
public class AbpPrintDesignerDomainModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddMapperlyObjectMapper<AbpPrintDesignerDomainModule>();
    }
}
