using Volo.Abp.EventBus;
using Volo.Abp.Modularity;

namespace LINGYUN.Common.EventBus
{
    [DependsOn(typeof(AbpEventBusModule))]
    public class CommonEventBusModule : AbpModule
    {

    }
}
