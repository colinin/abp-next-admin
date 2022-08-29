using LINGYUN.Abp.Tests;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.Notifications
{
    [DependsOn(
        typeof(AbpNotificationsModule),
        typeof(AbpTestsBaseModule))]
    public class AbpNotificationsTestsModule : AbpModule
    {
    }
}
