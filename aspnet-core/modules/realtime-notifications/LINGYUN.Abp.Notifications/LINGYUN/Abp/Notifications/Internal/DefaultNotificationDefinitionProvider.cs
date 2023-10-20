using LINGYUN.Abp.Notifications.Localization;
using Volo.Abp.Localization;

namespace LINGYUN.Abp.Notifications.Internal;

internal class DefaultNotificationDefinitionProvider : NotificationDefinitionProvider
{
    public override void Define(INotificationDefinitionContext context)
    {
        var internalGroup = context.AddGroup(
                DefaultNotifications.GroupName,
                L("Notifications:Internal"));

        internalGroup.AddNotification(
            DefaultNotifications.OnsideNotice,
            L("Notifications:OnsideNotice"),
            L("Notifications:OnsideNoticeDesc"),
            notificationType: NotificationType.Application,
            lifetime: NotificationLifetime.Persistent,
            allowSubscriptionToClients: true)
            .WithProviders(NotificationProviderNames.SignalR);
        internalGroup.AddNotification(
            DefaultNotifications.ActivityNotice,
            L("Notifications:ActivityNotice"),
            L("Notifications:ActivityNoticeDesc"),
            notificationType: NotificationType.Application,
            lifetime: NotificationLifetime.Persistent,
            allowSubscriptionToClients: true)
            .WithProviders(NotificationProviderNames.SignalR);
        internalGroup.AddNotification(
            DefaultNotifications.SystemNotice,
            L("Notifications:SystemNotice"),
            L("Notifications:SystemNoticeDesc"),
            notificationType: NotificationType.System,
            lifetime: NotificationLifetime.Persistent,
            allowSubscriptionToClients: true)
            .WithProviders(NotificationProviderNames.SignalR);
    }

    protected LocalizableString L(string name)
    {
        return LocalizableString.Create<NotificationsResource>(name);
    }
}
