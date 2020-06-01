using LINGYUN.Abp.MessageService.Notifications;
using LINGYUN.Abp.MessageService.Subscriptions;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.MessageService.EntityFrameworkCore
{
    [DependsOn(
        typeof(AbpMessageServiceDomainModule),
        typeof(AbpEntityFrameworkCoreModule))]
    public class AbpMessageServiceEntityFrameworkCoreModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAbpDbContext<MessageServiceDbContext>(options =>
            {
                options.AddRepository<Notification, INotificationRepository>();
                options.AddRepository<UserNotification, IUserNotificationRepository>();
                options.AddRepository<UserSubscribe, IUserSubscribeRepository>();

                options.AddDefaultRepositories(includeAllEntities: true);
            });
        }
    }
}
