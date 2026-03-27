using System.ComponentModel.DataAnnotations;
using Volo.Abp.ObjectExtending;
using Volo.Abp.Validation;

namespace LINGYUN.Abp.AIManagement.Tools.Dtos;
public abstract class AIToolDefinitionRecordCreateOrUpdateDto : ExtensibleObject
{
    [Required]
    [DynamicStringLength(typeof(AIToolDefinitionRecordConsts), nameof(AIToolDefinitionRecordConsts.MaxProviderLength))]
    public string Provider { get; set; }

    [DynamicStringLength(typeof(AIToolDefinitionRecordConsts), nameof(AIToolDefinitionRecordConsts.MaxDescriptionLength))]
    public string? Description { get; set; }

    public bool IsEnabled { get; set; }

    public bool IsGlobal { get; set; }

    [DynamicStringLength(typeof(AIToolDefinitionRecordConsts), nameof(AIToolDefinitionRecordConsts.MaxStateCheckersLength))]
    public string? StateCheckers { get; set; }
}
