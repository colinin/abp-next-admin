namespace LINGYUN.Abp.Notifications
{
    public interface INotificationDefinitionContext
    {
        NotificationDefinition GetOrNull(string category);

        void Add(params NotificationDefinition[] definitions);
    }
}
