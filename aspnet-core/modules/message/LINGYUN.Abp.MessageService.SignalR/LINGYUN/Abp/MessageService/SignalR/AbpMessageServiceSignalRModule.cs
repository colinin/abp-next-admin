using LINGYUN.Abp.IM.SignalR;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.MessageService.SignalR
{
    [DependsOn(typeof(AbpIMSignalRModule))]
    public class AbpMessageServiceSignalRModule : AbpModule
    {

    }
}
