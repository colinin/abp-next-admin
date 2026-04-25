using System;
using Volo.Abp.Application.Dtos;

namespace LINGYUN.Abp.Notifications;

#nullable enable
public class NotificationSendRecordGetPagedListInput : PagedAndSortedResultRequestDto
{
    public string? Provider { get; set; }
    public DateTime? BeginSendTime { get; set; }
    public DateTime? EndSendTime { get; set; }
    public Guid? UserId { get; set; }
    public string? NotificationName { get; set; }
    public NotificationSendState? State { get; set; }
}
#nullable disable
