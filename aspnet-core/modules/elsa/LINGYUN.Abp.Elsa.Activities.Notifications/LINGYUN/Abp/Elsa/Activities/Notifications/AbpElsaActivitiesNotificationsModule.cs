using LINGYUN.Abp.Notifications;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.Elsa.Activities.Notifications;

[DependsOn(
    typeof(AbpElsaModule),
    typeof(AbpNotificationsModule))]
public class AbpElsaActivitiesNotificationsModule : AbpModule
{
}
