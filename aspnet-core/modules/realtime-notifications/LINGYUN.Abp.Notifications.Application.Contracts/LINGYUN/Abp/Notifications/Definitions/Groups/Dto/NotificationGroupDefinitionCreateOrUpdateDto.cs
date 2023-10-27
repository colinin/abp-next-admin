using System.ComponentModel.DataAnnotations;
using Volo.Abp.Data;
using Volo.Abp.Validation;

namespace LINGYUN.Abp.Notifications.Definitions.Groups;

public abstract class NotificationGroupDefinitionCreateOrUpdateDto : IHasExtraProperties
{
    [Required]
    [DynamicStringLength(typeof(NotificationDefinitionGroupRecordConsts), nameof(NotificationDefinitionGroupRecordConsts.MaxDisplayNameLength))]
    public string DisplayName { get; set; }

    [DynamicStringLength(typeof(NotificationDefinitionGroupRecordConsts), nameof(NotificationDefinitionGroupRecordConsts.MaxDescriptionLength))]
    public string Description { get; set; }

    public bool AllowSubscriptionToClients { get; set; }

    public ExtraPropertyDictionary ExtraProperties { get; set; } = new ExtraPropertyDictionary();
}
