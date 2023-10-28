using System.ComponentModel.DataAnnotations;
using Volo.Abp.Validation;

namespace LINGYUN.Abp.Notifications.Definitions.Notifications;

public class NotificationDefinitionCreateDto : NotificationDefinitionCreateOrUpdateDto
{
    [Required]
    [DynamicStringLength(typeof(NotificationDefinitionRecordConsts), nameof(NotificationDefinitionRecordConsts.MaxNameLength))]
    public string Name { get; set; }

    [Required]
    [DynamicStringLength(typeof(NotificationDefinitionGroupRecordConsts), nameof(NotificationDefinitionGroupRecordConsts.MaxNameLength))]
    public string GroupName { get; set; }
}
