namespace LINGYUN.Abp.Notifications.Definitions.Notifications;
public class NotificationDefinitionGetListInput
{
    public string Filter { get; set; }
    public string GroupName { get; set; }
    public string Template { get; set; }
    public bool? AllowSubscriptionToClients { get; set; }
    public NotificationLifetime? NotificationLifetime { get; set; }
    public NotificationType? NotificationType { get; set; }
    public NotificationContentType? ContentType { get; set; }
}
