using LINGYUN.Abp.MessageService.Chat;
using LINGYUN.Abp.MessageService.Group;
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

                options.AddRepository<ChatGroup, IGroupRepository>();
                options.AddRepository<UserChatGroup, IUserChatGroupRepository>();
                options.AddRepository<UserChatCard, IUserChatCardRepository>();
                options.AddRepository<UserChatSetting, IUserChatSettingRepository>();

                options.AddRepository<UserChatFriend, IUserChatFriendRepository>();

                options.AddDefaultRepositories(includeAllEntities: true);
            });
        }
    }
}
