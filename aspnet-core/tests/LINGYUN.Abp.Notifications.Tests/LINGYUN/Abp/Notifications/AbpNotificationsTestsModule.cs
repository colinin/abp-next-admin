using LINGYUN.Abp.Tests;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.Notifications
{
    [DependsOn(
        typeof(AbpNotificationModule),
        typeof(AbpTestsBaseModule))]
    public class AbpNotificationsTestsModule : AbpModule
    {
    }
}
