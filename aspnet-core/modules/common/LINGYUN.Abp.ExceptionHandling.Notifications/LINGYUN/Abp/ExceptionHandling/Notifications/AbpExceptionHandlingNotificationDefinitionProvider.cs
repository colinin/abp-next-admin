using LINGYUN.Abp.ExceptionHandling.Localization;
using LINGYUN.Abp.Notifications;
using Volo.Abp.Localization;

namespace LINGYUN.Abp.ExceptionHandling.Notifications
{
    public class AbpExceptionHandlingNotificationDefinitionProvider : NotificationDefinitionProvider
    {
        public override void Define(INotificationDefinitionContext context)
        {
            var exceptionGroup = context.AddGroup(
                AbpExceptionHandlingNotificationNames.GroupName,
                L("Notifications:Exception"),
                false);

            exceptionGroup.AddNotification(
                name: AbpExceptionHandlingNotificationNames.NotificationName,
                displayName: L("Notifications:ExceptionNotifier"),
                description: L("Notifications:ExceptionNotifier"),
                notificationType: NotificationType.System,
                lifetime: NotificationLifetime.Persistent,
                allowSubscriptionToClients: false);
        }

        protected LocalizableString L(string name)
        {
            return LocalizableString.Create<ExceptionHandlingResource>(name);
        }
    }
}
