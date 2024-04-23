using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AutoMapper;
using Volo.Abp.Caching;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.Notifications.EntityFrameworkCore;

[DependsOn(
    typeof(AbpCachingModule),
    typeof(AbpAutoMapperModule),
    typeof(AbpNotificationsDomainModule))]
public class AbpNotificationsEntityFrameworkCoreModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAbpDbContext<NotificationsDbContext>(options =>
        {
            options.AddDefaultRepositories<INotificationsDbContext>();

            options.AddRepository<Notification, EfCoreNotificationRepository>();

            options.AddRepository<UserNotification, EfCoreUserNotificationRepository>();
            options.AddRepository<UserSubscribe, EfCoreUserSubscribeRepository>();
        });

        context.Services.AddAbpDbContext<NotificationsDefinitionDbContext>(options =>
        {
            options.AddDefaultRepositories<INotificationsDefinitionDbContext>();

            options.AddRepository<NotificationDefinitionRecord, EfCoreNotificationDefinitionRecordRepository>();
            options.AddRepository<NotificationDefinitionGroupRecord, EfCoreNotificationDefinitionGroupRecordRepository>();
        });
    }
}
