using Volo.Abp.Application;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.Exporter;

[DependsOn(
    typeof(AbpDddApplicationModule),
    typeof(AbpExporterApplicationContractsModule))]
public class AbpExporterHttpApiModule : AbpModule
{

}
