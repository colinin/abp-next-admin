using System;

namespace LINGYUN.Abp.Notifications;

public class UserNotificationDto
{
    public string Name { get; set; } = default!;
    public string Id { get; set; } = default!;
    public NotificationData Data { get; set; } = default!;
    public DateTime CreationTime { get; set; }
    public NotificationType Type { get; set; }
    public NotificationLifetime Lifetime { get; set; }
    public NotificationSeverity Severity { get; set; }
    public NotificationReadState State { get; set; }
    public NotificationContentType ContentType { get; set; }
}
