using LINGYUN.Abp.WebhooksManagement.Definitions.Dto;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Validation;

namespace LINGYUN.Abp.WebhooksManagement.Definitions;

public class WebhookDefinitionCreateDto : WebhookDefinitionCreateOrUpdateDto
{
    [Required]
    [DynamicStringLength(typeof(WebhookDefinitionRecordConsts), nameof(WebhookGroupDefinitionRecordConsts.MaxNameLength))]
    public string GroupName { get; set; }

    [Required]
    [DynamicStringLength(typeof(WebhookDefinitionRecordConsts), nameof(WebhookDefinitionRecordConsts.MaxNameLength))]
    public string Name { get; set; }
}
