using System.ComponentModel.DataAnnotations;
using Volo.Abp.Data;
using Volo.Abp.Validation;

namespace LINGYUN.Abp.WebhooksManagement.Definitions;
public abstract class WebhookGroupDefinitionCreateOrUpdateDto : IHasExtraProperties
{
    [Required]
    [DynamicStringLength(typeof(WebhookGroupDefinitionRecordConsts), nameof(WebhookGroupDefinitionRecordConsts.MaxDisplayNameLength))]
    public string DisplayName { get; set; }

    public ExtraPropertyDictionary ExtraProperties { get; set; } = new ExtraPropertyDictionary();
}
