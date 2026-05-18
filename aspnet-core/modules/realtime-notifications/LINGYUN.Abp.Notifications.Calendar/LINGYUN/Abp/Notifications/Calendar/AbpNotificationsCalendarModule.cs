using LINGYUN.Abp.Calendar;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.Notifications.Calendar;

[DependsOn(
    typeof(AbpNotificationsModule),
    typeof(AbpCalendarModule))]
public class AbpNotificationsCalendarModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddTransient<INotificationPublishInterceptor, WorkdayCalendarPublishInterceptor>();
    }
}
