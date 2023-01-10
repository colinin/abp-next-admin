using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AutoMapper;
using Volo.Abp.Caching;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.Notifications.EntityFrameworkCore;

[DependsOn(
    typeof(AbpCachingModule),
    typeof(AbpAutoMapperModule),
    typeof(AbpNotificationsModule),
    typeof(AbpNotificationsDomainSharedModule))]
public class AbpNotificationsEntityFrameworkCoreModule : AbpModule
{
    [DependsOn(
        typeof(AbpNotificationsDomainModule),
        typeof(AbpEntityFrameworkCoreModule))]
    public class AbpMessageServiceEntityFrameworkCoreModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAbpDbContext<NotificationsDbContext>(options =>
            {
                options.AddDefaultRepositories<INotificationsDbContext>();

                options.AddRepository<Notification, EfCoreNotificationRepository>();
                options.AddRepository<NotificationDefinitionRecord, EfCoreNotificationDefinitionRecordRepository>();
                options.AddRepository<NotificationDefinitionGroupRecord, EfCoreNotificationDefinitionGroupRecordRepository>();

                options.AddRepository<UserNotification, EfCoreUserNotificationRepository>();
                options.AddRepository<UserSubscribe, EfCoreUserSubscribeRepository>();
            });
        }
    }
}
