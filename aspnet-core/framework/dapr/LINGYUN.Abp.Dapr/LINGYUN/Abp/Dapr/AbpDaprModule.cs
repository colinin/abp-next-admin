using Volo.Abp.Json;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.Dapr
{
    [DependsOn(typeof(AbpJsonModule))]
    public class AbpDaprModule : AbpModule
    {
    }
}
