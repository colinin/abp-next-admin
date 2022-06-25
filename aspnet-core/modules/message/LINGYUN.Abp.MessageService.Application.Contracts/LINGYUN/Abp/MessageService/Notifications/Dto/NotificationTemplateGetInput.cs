using System.ComponentModel.DataAnnotations;

namespace LINGYUN.Abp.MessageService.Notifications;

public class NotificationTemplateGetInput
{
    [Required]
    public string Name { get; set; }

    public string Culture { get; set; }
}
