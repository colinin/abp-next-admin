using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace LINGYUN.Abp.Notifications;

public class NotificationSendDto
{
    [Required]
    [StringLength(NotificationConsts.MaxNameLength)]
    [DisplayName("Notifications:Name")]
    public string Name { get; set; }

    [DisplayName("Notifications:Data")]
    public Dictionary<string, object> Data { get; set; } = new Dictionary<string, object>();

    [DisplayName("Notifications:Culture")]
    public string Culture { get; set; }

    [DisplayName("Notifications:ToUserId")]
    public List<UserIdentifier> ToUsers { get; set; } = new List<UserIdentifier>();

    [DisplayName("Notifications:Severity")]
    public NotificationSeverity Severity { get; set; } = NotificationSeverity.Info;
}
