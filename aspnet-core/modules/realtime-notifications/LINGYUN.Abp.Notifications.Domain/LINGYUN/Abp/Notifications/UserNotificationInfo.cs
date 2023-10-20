using System;
using Volo.Abp.Data;

namespace LINGYUN.Abp.Notifications;

public class UserNotificationInfo
{
    public Guid? TenantId { get; set; }
    public string Name { get; set; }
    public long Id { get; set; }
    public ExtraPropertyDictionary ExtraProperties { get; set; }
    public string NotificationTypeName { get; set; }
    public DateTime CreationTime { get; set; }
    public NotificationType Type { get; set; }
    public NotificationContentType ContentType { get; set; }
    public NotificationSeverity Severity { get; set; }
    public NotificationReadState State { get; set; }
}
