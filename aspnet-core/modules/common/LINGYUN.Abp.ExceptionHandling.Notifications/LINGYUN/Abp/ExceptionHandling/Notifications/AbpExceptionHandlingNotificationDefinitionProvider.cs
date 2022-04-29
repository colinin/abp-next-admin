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
                allowSubscriptionToClients: false)
                // 指定通知提供程序
                .WithProviders(
                    NotificationProviderNames.SignalR,
                    NotificationProviderNames.Emailing)
                // 特定的通知提供程序属性
                // 此处为邮件通知定义的模板名称
                .WithProperty("Template", "ExceptionNotifier");
        }

        protected LocalizableString L(string name)
        {
            return LocalizableString.Create<ExceptionHandlingResource>(name);
        }
    }
}
