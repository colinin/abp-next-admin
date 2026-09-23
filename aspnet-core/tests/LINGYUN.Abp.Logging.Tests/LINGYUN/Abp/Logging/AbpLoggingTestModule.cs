using LINGYUN.Abp.Serilog.Enrichers.UniqueId;
using LINGYUN.Abp.Tests;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.Logging;

[DependsOn(
    typeof(AbpTestsBaseModule),
    typeof(AbpLoggingModule),
    typeof(AbpSerilogEnrichersUniqueIdModule))]
public class AbpLoggingTestModule : AbpModule
{

}
