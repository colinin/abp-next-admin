using LINGYUN.Abp.Elsa.Notifications;
using LINGYUN.Abp.Tests;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.Elsa.Tests
{
    [DependsOn(
        typeof(AbpElsaNotificationsModule),
        typeof(AbpTestsBaseModule)
        )]
    public class AbpElsaTestsModule : AbpModule
    {
    }
}
