using System;
using Volo.Abp.Application.Dtos;

namespace LINGYUN.Abp.Notifications;
public class NotificationSendRecordDto : EntityDto<string>
{
    public string Provider { get; set; }
    public DateTime SendTime { get; set; }
    public Guid UserId { get; set; }
    public string UserName { get; set; }
    public NotificationSendState State { get; set; }
    public string Reason { get; set; }
    public UserNotificationDto Notification { get; set; }
}
