using LINGYUN.Abp.RealTime;
using Volo.Abp.AspNetCore.SignalR;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.IM.SignalR
{
    [DependsOn(
        typeof(AbpRealTimeModule),
        typeof(AbpAspNetCoreSignalRModule))]
    public class AbpIMSignalRModule : AbpModule
    {

    }
}
