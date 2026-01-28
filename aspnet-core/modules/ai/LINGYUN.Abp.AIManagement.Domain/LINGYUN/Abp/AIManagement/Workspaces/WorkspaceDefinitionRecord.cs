using System;
using Volo.Abp;
using Volo.Abp.Data;
using Volo.Abp.Domain.Entities.Auditing;

namespace LINGYUN.Abp.AIManagement.Workspaces;
public class WorkspaceDefinitionRecord : AuditedAggregateRoot<Guid>
{
    public string Name { get; private set; }

    public string Provider { get; private set; }

    public string ModelName { get; private set; }

    public string DisplayName { get; private set; }

    public string? Description { get; set; }

    public string? ApiKey { get; private set; }

    public string? ApiBaseUrl { get; private set; }

    public string? SystemPrompt { get; set; }

    public string? Instructions { get; set; }

    public float? Temperature { get; set; }

    public int? MaxOutputTokens { get; set; }

    public float? FrequencyPenalty { get; set; }

    public float? PresencePenalty { get; set; }

    public bool IsEnabled { get; set; }

    public string? StateCheckers { get; set; }

    protected WorkspaceDefinitionRecord()
    {
        ExtraProperties = new ExtraPropertyDictionary();
        this.SetDefaultsForExtraProperties();
    }

    public WorkspaceDefinitionRecord(
        Guid id,
        string name, 
        string provider,
        string modelName,
        string displayName,
        string? description = null,
        string? systemPrompt = null,
        string? instructions = null, 
        float? temperature = null, 
        int? maxOutputTokens = null, 
        float? frequencyPenalty = null,
        float? presencePenalty = null,
        string? stateCheckers = null)
        : base(id)
    {
        Name = Check.NotNullOrWhiteSpace(name, nameof(name), WorkspaceDefinitionRecordConsts.MaxNameLength);
        Provider = Check.NotNullOrWhiteSpace(provider, nameof(provider), WorkspaceDefinitionRecordConsts.MaxProviderLength);
        ModelName = Check.NotNullOrWhiteSpace(modelName, nameof(modelName), WorkspaceDefinitionRecordConsts.MaxModelNameLength);
        DisplayName = Check.NotNullOrWhiteSpace(displayName, nameof(displayName), WorkspaceDefinitionRecordConsts.MaxDisplayNameLength);
        Description = Check.Length(description, nameof(description), WorkspaceDefinitionRecordConsts.MaxDescriptionLength);
        SystemPrompt = Check.Length(systemPrompt, nameof(systemPrompt), WorkspaceDefinitionRecordConsts.MaxSystemPromptLength);
        Instructions = Check.Length(instructions, nameof(instructions), WorkspaceDefinitionRecordConsts.MaxInstructionsLength);
        StateCheckers = Check.Length(stateCheckers, nameof(stateCheckers), WorkspaceDefinitionRecordConsts.MaxStateCheckersLength);
        Temperature = temperature;
        MaxOutputTokens = maxOutputTokens;
        FrequencyPenalty = frequencyPenalty;
        PresencePenalty = presencePenalty;

        IsEnabled = true;
        ExtraProperties = new ExtraPropertyDictionary();
        this.SetDefaultsForExtraProperties();
    }

    public void SetDisplayName(string displayName)
    {
        DisplayName = Check.NotNullOrWhiteSpace(displayName, nameof(displayName), WorkspaceDefinitionRecordConsts.MaxDisplayNameLength);
    }

    public void SetModel(string provider, string modelName)
    {
        Provider = Check.NotNullOrWhiteSpace(provider, nameof(provider), WorkspaceDefinitionRecordConsts.MaxProviderLength);
        ModelName = Check.NotNullOrWhiteSpace(modelName, nameof(modelName), WorkspaceDefinitionRecordConsts.MaxModelNameLength);
    }

    public void SetApiKey(string? apiKey = null, string? apiBaseUrl = null)
    {
        ApiKey = Check.Length(apiKey, nameof(apiKey), WorkspaceDefinitionRecordConsts.MaxApiKeyLength);
        ApiBaseUrl = Check.Length(apiBaseUrl, nameof(apiBaseUrl), WorkspaceDefinitionRecordConsts.MaxApiBaseUrlLength);
    }

