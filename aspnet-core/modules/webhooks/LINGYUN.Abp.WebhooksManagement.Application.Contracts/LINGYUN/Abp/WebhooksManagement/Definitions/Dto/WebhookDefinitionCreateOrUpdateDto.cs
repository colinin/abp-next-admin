using System.Collections.Generic;
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

    public List<string> RequiredFeatures { get; set; } = new List<string>();

    public ExtraPropertyDictionary ExtraProperties { get; set; } = new ExtraPropertyDictionary();
}
