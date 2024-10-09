using Volo.Abp.Application;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.Exporter;

[DependsOn(
    typeof(AbpDddApplicationModule),
    typeof(AbpExporterCoreModule),
    typeof(AbpExporterApplicationContractsModule))]
public class AbpExporterApplicationModule : AbpModule
{
}
