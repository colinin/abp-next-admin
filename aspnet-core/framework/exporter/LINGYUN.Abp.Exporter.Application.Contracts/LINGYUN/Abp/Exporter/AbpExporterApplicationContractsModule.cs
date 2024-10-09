using Volo.Abp.Application;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.Exporter;

[DependsOn(typeof(AbpDddApplicationContractsModule))]
public class AbpExporterApplicationContractsModule : AbpModule
{
}
