using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Data;
using Volo.Abp.Validation;

namespace LINGYUN.Abp.Notifications.Definitions.Notifications;

public abstract class NotificationDefinitionCreateOrUpdateDto : IHasExtraProperties
{
    [DynamicStringLength(typeof(NotificationDefinitionRecordConsts), nameof(NotificationDefinitionRecordConsts.MaxTemplateLength))]
    public string Template { get; set; }

    [Required]
    [DynamicStringLength(typeof(NotificationDefinitionRecordConsts), nameof(NotificationDefinitionRecordConsts.MaxDisplayNameLength))]
    public string DisplayName { get; set; }

    [DynamicStringLength(typeof(NotificationDefinitionRecordConsts), nameof(NotificationDefinitionRecordConsts.MaxDescriptionLength))]
    public string Description { get; set; }

    public bool AllowSubscriptionToClients { get; set; }

    public NotificationLifetime NotificationLifetime { get; set; } = NotificationLifetime.OnlyOne;

    public NotificationType NotificationType { get; set; } = NotificationType.User;

    public NotificationContentType ContentType { get; set; } = NotificationContentType.Text;

    public List<string> Providers { get; set; } = new List<string>();

    public ExtraPropertyDictionary ExtraProperties { get; set; } = new ExtraPropertyDictionary();
}
