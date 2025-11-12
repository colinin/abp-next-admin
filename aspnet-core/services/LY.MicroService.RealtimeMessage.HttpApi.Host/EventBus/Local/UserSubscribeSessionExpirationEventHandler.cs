using LINGYUN.Abp.Identity.Notifications;
using LINGYUN.Abp.Notifications;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities.Events;
using Volo.Abp.EventBus;
using Volo.Abp.Users;

namespace LY.MicroService.RealtimeMessage.EventBus;

public class UserSubscribeSessionExpirationEventHandler : ILocalEventHandler<EntityCreatedEventData<UserEto>>, ITransientDependency
{
    private readonly INotificationSubscriptionManager _notificationSubscriptionManager;

    public UserSubscribeSessionExpirationEventHandler(INotificationSubscriptionManager notificationSubscriptionManager)
    {
        _notificationSubscriptionManager = notificationSubscriptionManager; 
    }

    public async virtual Task HandleEventAsync(EntityCreatedEventData<UserEto> eventData)
    {
        // 新用户订阅会话过期通知
        await _notificationSubscriptionManager
            .SubscribeAsync(
                eventData.Entity.TenantId,
                new UserIdentifier(eventData.Entity.Id, eventData.Entity.UserName),
                IdentityNotificationNames.Session.ExpirationSession);
        // 新用户订阅不活跃用户清理通知
        await _notificationSubscriptionManager
            .SubscribeAsync(
                eventData.Entity.TenantId,
                new UserIdentifier(eventData.Entity.Id, eventData.Entity.UserName),
                IdentityNotificationNames.IdentityUser.CleaningUpInactiveUsers);
    }
}
