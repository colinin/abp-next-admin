using LINGYUN.Abp.Notifications;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LINGYUN.Abp.MessageService.Notifications
{
    public class NotificationSendDto
    {
        [Required]
        [StringLength(NotificationConsts.MaxNameLength)]
        public string Name { get; set; }

        public Dictionary<string, object> Data { get; set; } = new Dictionary<string, object>();

        public string Culture { get; set; }

        public Guid? ToUserId { get; set; }

        [StringLength(128)]
        public string ToUserName { get; set; }

        public NotificationSeverity Severity { get; set; } = NotificationSeverity.Info;
    }
}
