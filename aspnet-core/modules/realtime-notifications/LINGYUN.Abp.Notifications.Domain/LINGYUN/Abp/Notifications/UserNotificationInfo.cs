using System;
using Volo.Abp.Data;

namespace LINGYUN.Abp.Notifications;

public class UserNotificationInfo
{
    public Guid? TenantId { get; set; }
    public string Name { get; set; } = default!;
    public long Id { get; set; }
    public long NotificationId { get; set; }
    public ExtraPropertyDictionary ExtraProperties { get; set; } = default!;
    public string NotificationTypeName { get; set; } = default!;
    public DateTime CreationTime { get; set; }
    public NotificationType Type { get; set; }
    public NotificationContentType ContentType { get; set; }
    public NotificationSeverity Severity { get; set; }
    public NotificationReadState State { get; set; }
}
