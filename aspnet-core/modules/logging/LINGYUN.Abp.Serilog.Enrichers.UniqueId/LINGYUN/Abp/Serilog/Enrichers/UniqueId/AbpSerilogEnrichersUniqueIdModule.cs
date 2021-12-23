using LINGYUN.Abp.IdGenerator;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.Serilog.Enrichers.UniqueId
{
    [DependsOn(typeof(AbpIdGeneratorModule))]
    public class AbpSerilogEnrichersUniqueIdModule : AbpModule
    {
    }
}
