using LINGYUN.Abp.Notifications.Localization;
using Volo.Abp.Application.Services;

namespace LINGYUN.Abp.Notifications;

public abstract class AbpNotificationsApplicationServiceBase : ApplicationService
{
    protected AbpNotificationsApplicationServiceBase()
    {
        LocalizationResource = typeof(NotificationsResource);
        ObjectMapperContext = typeof(AbpNotificationsApplicationModule);
    }
}
