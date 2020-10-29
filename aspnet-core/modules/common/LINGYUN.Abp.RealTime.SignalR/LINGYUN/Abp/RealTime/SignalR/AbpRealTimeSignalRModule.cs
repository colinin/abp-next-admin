using Volo.Abp.AspNetCore.SignalR;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.RealTime.SignalR
{
    [DependsOn(
        typeof(AbpAspNetCoreSignalRModule))]
    public class AbpRealTimeSignalRModule : AbpModule
    {
    }
}
