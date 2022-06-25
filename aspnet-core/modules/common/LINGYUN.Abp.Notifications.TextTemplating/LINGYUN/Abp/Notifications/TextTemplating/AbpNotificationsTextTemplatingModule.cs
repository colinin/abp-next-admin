using Volo.Abp.Modularity;
using Volo.Abp.TextTemplating;

namespace LINGYUN.Abp.Notifications.TextTemplating;

[DependsOn(
    typeof(AbpNotificationModule),
    typeof(AbpTextTemplatingCoreModule))]
public class AbpNotificationsTextTemplatingModule : AbpModule
{

}
