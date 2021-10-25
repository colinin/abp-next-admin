using LINGYUN.Abp.RealTime;
using Volo.Abp.EventBus;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.IM
{
    [DependsOn(
        typeof(AbpEventBusModule),
        typeof(AbpRealTimeModule))]
    public class AbpIMModule : AbpModule
    {
    }
}
