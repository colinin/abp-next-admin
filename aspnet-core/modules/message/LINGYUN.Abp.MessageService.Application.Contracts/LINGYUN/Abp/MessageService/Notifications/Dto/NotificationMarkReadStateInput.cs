using LINGYUN.Abp.Notifications;
using System.ComponentModel.DataAnnotations;

namespace LINGYUN.Abp.MessageService.Notifications;

public class NotificationMarkReadStateInput
{
    [Required]
    public long[] IdList { get; set; }

    public NotificationReadState State { get; set; }
}
