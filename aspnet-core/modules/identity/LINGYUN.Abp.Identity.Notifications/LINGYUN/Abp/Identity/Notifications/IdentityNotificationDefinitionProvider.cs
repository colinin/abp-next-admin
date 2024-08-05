using LINGYUN.Abp.Notifications;
using Volo.Abp.Identity.Localization;
using Volo.Abp.Localization;

namespace LINGYUN.Abp.Identity.Notifications;
public class IdentityNotificationDefinitionProvider : NotificationDefinitionProvider
{
    public override void Define(INotificationDefinitionContext context)
    {
        var group = context.AddGroup(
            IdentityNotificationNames.GroupName,
            L("Notifications:AbpIdentity"),
            L("Notifications:AbpIdentity"));

        group.AddNotification(
            IdentityNotificationNames.Session.ExpirationSession,
            L("Notifications:SessionExpiration"),
            L("Notifications:SessionExpiration"),
            NotificationType.ServiceCallback,
            NotificationLifetime.Persistent,
            NotificationContentType.Json,
            allowSubscriptionToClients: true);
    }

    private static ILocalizableString L(string name)
    {
        return LocalizableString.Create<IdentityResource>(name);
    }
}
