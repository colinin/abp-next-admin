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

        // 账号保持活跃通知
        group.AddNotification(
            IdentityNotificationNames.IdentityUser.InactiveUserReminderNotifier,
            L("InactiveUserReminderNotifier"),
            L("InactiveUserReminderNotifier"),
            NotificationType.User,
            NotificationLifetime.Persistent,
            NotificationContentType.Html,
            allowSubscriptionToClients: false) // 客户端禁用, 所有新用户必须订阅此通知
            .WithProviders(NotificationProviderNames.Emailing);
        // 账号停用通知
        group.AddNotification(
            IdentityNotificationNames.IdentityUser.InactiveUserDeactivationNotifier,
            L("InactiveUserDeactivationNotifier"),
            L("InactiveUserDeactivationNotifier"),
            NotificationType.User,
            NotificationLifetime.Persistent,
            NotificationContentType.Html,
            allowSubscriptionToClients: false) // 客户端禁用, 所有新用户必须订阅此通知
            .WithProviders(NotificationProviderNames.Emailing);
        // 账号删除通知不需要添加,直接发送邮件通知即可,因为发布通知时用户已删除
    }

    private static ILocalizableString L(string name)
    {
        return LocalizableString.Create<IdentityResource>(name);
    }
}
