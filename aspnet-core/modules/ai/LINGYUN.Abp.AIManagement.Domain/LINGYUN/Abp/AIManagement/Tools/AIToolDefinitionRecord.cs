using JetBrains.Annotations;
using System;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace LINGYUN.Abp.AIManagement.Tools;
public class AIToolDefinitionRecord : AuditedAggregateRoot<Guid>
{
    public string Name { get; private set; }

    public string Provider { get; private set; }

    public string? Description { get; set; }

    public bool IsEnabled { get; set; }

    public bool IsSystem { get; set; }

    public string? StateCheckers { get; set; }
    protected AIToolDefinitionRecord()
    {

    }

    public AIToolDefinitionRecord(
        [NotNull] string name,
        [NotNull] string provider,
        [CanBeNull] string? description = null)
    {
        Name = Check.NotNullOrWhiteSpace(name, nameof(name), AIToolDefinitionRecordConsts.MaxNameLength);
        Provider = Check.NotNullOrWhiteSpace(provider, nameof(provider), AIToolDefinitionRecordConsts.MaxProviderLength);
        Description = Check.Length(description, nameof(description), AIToolDefinitionRecordConsts.MaxDescriptionLength);
    }
}
