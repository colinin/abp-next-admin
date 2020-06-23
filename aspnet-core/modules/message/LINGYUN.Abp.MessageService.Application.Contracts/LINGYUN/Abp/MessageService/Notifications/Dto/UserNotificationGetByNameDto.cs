using System.ComponentModel.DataAnnotations;

namespace LINGYUN.Abp.MessageService.Notifications
{
    public class UserNotificationGetByNameDto
    {
        [Required]
        [StringLength(NotificationConsts.MaxNameLength)]
        public string NotificationName { get; set; }
    }
}
