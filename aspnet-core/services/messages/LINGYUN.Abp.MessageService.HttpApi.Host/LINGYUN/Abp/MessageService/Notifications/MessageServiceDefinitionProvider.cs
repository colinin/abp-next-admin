using LINGYUN.Abp.MessageService.Localization;
using LINGYUN.Abp.Notifications;
using Volo.Abp.Localization;

namespace LINGYUN.Abp.MessageService.Notifications
{
    public class MessageServiceDefinitionProvider : NotificationDefinitionProvider
    {
        public override void Define(INotificationDefinitionContext context)
        {
            context.Add(new NotificationDefinition(
                "TestApplicationNotofication", 
                L("TestApplicationNotofication"),
                L("TestApplicationNotofication"),
                notificationType: NotificationType.Application, 
                lifetime: NotificationLifetime.OnlyOne,
                allowSubscriptionToClients: true));
        }

        protected LocalizableString L(string name)
        {
            return LocalizableString.Create<MessageServiceResource>(name);
        }
    }
}
