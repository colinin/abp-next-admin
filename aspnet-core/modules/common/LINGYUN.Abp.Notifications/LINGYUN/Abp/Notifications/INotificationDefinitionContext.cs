namespace LINGYUN.Abp.Notifications
{
    public interface INotificationDefinitionContext
    {
        NotificationDefinition GetOrNull(string name);

        void Add(params NotificationDefinition[] definitions);
    }
}