    public bool HasSameData(WorkspaceDefinitionRecord otherWorkspace)
    {
        if (Name != otherWorkspace.Name)
        {
            return false;
        }

        if (Provider != otherWorkspace.Provider)
        {
            return false;
        }

        if (ModelName != otherWorkspace.ModelName)
        {
            return false;
        }

        if (DisplayName != otherWorkspace.DisplayName)
        {
            return false;
        }

        if (Description != otherWorkspace.Description)
        {
            return false;
        }

        if (ApiKey != otherWorkspace.ApiKey)
        {
            return false;
        }

        if (ApiBaseUrl != otherWorkspace.ApiBaseUrl)
        {
            return false;
        }

        if (SystemPrompt != otherWorkspace.SystemPrompt)
        {
            return false;
        }

        if (Instructions != otherWorkspace.Instructions)
        {
            return false;
        }

        if (IsEnabled != otherWorkspace.IsEnabled)
        {
            return false;
        }

        if (Temperature != otherWorkspace.Temperature)
        {
            return false;
        }

        if (MaxOutputTokens != otherWorkspace.MaxOutputTokens)
        {
            return false;
        }

        if (FrequencyPenalty != otherWorkspace.FrequencyPenalty)
        {
            return false;
        }

        if (PresencePenalty != otherWorkspace.PresencePenalty)
        {
            return false;
        }

        if (StateCheckers != otherWorkspace.StateCheckers)
        {
            return false;
        }

        if (!this.HasSameExtraProperties(otherWorkspace))
        {
            return false;
        }

        return true;
    }

    public void Patch(WorkspaceDefinitionRecord otherWorkspace)
    {
        if (Name != otherWorkspace.Name)
        {
            Name = otherWorkspace.Name;
        }

        if (Provider != otherWorkspace.Provider)
        {
            Provider = otherWorkspace.Provider;
        }

        if (ModelName != otherWorkspace.ModelName)
        {
            ModelName = otherWorkspace.ModelName;
        }

        if (DisplayName != otherWorkspace.DisplayName)
        {
            DisplayName = otherWorkspace.DisplayName;
        }

        if (Description != otherWorkspace.Description)
        {
            Description = otherWorkspace.Description;
        }

        if (ApiKey != otherWorkspace.ApiKey)
        {
            ApiKey = otherWorkspace.ApiKey;
        }

        if (ApiBaseUrl != otherWorkspace.ApiBaseUrl)
        {
            ApiBaseUrl = otherWorkspace.ApiBaseUrl;
        }

        if (SystemPrompt != otherWorkspace.SystemPrompt)
        {
            SystemPrompt = otherWorkspace.SystemPrompt;
        }

        if (Instructions != otherWorkspace.Instructions)
        {
            Instructions = otherWorkspace.Instructions;
        }

        if (IsEnabled != otherWorkspace.IsEnabled)
        {
            IsEnabled = otherWorkspace.IsEnabled;
        }

        if (Temperature != otherWorkspace.Temperature)
        {
            Temperature = otherWorkspace.Temperature;
        }

        if (MaxOutputTokens != otherWorkspace.MaxOutputTokens)
        {
            MaxOutputTokens = otherWorkspace.MaxOutputTokens;
        }

        if (FrequencyPenalty != otherWorkspace.FrequencyPenalty)
        {
            FrequencyPenalty = otherWorkspace.FrequencyPenalty;
        }

        if (PresencePenalty != otherWorkspace.PresencePenalty)
        {
            PresencePenalty = otherWorkspace.PresencePenalty;
        }

        if (StateCheckers != otherWorkspace.StateCheckers)
        {
            StateCheckers = otherWorkspace.StateCheckers;
        }

        if (!this.HasSameExtraProperties(otherWorkspace))
        {
            ExtraProperties.Clear();

            foreach (var property in otherWorkspace.ExtraProperties)
            {
                ExtraProperties.Add(property.Key, property.Value);
            }
        }
    }
}
