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
            context.Add(new NotificationDefinition(
                UserNotificationNames.WelcomeToApplication,
                L("WelcomeToApplicationNotification"), 
                L("WelcomeToApplicationNotification"), 
                allowSubscriptionToClients: true));
            context.Add(new NotificationDefinition(
                TenantNotificationNames.NewTenantRegistered,
                L("NewTenantRegisterdNotification"),
                L("NewTenantRegisterdNotification"),
                allowSubscriptionToClients: true));
        }

        protected LocalizableString L(string name)
        {
            return LocalizableString.Create<MessageServiceResource>(name);
        }
    }
}
