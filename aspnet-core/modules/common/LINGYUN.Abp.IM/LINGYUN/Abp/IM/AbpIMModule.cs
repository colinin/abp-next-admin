using Volo.Abp.EventBus;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.IM
{
    [DependsOn(typeof(AbpEventBusModule))]
    public class AbpIMModule : AbpModule
    {
    }
}
