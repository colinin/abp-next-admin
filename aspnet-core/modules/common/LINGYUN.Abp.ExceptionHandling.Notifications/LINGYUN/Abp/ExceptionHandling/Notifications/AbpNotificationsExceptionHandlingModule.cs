using LINGYUN.Abp.Notifications;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.ExceptionHandling.Notifications
{
    [DependsOn(
        typeof(AbpExceptionHandlingModule),
        typeof(AbpNotificationModule))]
    public class AbpNotificationsExceptionHandlingModule : AbpModule
    {
    }
}
