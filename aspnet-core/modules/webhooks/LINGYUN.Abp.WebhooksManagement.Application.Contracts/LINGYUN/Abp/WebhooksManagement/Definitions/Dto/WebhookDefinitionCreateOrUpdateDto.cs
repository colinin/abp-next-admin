using System.ComponentModel.DataAnnotations;
using Volo.Abp.Data;
using Volo.Abp.Validation;

namespace LINGYUN.Abp.WebhooksManagement.Definitions.Dto;

public abstract class WebhookDefinitionCreateOrUpdateDto : IHasExtraProperties
{
    [Required]
    [DynamicStringLength(typeof(WebhookDefinitionRecordConsts), nameof(WebhookDefinitionRecordConsts.MaxDisplayNameLength))]
    public string DisplayName { get; set; }

    [DynamicStringLength(typeof(WebhookDefinitionRecordConsts), nameof(WebhookDefinitionRecordConsts.MaxDescriptionLength))]
    public string Description { get; set; }

    public bool IsEnabled { get; set; }

    [DynamicStringLength(typeof(WebhookDefinitionRecordConsts), nameof(WebhookDefinitionRecordConsts.MaxRequiredFeaturesLength))]
    public string RequiredFeatures { get; set; }

    public ExtraPropertyDictionary ExtraProperties { get; set; } = new ExtraPropertyDictionary();
}
