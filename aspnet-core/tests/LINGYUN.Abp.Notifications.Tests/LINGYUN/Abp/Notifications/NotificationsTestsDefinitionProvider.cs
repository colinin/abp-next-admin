namespace LINGYUN.Abp.Notifications
{
    public class NotificationsTestsDefinitionProvider : NotificationDefinitionProvider
    {
        public override void Define(INotificationDefinitionContext context)
        {
            var group = context.AddGroup(NotificationsTestsNames.GroupName);

            group.AddNotification(NotificationsTestsNames.Test1,
                notificationType: NotificationType.Application,
                contentType: NotificationContentType.Text,
                lifetime: NotificationLifetime.OnlyOne);

            group.AddNotification(NotificationsTestsNames.Test2,
                notificationType: NotificationType.Application,
                contentType: NotificationContentType.Html,
                lifetime: NotificationLifetime.Persistent);

            group.AddNotification(NotificationsTestsNames.Test3,
                notificationType: NotificationType.User,
                contentType: NotificationContentType.Markdown,
                lifetime: NotificationLifetime.OnlyOne);

            group.AddNotification(NotificationsTestsNames.Test4,
                notificationType: NotificationType.System,
                contentType: NotificationContentType.Json,
                lifetime: NotificationLifetime.OnlyOne);
        }
    }
}
