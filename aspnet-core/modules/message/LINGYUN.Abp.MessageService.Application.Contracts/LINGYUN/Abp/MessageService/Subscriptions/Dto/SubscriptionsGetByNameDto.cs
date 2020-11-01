using LINGYUN.Abp.MessageService.Notifications;
using System.ComponentModel.DataAnnotations;

namespace LINGYUN.Abp.MessageService.Subscriptions
{
    public class SubscriptionsGetByNameDto
    {
        [Required]
        [StringLength(NotificationConsts.MaxNameLength)]
        public string Name { get; set; }
    }
}
