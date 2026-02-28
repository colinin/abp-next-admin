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
            allowSubscriptionToClients: false) // 客户端禁用, 所有新用户必须订阅此通知
            .WithProviders(
                NotificationProviderNames.SignalR, // 实时通知处理会话过期事件
                NotificationProviderNames.Emailing); // 邮件通知会话过期

        group.AddNotification(
            IdentityNotificationNames.IdentityUser.CleaningUpInactiveUsers,
            L("Notifications:CleaningUpInactiveUsers"),
            L("Notifications:CleaningUpInactiveUsers"),
            NotificationType.Application,
            NotificationLifetime.Persistent,
            NotificationContentType.Markdown,
            allowSubscriptionToClients: false) // 客户端禁用, 所有新用户必须订阅此通知
            .WithProviders(NotificationProviderNames.Emailing);
    }

    private static ILocalizableString L(string name)
    {
        return LocalizableString.Create<IdentityResource>(name);
    }
}
