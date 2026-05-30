using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities.Events;
using Volo.Abp.Users;

namespace LY.MicroService.Applications.Single.EventBus.Local;

public class UserCreateSubscribeIdentityNotificationsEventHandler : ILocalEventHandler<EntityCreatedEventData<UserEto>>, ITransientDependency
{
    private readonly INotificationSubscriptionManager _notificationSubscriptionManager;
    public UserCreateSubscribeIdentityNotificationsEventHandler(
        INotificationSubscriptionManager notificationSubscriptionManager
        )
    {
        _notificationSubscriptionManager = notificationSubscriptionManager;
    }

    public async virtual Task HandleEventAsync(EntityCreatedEventData<UserEto> eventData)
    {
        var userIdentifer = new UserIdentifier(eventData.Entity.Id, eventData.Entity.UserName);
        // 订阅用户会话过期通知
        await _notificationSubscriptionManager
            .SubscribeAsync(
                eventData.Entity.TenantId,
                userIdentifer,
                IdentityNotificationNames.Session.ExpirationSession);
        // 订阅用户账号不活跃通知
        await _notificationSubscriptionManager
            .SubscribeAsync(
                eventData.Entity.TenantId,
                userIdentifer,
                IdentityNotificationNames.IdentityUser.InactiveUserReminderNotifier);
        // 订阅用户账号停用通知
        await _notificationSubscriptionManager
            .SubscribeAsync(
                eventData.Entity.TenantId,
                userIdentifer,
                IdentityNotificationNames.IdentityUser.InactiveUserDeactivationNotifier);
    }
}
