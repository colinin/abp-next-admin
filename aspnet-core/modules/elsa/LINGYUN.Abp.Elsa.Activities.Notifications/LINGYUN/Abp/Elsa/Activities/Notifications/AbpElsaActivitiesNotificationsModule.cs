using LINGYUN.Abp.Notifications;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.Elsa.Activities.Notifications;

[DependsOn(
    typeof(AbpElsaModule),
    typeof(AbpNotificationModule))]
public class AbpElsaActivitiesNotificationsModule : AbpModule
{
}
