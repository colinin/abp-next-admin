using JetBrains.Annotations;
using LINGYUN.Abp.Dynamic.Definitions;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.AI.Tools;

public class AIToolDefinitionManager : 
    DynamicDefinitionManager<AIToolDefinition>,
    IAIToolDefinitionManager,
    ITransientDependency
{
    protected IStaticAIToolDefinitionStore StaticStore { get; }
    protected IDynamicAIToolDefinitionStore DynamicStore { get; }
    protected AbpAIToolsOptions AIToolsOptions { get; }

    public AIToolDefinitionManager(
        IStaticAIToolDefinitionStore staticStore,
        IDynamicAIToolDefinitionStore dynamicStore,
        IOptions<AbpDynamicDefinitionsOptions> options,
        IOptions<AbpAIToolsOptions> aIToolsOptions)
        : base(options)
    {
        StaticStore = staticStore;
        DynamicStore = dynamicStore;
        AIToolsOptions = aIToolsOptions.Value;
    }

    public async virtual Task<AIToolDefinition> GetAsync([NotNull] string name)
    {
        return await GetOrNullAsync(name) ?? throw new AbpException("Undefined AITool: " + name);
    }

    public async virtual Task<IReadOnlyList<AIToolDefinition>> GetAllAsync()
    {
        var staticDefinitions = await StaticStore.GetAllAsync();
        var dynamicDefinitions = await DynamicStore.GetAllAsync();

        return await GetDefinitionsAsync(staticDefinitions, dynamicDefinitions);
    }

    public async virtual Task<AIToolDefinition?> GetOrNullAsync([NotNull] string name)
    {
        Check.NotNull(name, nameof(name));

        var staticDefinition = await StaticStore.GetOrNullAsync(name);
        var dynamicDefinition = await DynamicStore.GetOrNullAsync(name);

        return await GetDefinitionAsync(staticDefinition, dynamicDefinition);
    }

    protected override string GetDefinitionKey(AIToolDefinition definition)
    {
        return definition.Name;
    }

    protected override Task AfterMergedDefinitionsAsync(Dictionary<string, AIToolDefinition> mergedDefinitions)
    {
        foreach (var deletedToolName in AIToolsOptions.DeletedAITools)
        {
            mergedDefinitions.Remove(deletedToolName);
        }

        return Task.CompletedTask;
    }

    protected override Task<AIToolDefinition> MergeDefinitionAsync(AIToolDefinition targetDefinition, AIToolDefinition sourceDefinition)
    {
        var provider = !string.IsNullOrEmpty(sourceDefinition.Provider)
            ? sourceDefinition.Provider
            : targetDefinition.Provider;
        var description = sourceDefinition.Description ?? targetDefinition.Description;

        var mergedAITool = new AIToolDefinition(
            targetDefinition.Name,
            provider,
            description
        )
        {
            IsEnabled = targetDefinition.IsEnabled || sourceDefinition.IsEnabled,
            IsGlobal = targetDefinition.IsGlobal || sourceDefinition.IsGlobal
        };

        foreach (var checker in targetDefinition.StateCheckers)
        {
            if (!mergedAITool.StateCheckers.Contains(checker))
            {
                mergedAITool.StateCheckers.Add(checker);
            }
        }

        foreach (var checker in sourceDefinition.StateCheckers)
        {
            if (!mergedAITool.StateCheckers.Contains(checker))
            {
                mergedAITool.StateCheckers.Add(checker);
            }
        }

        foreach (var property in targetDefinition.Properties)
        {
            mergedAITool.Properties[property.Key] = property.Value;
        }

        foreach (var property in sourceDefinition.Properties)
        {
            mergedAITool.Properties[property.Key] = property.Value;
        }

        return Task.FromResult(mergedAITool);
    }
}
