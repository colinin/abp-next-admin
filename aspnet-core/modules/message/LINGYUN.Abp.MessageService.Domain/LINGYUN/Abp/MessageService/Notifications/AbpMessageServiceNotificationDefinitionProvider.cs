using LINGYUN.Abp.MessageService.Localization;
using LINGYUN.Abp.Notifications;
using Volo.Abp.Localization;
using Volo.Abp.Users.Notifications;

namespace LINGYUN.Abp.MessageService.Notifications
{
    public class AbpMessageServiceNotificationDefinitionProvider : NotificationDefinitionProvider
    {
        public override void Define(INotificationDefinitionContext context)
        {
            context.Add(new NotificationDefinition(
                UserNotificationNames.WelcomeToApplication,
                L("WelcomeToApplicationNotification"), 
                L("WelcomeToApplicationNotification"), 
                allowSubscriptionToClients: true));
        }

        protected LocalizableString L(string name)
        {
            return LocalizableString.Create<MessageServiceResource>(name);
        }
    }
}
