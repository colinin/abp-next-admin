using LINGYUN.Abp.MessageService.Localization;
using LINGYUN.Abp.Notifications;
using Volo.Abp.Localization;

namespace LINGYUN.Abp.MessageService.Notifications;

public class AbpMessageServiceNotificationDefinitionProvider : NotificationDefinitionProvider
{
    public override void Define(INotificationDefinitionContext context)
    {
        var imGroup = context.AddGroup(
            MessageServiceNotificationNames.IM.GroupName,
            L("Notifications:IM"));
        imGroup.AddNotification(
            MessageServiceNotificationNames.IM.FriendValidation,
            L("Notifications:FriendValidation"),
            L("Notifications:FriendValidation"),
            notificationType: NotificationType.Application,
            lifetime: NotificationLifetime.Persistent,
            allowSubscriptionToClients: true)
            .WithProviders(
                NotificationProviderNames.SignalR);
        imGroup.AddNotification(
            MessageServiceNotificationNames.IM.NewFriend,
            L("Notifications:NewFriend"),
            L("Notifications:NewFriend"),
            notificationType: NotificationType.Application,
            lifetime: NotificationLifetime.Persistent,
            allowSubscriptionToClients: true)
            .WithProviders(
                NotificationProviderNames.SignalR);
        imGroup.AddNotification(
            MessageServiceNotificationNames.IM.JoinGroup,
            L("Notifications:JoinGroup"),
            L("Notifications:JoinGroup"),
            notificationType: NotificationType.Application,
            lifetime: NotificationLifetime.Persistent,
            allowSubscriptionToClients: true)
            .WithProviders(
                NotificationProviderNames.SignalR);
        imGroup.AddNotification(
            MessageServiceNotificationNames.IM.ExitGroup,
            L("Notifications:ExitGroup"),
            L("Notifications:ExitGroup"),
            notificationType: NotificationType.Application,
            lifetime: NotificationLifetime.Persistent,
            allowSubscriptionToClients: true)
            .WithProviders(
                NotificationProviderNames.SignalR);
        imGroup.AddNotification(
           MessageServiceNotificationNames.IM.DissolveGroup,
           L("Notifications:DissolveGroup"),
           L("Notifications:DissolveGroup"),
           notificationType: NotificationType.Application,
           lifetime: NotificationLifetime.Persistent,
           allowSubscriptionToClients: true)
            .WithProviders(
                NotificationProviderNames.SignalR);
    }

    protected LocalizableString L(string name)
    {
        return LocalizableString.Create<MessageServiceResource>(name);
    }
}
