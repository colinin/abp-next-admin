using System.ComponentModel.DataAnnotations;
using Volo.Abp.Validation;

namespace LINGYUN.Abp.MessageService.Notifications;

public class NotificationTemplateSetInput
{
    [Required]
    [DynamicStringLength(typeof(NotificationTemplateConsts), nameof(NotificationTemplateConsts.MaxNameLength))]
    public string Name { get; set; }

    [Required]
    [DynamicStringLength(typeof(NotificationTemplateConsts), nameof(NotificationTemplateConsts.MaxCultureLength))]
    public string Culture { get; set; }

    [Required]
    [DynamicStringLength(typeof(NotificationTemplateConsts), nameof(NotificationTemplateConsts.MaxContentLength))]
    public string Content { get; set; }
}
