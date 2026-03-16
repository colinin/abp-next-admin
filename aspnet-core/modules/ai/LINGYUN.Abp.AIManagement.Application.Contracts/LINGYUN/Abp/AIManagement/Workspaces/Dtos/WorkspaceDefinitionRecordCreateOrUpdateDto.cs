using System.ComponentModel.DataAnnotations;
using Volo.Abp.ObjectExtending;
using Volo.Abp.Validation;

namespace LINGYUN.Abp.AIManagement.Workspaces.Dtos;
public abstract class WorkspaceDefinitionRecordCreateOrUpdateDto : ExtensibleObject
{
    [Required]
    [DynamicStringLength(typeof(WorkspaceDefinitionRecordConsts), nameof(WorkspaceDefinitionRecordConsts.MaxProviderLength))]
    public string Provider { get; set; }

    [Required]
    [DynamicStringLength(typeof(WorkspaceDefinitionRecordConsts), nameof(WorkspaceDefinitionRecordConsts.MaxModelNameLength))]
    public string ModelName { get; set; }

    [Required]
    [DynamicStringLength(typeof(WorkspaceDefinitionRecordConsts), nameof(WorkspaceDefinitionRecordConsts.MaxDisplayNameLength))]
    public string DisplayName { get; set; }

    [DynamicStringLength(typeof(WorkspaceDefinitionRecordConsts), nameof(WorkspaceDefinitionRecordConsts.MaxDescriptionLength))]
    public string? Description { get; set; }

    [DynamicStringLength(typeof(WorkspaceDefinitionRecordConsts), nameof(WorkspaceDefinitionRecordConsts.MaxApiKeyLength))]
    public string? ApiKey { get; set; }

    [DynamicStringLength(typeof(WorkspaceDefinitionRecordConsts), nameof(WorkspaceDefinitionRecordConsts.MaxApiBaseUrlLength))]
    public string? ApiBaseUrl { get; set; }

    [DynamicStringLength(typeof(WorkspaceDefinitionRecordConsts), nameof(WorkspaceDefinitionRecordConsts.MaxSystemPromptLength))]
    public string? SystemPrompt { get; set; }

    [DynamicStringLength(typeof(WorkspaceDefinitionRecordConsts), nameof(WorkspaceDefinitionRecordConsts.MaxInstructionsLength))]
    public string? Instructions { get; set; }

    public float? Temperature { get; set; }

    public int? MaxOutputTokens { get; set; }

    public float? FrequencyPenalty { get; set; }

    public float? PresencePenalty { get; set; }

    public bool IsEnabled { get; set; }

    [DynamicStringLength(typeof(WorkspaceDefinitionRecordConsts), nameof(WorkspaceDefinitionRecordConsts.MaxStateCheckersLength))]
    public string? StateCheckers { get; set; }
}
