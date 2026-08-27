using JetBrains.Annotations;
using LINGYUN.Abp.Dynamic.Definitions;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.AI.Workspaces;

public class WorkspaceDefinitionManager :
    DynamicDefinitionManager<WorkspaceDefinition>,
    IWorkspaceDefinitionManager,
    ITransientDependency
{
    protected IStaticWorkspaceDefinitionStore StaticStore { get; }
    protected IDynamicWorkspaceDefinitionStore DynamicStore { get; }
    protected AbpAICoreOptions AIOptions { get; }

    public WorkspaceDefinitionManager(
        IStaticWorkspaceDefinitionStore staticStore,
        IDynamicWorkspaceDefinitionStore dynamicStore,
        IOptions<AbpDynamicDefinitionsOptions> options,
        IOptions<AbpAICoreOptions> aIOptions)
        : base(options)
    {
        StaticStore = staticStore;
        DynamicStore = dynamicStore;
        AIOptions = aIOptions.Value;
    }

    public async virtual Task<IReadOnlyList<WorkspaceDefinition>> GetAllAsync()
    {
        var staticDefinitions = await StaticStore.GetAllAsync();
        var dynamicDefinitions = await DynamicStore.GetAllAsync();

        return await GetDefinitionsAsync(staticDefinitions, dynamicDefinitions);
    }

    public async virtual Task<WorkspaceDefinition> GetAsync([NotNull] string name)
    {
        return await GetOrNullAsync(name) ?? throw new AbpException("Undefined Workspace: " + name);
    }

    public async virtual Task<WorkspaceDefinition?> GetOrNullAsync([NotNull] string name)
    {
        Check.NotNull(name, nameof(name));

        var staticDefinition = await StaticStore.GetOrNullAsync(name);
        var dynamicDefinition = await DynamicStore.GetOrNullAsync(name);

        return await GetDefinitionAsync(staticDefinition, dynamicDefinition);
    }

    protected override string GetDefinitionKey(WorkspaceDefinition definition)
    {
        return definition.Name;
    }

    protected override Task AfterMergedDefinitionsAsync(Dictionary<string, WorkspaceDefinition> mergedDefinitions)
    {
        foreach (var deletedWorkspaceName in AIOptions.DeletedWorkspaces)
        {
            mergedDefinitions.Remove(deletedWorkspaceName);
        }

        return Task.CompletedTask;
    }

    protected override Task<WorkspaceDefinition> MergeDefinitionAsync(WorkspaceDefinition targetDefinition, WorkspaceDefinition sourceDefinition)
    {
        var provider = !string.IsNullOrEmpty(sourceDefinition.Provider)
            ? sourceDefinition.Provider
            : targetDefinition.Provider;
        var modelName = !string.IsNullOrEmpty(sourceDefinition.ModelName)
            ? sourceDefinition.ModelName
            : targetDefinition.ModelName;
        var displayName = sourceDefinition.DisplayName ?? targetDefinition.DisplayName;
        var mergedWorkspace = new WorkspaceDefinition(
            targetDefinition.Name,
            provider,
            modelName,
            displayName
        )
        {
            Description = sourceDefinition.Description ?? targetDefinition.Description
        };

        if (!string.IsNullOrEmpty(sourceDefinition.ApiKey))
        {
            mergedWorkspace.WithApiKey(sourceDefinition.ApiKey!);
        }
        else if (!string.IsNullOrEmpty(targetDefinition.ApiKey))
        {
            mergedWorkspace.WithApiKey(targetDefinition.ApiKey!);
        }

        if (!string.IsNullOrEmpty(sourceDefinition.ApiBaseUrl))
        {
            mergedWorkspace.WithApiBaseUrl(sourceDefinition.ApiBaseUrl!);
        }
        else if (!string.IsNullOrEmpty(targetDefinition.ApiBaseUrl))
        {
            mergedWorkspace.WithApiBaseUrl(targetDefinition.ApiBaseUrl!);
        }
        mergedWorkspace.SystemPrompt = sourceDefinition.SystemPrompt ?? targetDefinition.SystemPrompt;
        mergedWorkspace.Instructions = sourceDefinition.Instructions ?? targetDefinition.Instructions;
        mergedWorkspace.Temperature = sourceDefinition.Temperature ?? targetDefinition.Temperature;
        mergedWorkspace.MaxOutputTokens = sourceDefinition.MaxOutputTokens ?? targetDefinition.MaxOutputTokens;
        mergedWorkspace.FrequencyPenalty = sourceDefinition.FrequencyPenalty ?? targetDefinition.FrequencyPenalty;
        mergedWorkspace.PresencePenalty = sourceDefinition.PresencePenalty ?? targetDefinition.PresencePenalty;
        mergedWorkspace.IsEnabled = targetDefinition.IsEnabled || sourceDefinition.IsEnabled;

        foreach (var checker in targetDefinition.StateCheckers)
        {
            if (!mergedWorkspace.StateCheckers.Contains(checker))
            {
                mergedWorkspace.StateCheckers.Add(checker);
            }
        }

        foreach (var checker in sourceDefinition.StateCheckers)
        {
            if (!mergedWorkspace.StateCheckers.Contains(checker))
            {
                mergedWorkspace.StateCheckers.Add(checker);
            }
        }

        foreach (var tool in targetDefinition.Tools)
        {
            if (!mergedWorkspace.Tools.Contains(tool))
            {
                mergedWorkspace.Tools.Add(tool);
            }
        }

        foreach (var tool in sourceDefinition.Tools)
        {
            if (!mergedWorkspace.Tools.Contains(tool))
            {
                mergedWorkspace.Tools.Add(tool);
            }
        }

        foreach (var property in targetDefinition.Properties)
        {
            mergedWorkspace.Properties[property.Key] = property.Value;
        }

        foreach (var property in sourceDefinition.Properties)
        {
            mergedWorkspace.Properties[property.Key] = property.Value;
        }

        return Task.FromResult(mergedWorkspace);
    }
}
