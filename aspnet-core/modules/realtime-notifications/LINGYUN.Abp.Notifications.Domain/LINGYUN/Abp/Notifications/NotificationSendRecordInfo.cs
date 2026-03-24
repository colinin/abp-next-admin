using System;

namespace LINGYUN.Abp.Notifications;
public class NotificationSendRecordInfo
{
    public long Id { get; set; }
    public string Provider { get; set; }
    public DateTime SendTime { get; set; }
    public Guid UserId { get; set; }
    public string UserName { get; set; }
    public NotificationSendState State { get; set; }
    public string Reason { get; set; }
    public UserNotificationInfo NotificationInfo { get; set; }
}
