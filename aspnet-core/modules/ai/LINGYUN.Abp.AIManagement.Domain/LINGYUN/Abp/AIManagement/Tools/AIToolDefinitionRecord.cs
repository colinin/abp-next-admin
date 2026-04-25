using JetBrains.Annotations;
using System;
using Volo.Abp;
using Volo.Abp.Data;
using Volo.Abp.Domain.Entities.Auditing;

namespace LINGYUN.Abp.AIManagement.Tools;
public class AIToolDefinitionRecord : AuditedAggregateRoot<Guid>
{
    public string Name { get; private set; }

    public string Provider { get; private set; }

    public string? Description { get; set; }

    public bool IsEnabled { get; set; }

    public bool IsSystem { get; set; }

    public bool IsGlobal { get; set; }

    public string? StateCheckers { get; set; }
    protected AIToolDefinitionRecord()
    {

    }

    public AIToolDefinitionRecord(
        [NotNull] Guid id,
        [NotNull] string name,
        [NotNull] string provider,
        [CanBeNull] string? description = null,
        [CanBeNull] string? stateCheckers = null)
        : base(id)
    {
        Name = Check.NotNullOrWhiteSpace(name, nameof(name), AIToolDefinitionRecordConsts.MaxNameLength);
        Provider = Check.NotNullOrWhiteSpace(provider, nameof(provider), AIToolDefinitionRecordConsts.MaxProviderLength);
        Description = Check.Length(description, nameof(description), AIToolDefinitionRecordConsts.MaxDescriptionLength);
        StateCheckers = Check.Length(stateCheckers, nameof(stateCheckers), AIToolDefinitionRecordConsts.MaxStateCheckersLength);
    }

    public bool HasSameData(AIToolDefinitionRecord otherAITool)
    {
        if (Name != otherAITool.Name)
        {
            return false;
        }

        if (Provider != otherAITool.Provider)
        {
            return false;
        }

        if (Description != otherAITool.Description)
        {
            return false;
        }

        if (IsEnabled != otherAITool.IsEnabled)
        {
            return false;
        }

        if (IsSystem != otherAITool.IsSystem)
        {
            return false;
        }

        if (IsGlobal != otherAITool.IsGlobal)
        {
            return false;
        }

        if (StateCheckers != otherAITool.StateCheckers)
        {
            return false;
        }

        if (!this.HasSameExtraProperties(otherAITool))
        {
            return false;
        }

        return true;
    }

    public void Patch(AIToolDefinitionRecord otherAITool)
    {
        if (Name != otherAITool.Name)
        {
            Name = otherAITool.Name;
        }

        if (Provider != otherAITool.Provider)
        {
            Provider = otherAITool.Provider;
        }

        if (Description != otherAITool.Description)
        {
            Description = otherAITool.Description;
        }

        if (IsEnabled != otherAITool.IsEnabled)
        {
            IsEnabled = otherAITool.IsEnabled;
        }

        if (IsSystem != otherAITool.IsSystem)
        {
            IsSystem = otherAITool.IsSystem;
        }

        if (IsGlobal != otherAITool.IsGlobal)
        {
            IsGlobal = otherAITool.IsGlobal;
        }

        if (StateCheckers != otherAITool.StateCheckers)
        {
            StateCheckers = otherAITool.StateCheckers;
        }

        if (!this.HasSameExtraProperties(otherAITool))
        {
            ExtraProperties.Clear();

            foreach (var property in otherAITool.ExtraProperties)
            {
                ExtraProperties.Add(property.Key, property.Value);
            }
        }
    }
}
