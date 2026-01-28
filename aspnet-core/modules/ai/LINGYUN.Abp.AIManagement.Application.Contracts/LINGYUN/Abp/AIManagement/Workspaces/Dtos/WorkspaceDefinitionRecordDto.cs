using System;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;

namespace LINGYUN.Abp.AIManagement.Workspaces.Dtos;

[Serializable]
public class WorkspaceDefinitionRecordDto : ExtensibleAuditedEntityDto<Guid>, IHasConcurrencyStamp
{
    public string Name { get; set; }

    public string Provider { get; set; }

    public string ModelName { get; set; }

    public string DisplayName { get; set; }

    public string? Description { get; set; }

    public string? ApiBaseUrl { get; set; }

    public string? SystemPrompt { get; set; }

    public string? Instructions { get; set; }

    public float? Temperature { get; set; }

    public int? MaxOutputTokens { get; set; }

    public float? FrequencyPenalty { get; set; }

    public float? PresencePenalty { get; set; }

    public bool IsEnabled { get; set; }

    public string? StateCheckers { get; set; }

    public string ConcurrencyStamp { get; set; }
}
