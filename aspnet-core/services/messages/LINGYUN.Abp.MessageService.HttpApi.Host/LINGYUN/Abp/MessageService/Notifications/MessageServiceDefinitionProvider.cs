using LINGYUN.Abp.MessageService.Localization;
using LINGYUN.Abp.Notifications;
using Volo.Abp.Localization;

namespace LINGYUN.Abp.MessageService.LINGYUN.Abp.MessageService.Notifications
{
    public class MessageServiceDefinitionProvider : NotificationDefinitionProvider
    {
        public override void Define(INotificationDefinitionContext context)
        {
            context.Add(new NotificationDefinition(
                "TestApplicationNotofication", 
                L("TestApplicationNotofication"),
                L("TestApplicationNotofication"),
                NotificationType.Application, 
                true));
        }

        protected LocalizableString L(string name)
        {
            return LocalizableString.Create<MessageServiceResource>(name);
        }
    }
}
