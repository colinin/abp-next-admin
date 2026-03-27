using System;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;

namespace LINGYUN.Abp.AIManagement.Tools.Dtos;

[Serializable]
public class AIToolDefinitionRecordDto : ExtensibleAuditedEntityDto<Guid>, IHasConcurrencyStamp
{
    public string Name { get; set; }

    public string Provider { get; set; }

    public string? Description { get; set; }

    public bool IsEnabled { get; set; }

    public bool IsSystem { get; set; }

    public string? StateCheckers { get; set; }

    public string ConcurrencyStamp { get; set; }
}
