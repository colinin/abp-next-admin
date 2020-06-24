using LINGYUN.Abp.Notifications;

namespace LINGYUN.Abp.ExceptionHandling.Notifications
{
    public class AbpExceptionHandlingNotificationDefinitionProvider : NotificationDefinitionProvider
    {
        public override void Define(INotificationDefinitionContext context)
        {
            context.Add(new NotificationDefinition(AbpExceptionHandlingNotificationNames.NotificationName));
        }
    }
}
