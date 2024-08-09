using LINGYUN.Abp.Notifications;
using LINGYUN.Abp.RealTime.Localization;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities.Events.Distributed;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.Identity;
using Volo.Abp.Identity.Localization;
using Volo.Abp.Localization;
using Volo.Abp.Timing;

namespace LINGYUN.Abp.Identity.Notifications;
public class IdentitySessionRevokeEventHandler :
    IDistributedEventHandler<EntityDeletedEto<IdentitySessionEto>>,
    ITransientDependency
{
    private readonly IClock _clock;
    private readonly INotificationSender _notificationSender;

    public IdentitySessionRevokeEventHandler(IClock clock, INotificationSender notificationSender)
    {
        _clock = clock;
        _notificationSender = notificationSender;
    }

    public async virtual Task HandleEventAsync(EntityDeletedEto<IdentitySessionEto> eventData)
    {
        var notificationData = new NotificationData();
        notificationData.WriteLocalizedData(
            new LocalizableStringInfo(
                LocalizationResourceNameAttribute.GetName(typeof(IdentityResource)),
                "Notifications:SessionExpiration"),
            new LocalizableStringInfo(
                LocalizationResourceNameAttribute.GetName(typeof(IdentityResource)),
                "Notifications:SessionExpirationMessage",
                new Dictionary<object, object>
                {
                    { "Device", eventData.Entity.Device }
                }),
            _clock.Now,
            eventData.Entity.UserId.ToString());
        notificationData.TrySetData("SessionId", eventData.Entity.SessionId);
        notificationData.TrySetData("ClientId", eventData.Entity.ClientId);
        notificationData.TrySetData("Device", eventData.Entity.Device);
        notificationData.TrySetData("IpAddresses", eventData.Entity.IpAddresses);
        notificationData.TrySetData("UserId", eventData.Entity.UserId.ToString());
        notificationData.TrySetData("SignedIn", eventData.Entity.SignedIn.ToString("yyyy-MM-dd HH:mm:ss"));
        if (eventData.Entity.LastAccessed.HasValue)
        {
            notificationData.TrySetData("LastAccessed", eventData.Entity.LastAccessed.Value.ToString("yyyy-MM-dd HH:mm:ss"));
        }

        await _notificationSender.SendNofiterAsync(
            IdentityNotificationNames.Session.ExpirationSession,
            notificationData,
            new UserIdentifier(eventData.Entity.UserId, eventData.Entity.SessionId),
            eventData.Entity.TenantId);
    }
}
