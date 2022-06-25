using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace LINGYUN.Abp.MessageService.Notifications
{
    public class NotificationGetByIdDto
    {
        [Required]
        [DisplayName("Notifications:Id")]
        public long NotificationId { get; set; }
    }
}
