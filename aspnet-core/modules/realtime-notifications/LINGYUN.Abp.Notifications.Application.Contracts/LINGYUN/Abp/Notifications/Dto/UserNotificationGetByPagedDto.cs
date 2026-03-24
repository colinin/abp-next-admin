using System;
using System.ComponentModel;
using Volo.Abp.Application.Dtos;

namespace LINGYUN.Abp.Notifications;

public class UserNotificationGetByPagedDto : PagedAndSortedResultRequestDto
{
    public string Filter { get; set; }

    [DisplayName("Notifications:State")]
    public NotificationReadState? ReadState { get; set; }
    public DateTime? BeginCreationTime { get; set; }
    public DateTime? EndCreationTime { get; set; }
}
