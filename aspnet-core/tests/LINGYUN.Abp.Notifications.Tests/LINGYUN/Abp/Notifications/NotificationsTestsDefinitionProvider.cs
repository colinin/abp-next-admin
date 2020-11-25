namespace LINGYUN.Abp.Notifications
{
    public class NotificationsTestsDefinitionProvider : NotificationDefinitionProvider
    {
        public override void Define(INotificationDefinitionContext context)
        {
            var group = context.AddGroup(NotificationsTestsNames.GroupName);

            group.AddNotification(NotificationsTestsNames.Test1,
                notificationType: NotificationType.Application,
                lifetime: NotificationLifetime.OnlyOne);

            group.AddNotification(NotificationsTestsNames.Test2,
                notificationType: NotificationType.Application,
                lifetime: NotificationLifetime.Persistent);

            group.AddNotification(NotificationsTestsNames.Test3,
                notificationType: NotificationType.User,
                lifetime: NotificationLifetime.OnlyOne);
        }
    }
}
