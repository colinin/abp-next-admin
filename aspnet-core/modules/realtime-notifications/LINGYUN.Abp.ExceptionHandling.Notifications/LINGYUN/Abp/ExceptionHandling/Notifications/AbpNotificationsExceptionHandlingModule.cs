using LINGYUN.Abp.Notifications.Common;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.ExceptionHandling.Notifications;

[DependsOn(
    typeof(AbpExceptionHandlingModule),
    typeof(AbpNotificationsCommonModule))]
public class AbpNotificationsExceptionHandlingModule : AbpModule
{
}
