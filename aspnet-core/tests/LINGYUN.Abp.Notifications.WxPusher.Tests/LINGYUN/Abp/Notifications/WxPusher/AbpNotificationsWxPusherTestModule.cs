using LINGYUN.Abp.WxPusher;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.Notifications.WxPusher;

[DependsOn(
    typeof(AbpNotificationsWxPusherModule),
    typeof(AbpWxPusherTestModule),
    typeof(AbpNotificationsTestsModule))]
public class AbpNotificationsWxPusherTestModule : AbpModule
{
}
