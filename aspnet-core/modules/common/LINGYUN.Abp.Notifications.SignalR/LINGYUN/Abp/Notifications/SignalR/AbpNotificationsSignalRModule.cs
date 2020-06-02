using LINGYUN.Abp.RealTime;
using Volo.Abp.AspNetCore.SignalR;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.Notifications.SignalR
{
    [DependsOn(
        typeof(AbpRealTimeModule),
        typeof(AbpNotificationModule),
        typeof(AbpAspNetCoreSignalRModule))]
    public class AbpNotificationsSignalRModule : AbpModule
    {

    }
}
