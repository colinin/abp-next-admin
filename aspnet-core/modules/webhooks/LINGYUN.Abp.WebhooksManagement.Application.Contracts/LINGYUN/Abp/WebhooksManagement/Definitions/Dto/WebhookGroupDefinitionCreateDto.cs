using System.ComponentModel.DataAnnotations;
using Volo.Abp.Validation;

namespace LINGYUN.Abp.WebhooksManagement.Definitions;
public class WebhookGroupDefinitionCreateDto : WebhookGroupDefinitionCreateOrUpdateDto
{
    [Required]
    [DynamicStringLength(typeof(WebhookGroupDefinitionRecordConsts), nameof(WebhookGroupDefinitionRecordConsts.MaxNameLength))]
    public string Name { get; set; }
}
