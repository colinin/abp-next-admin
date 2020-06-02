using LINGYUN.Abp.Notifications.Internal;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.Notifications
{
    public class AbpNotificationModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddTransient<INotificationDispatcher, DefaultNotificationDispatcher>();
        }
    }
}
