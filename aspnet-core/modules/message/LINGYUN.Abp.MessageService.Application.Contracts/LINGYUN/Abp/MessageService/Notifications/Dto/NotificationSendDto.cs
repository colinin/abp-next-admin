using LINGYUN.Abp.Notifications;
using System;
using System.ComponentModel.DataAnnotations;

namespace LINGYUN.Abp.MessageService.Notifications
{
    public class NotificationSendDto
    {
        [Required]
        [StringLength(NotificationConsts.MaxNameLength)]
        public string Name { get; set; }

        public NotificationData Data { get; set; } = new NotificationData();

        public Guid? ToUserId { get; set; }

        [StringLength(128)]
        public string ToUserName { get; set; }

        public NotificationSeverity Severity { get; set; } = NotificationSeverity.Info;
    }
}
