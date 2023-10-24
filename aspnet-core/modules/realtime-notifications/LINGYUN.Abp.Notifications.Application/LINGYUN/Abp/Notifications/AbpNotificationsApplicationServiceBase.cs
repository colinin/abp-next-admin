using LINGYUN.Abp.Notifications.Localization;
using Volo.Abp.Application.Services;

namespace LINGYUN.Abp.Notifications
{
    public abstract class AbpMessageServiceApplicationServiceBase : ApplicationService
    {
        protected AbpMessageServiceApplicationServiceBase()
        {
            LocalizationResource = typeof(NotificationsResource);
            ObjectMapperContext = typeof(AbpNotificationsApplicationModule);
        }
    }
}
