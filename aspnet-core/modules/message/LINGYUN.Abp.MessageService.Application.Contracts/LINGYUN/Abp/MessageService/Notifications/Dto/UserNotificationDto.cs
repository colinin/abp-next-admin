using LINGYUN.Abp.Notifications;
using System;

namespace LINGYUN.Abp.MessageService.Notifications;

public class UserNotificationDto
{
    public string Name { get; set; }
    public string Id { get; set; }
    public NotificationData Data { get; set; }
    public DateTime CreationTime { get; set; }
    public NotificationLifetime Lifetime { get; set; }
    public NotificationType Type { get; set; }
    public NotificationSeverity Severity { get; set; }
    public NotificationReadState State { get; set; }
}
