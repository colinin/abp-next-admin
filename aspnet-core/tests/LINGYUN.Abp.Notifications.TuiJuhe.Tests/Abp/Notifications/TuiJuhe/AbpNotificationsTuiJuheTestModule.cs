using Volo.Abp.Modularity;

namespace LINGYUN.Abp.Notifications.TuiJuhe;

[DependsOn(
    typeof(AbpNotificationsTuiJuheModule),
    typeof(AbpTuiJuheTestModule),
    typeof(AbpNotificationsTestsModule))]
public class AbpNotificationsTuiJuheTestModule : AbpModule
{
}
