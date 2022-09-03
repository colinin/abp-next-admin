using LINGYUN.Abp.Notifications;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace LINGYUN.Abp.MessageService.Notifications
{
    public class NotificationSendDto
    {
        [Required]
        [StringLength(NotificationConsts.MaxNameLength)]
        [DisplayName("Notifications:Name")]
        public string Name { get; set; }

        [StringLength(NotificationConsts.MaxNameLength)]
        [DisplayName("Notifications:TemplateName")]
        public string TemplateName { get; set; }

        [DisplayName("Notifications:Data")]
        public Dictionary<string, object> Data { get; set; } = new Dictionary<string, object>();

        [DisplayName("Notifications:Culture")]
        public string Culture { get; set; }

        [DisplayName("Notifications:ToUserId")]
        public Guid? ToUserId { get; set; }

        [StringLength(128)]
        [DisplayName("Notifications:ToUserName")]
        public string ToUserName { get; set; }

        [DisplayName("Notifications:Severity")]
        public NotificationSeverity Severity { get; set; } = NotificationSeverity.Info;
    }
}
