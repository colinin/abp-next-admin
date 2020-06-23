using System.ComponentModel.DataAnnotations;

namespace LINGYUN.Abp.MessageService.Notifications
{
    public class NotificationGetByIdDto
    {
        [Required]
        public long NotificationId { get; set; }
    }
}
