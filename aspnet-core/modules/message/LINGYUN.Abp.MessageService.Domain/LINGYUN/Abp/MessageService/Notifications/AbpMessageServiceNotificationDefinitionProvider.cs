using LINGYUN.Abp.MessageService.Localization;
using LINGYUN.Abp.Notifications;
using Volo.Abp.Localization;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Users;

namespace LINGYUN.Abp.MessageService.Notifications
{
    public class AbpMessageServiceNotificationDefinitionProvider : NotificationDefinitionProvider
    {
        public override void Define(INotificationDefinitionContext context)
        {
            var tenantsGroup = context.AddGroup(
                TenantNotificationNames.GroupName,
                L("Notifications:MultiTenancy"),
                false);

            tenantsGroup.AddNotification(
                TenantNotificationNames.NewTenantRegistered,
                L("Notifications:NewTenantRegisterd"),
                L("Notifications:NewTenantRegisterd"),
                notificationType: NotificationType.System,
                lifetime: NotificationLifetime.OnlyOne,
                allowSubscriptionToClients: false
                )
                .WithProviders();

            var usersGroup = context.AddGroup(
                UserNotificationNames.GroupName,
                L("Notifications:Users"));

            usersGroup.AddNotification(
                UserNotificationNames.WelcomeToApplication,
                L("Notifications:WelcomeToApplication"),
                L("Notifications:WelcomeToApplication"),
                notificationType: NotificationType.System,
                lifetime: NotificationLifetime.OnlyOne,
                allowSubscriptionToClients: true);

            var imGroup = context.AddGroup(
                MessageServiceNotificationNames.IM.GroupName,
                L("Notifications:IM"));
            imGroup.AddNotification(
                MessageServiceNotificationNames.IM.FriendValidation,
                L("Notifications:FriendValidation"),
                L("Notifications:FriendValidation"),
                notificationType: NotificationType.System,
                lifetime: NotificationLifetime.Persistent,
                allowSubscriptionToClients: true);
            imGroup.AddNotification(
                MessageServiceNotificationNames.IM.NewFriend,
                L("Notifications:NewFriend"),
                L("Notifications:NewFriend"),
                notificationType: NotificationType.System,
                lifetime: NotificationLifetime.Persistent,
                allowSubscriptionToClients: true);
            imGroup.AddNotification(
                MessageServiceNotificationNames.IM.JoinGroup,
                L("Notifications:JoinGroup"),
                L("Notifications:JoinGroup"),
                notificationType: NotificationType.System,
                lifetime: NotificationLifetime.Persistent,
                allowSubscriptionToClients: true);
            imGroup.AddNotification(
                MessageServiceNotificationNames.IM.ExitGroup,
                L("Notifications:ExitGroup"),
                L("Notifications:ExitGroup"),
                notificationType: NotificationType.System,
                lifetime: NotificationLifetime.Persistent,
                allowSubscriptionToClients: true);
            imGroup.AddNotification(
               MessageServiceNotificationNames.IM.DissolveGroup,
               L("Notifications:DissolveGroup"),
               L("Notifications:DissolveGroup"),
               notificationType: NotificationType.System,
               lifetime: NotificationLifetime.Persistent,
               allowSubscriptionToClients: true);
        }

        protected LocalizableString L(string name)
        {
            return LocalizableString.Create<MessageServiceResource>(name);
        }
    }
}
