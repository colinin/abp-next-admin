using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;

namespace LINGYUN.Abp.Dynamic.Definitions;

public abstract class DynamicDefinitionManager<TGroupDefinition, TDefinition> : DynamicDefinitionManager<TDefinition>
{
    protected DynamicDefinitionManager(
        IOptions<AbpDynamicDefinitionsOptions> options) : base(options)
    {
    }

    public async virtual Task<TGroupDefinition?> GetGroupDefinitionAsync(
        TGroupDefinition? staticGroupDefinition, 
        TGroupDefinition? dynamicGroupDefinition)
    {
        if (staticGroupDefinition != null && dynamicGroupDefinition != null)
        {
            var strategy = Options.GetStrategy<TGroupDefinition>();
            switch (strategy)
            {
                case DynamicDefinitionStrategy.Dynamic:
                    return dynamicGroupDefinition;
                case DynamicDefinitionStrategy.Merge:
                    await MergeGroupDefinitionAsync(staticGroupDefinition, dynamicGroupDefinition);
                    return staticGroupDefinition;
                case DynamicDefinitionStrategy.Static:
                default:
                    return staticGroupDefinition;
            }
        }

        return staticGroupDefinition ?? dynamicGroupDefinition;
    }

    public async virtual Task<IReadOnlyList<TGroupDefinition>> GetGroupDefinitionsAsync(
        IReadOnlyList<TGroupDefinition> staticGroupDefinitions, 
        IReadOnlyList<TGroupDefinition> dynamicGroupDefinitions)
    {
        var strategy = Options.GetStrategy<TGroupDefinition>();
        return strategy switch
        {
            DynamicDefinitionStrategy.Dynamic => await GetDynamicGroupDefinitionsAsync(staticGroupDefinitions, dynamicGroupDefinitions),
            DynamicDefinitionStrategy.Merge => await GetMergedGroupDefinitionsAsync(staticGroupDefinitions, dynamicGroupDefinitions),
            _ => await GetStaticGroupDefinitionsAsync(staticGroupDefinitions, dynamicGroupDefinitions),
        };
    }

    protected virtual Task<IReadOnlyList<TGroupDefinition>> GetStaticGroupDefinitionsAsync(
        IReadOnlyList<TGroupDefinition> staticGroupDefinitions, 
        IReadOnlyList<TGroupDefinition> dynamicGroupDefinitions)
    {
        var staticDefinitionKeys = staticGroupDefinitions
            .Select(GetGroupDefinitionKey)
            .ToImmutableHashSet();

        return Task.FromResult<IReadOnlyList<TGroupDefinition>>(
            staticGroupDefinitions
                .Concat(dynamicGroupDefinitions.Where(d => !staticDefinitionKeys.Contains(GetGroupDefinitionKey(d))))
                .ToImmutableList()
        );
    }

    protected virtual Task<IReadOnlyList<TGroupDefinition>> GetDynamicGroupDefinitionsAsync(
        IReadOnlyList<TGroupDefinition> staticGroupDefinitions,
        IReadOnlyList<TGroupDefinition> dynamicGroupDefinitions)
    {
        var dynamicDefinitionKeys = dynamicGroupDefinitions
            .Select(GetGroupDefinitionKey)
            .ToImmutableHashSet();

        return Task.FromResult<IReadOnlyList<TGroupDefinition>>(
            dynamicGroupDefinitions
                .Concat(staticGroupDefinitions.Where(s => !dynamicDefinitionKeys.Contains(GetGroupDefinitionKey(s))))
                .ToImmutableList()
        );
    }

    protected async virtual Task<IReadOnlyList<TGroupDefinition>> GetMergedGroupDefinitionsAsync(
        IReadOnlyList<TGroupDefinition> staticGroupDefinitions, 
        IReadOnlyList<TGroupDefinition> dynamicGroupDefinitions)
    {
        var mergedGroups = new Dictionary<string, TGroupDefinition>();

        foreach (var staticGroup in staticGroupDefinitions)
        {
            mergedGroups[GetGroupDefinitionKey(staticGroup)] = staticGroup;
        }

        foreach (var dynamicGroup in dynamicGroupDefinitions)
        {
            if (mergedGroups.TryGetValue(GetGroupDefinitionKey(dynamicGroup), out var existingGroup))
            {
                await BeforeMergeGroupDefinitionAsync(existingGroup, dynamicGroup);
                await MergeGroupDefinitionAsync(existingGroup, dynamicGroup);
                await AfterMergeGroupDefinitionAsync(existingGroup, dynamicGroup);
            }
            else
            {
                mergedGroups[GetGroupDefinitionKey(dynamicGroup)] = dynamicGroup;
            }
        }

        await AfterMergedGroupDefinitionsAsync(mergedGroups);

        return mergedGroups.Values.ToImmutableList();
    }

    protected abstract string GetGroupDefinitionKey(TGroupDefinition groupDefinition);

    protected abstract Task MergeGroupDefinitionAsync(TGroupDefinition targetGroupDefinition, TGroupDefinition sourceGroupDefinition);

    protected virtual Task BeforeMergeGroupDefinitionAsync(TGroupDefinition targetGroupDefinition, TGroupDefinition sourceGroupDefinition)
    {
        return Task.CompletedTask;
    }

    protected virtual Task AfterMergeGroupDefinitionAsync(TGroupDefinition targetGroupDefinition, TGroupDefinition sourceGroupDefinition)
    {
        return Task.CompletedTask;
    }

    protected virtual Task AfterMergedGroupDefinitionsAsync(Dictionary<string, TGroupDefinition> mergedDefinitions)
    {
        return Task.CompletedTask;
    }
}

public abstract class DynamicDefinitionManager<TDefinition>
{
    protected AbpDynamicDefinitionsOptions Options { get; }
    protected DynamicDefinitionManager(IOptions<AbpDynamicDefinitionsOptions> options)
    {
        Options = options.Value;
    }

    public async virtual Task<TDefinition?> GetDefinitionAsync(TDefinition? staticDefinition, TDefinition? dynamicDefinition)
    {
        if (staticDefinition != null && dynamicDefinition != null)
        {
            var strategy = Options.GetStrategy<TDefinition>();
            return strategy switch
            {
                DynamicDefinitionStrategy.Static => staticDefinition,
                DynamicDefinitionStrategy.Dynamic => dynamicDefinition,
                DynamicDefinitionStrategy.Merge => await MergeDefinitionAsync(staticDefinition, dynamicDefinition),
                _ => staticDefinition
            };
        }

        return staticDefinition ?? dynamicDefinition;
    }

    public async virtual Task<IReadOnlyList<TDefinition>> GetDefinitionsAsync(IReadOnlyList<TDefinition> staticDefinitions, IReadOnlyList<TDefinition> dynamicDefinitions)
    {
        var strategy = Options.GetStrategy<TDefinition>();
        return strategy switch
        {
            DynamicDefinitionStrategy.Dynamic => await GetDynamicDefinitionsAsync(staticDefinitions, dynamicDefinitions),
            DynamicDefinitionStrategy.Merge => await GetMergedDefinitionsAsync(staticDefinitions, dynamicDefinitions),
            _ => await GetStaticDefinitionsAsync(staticDefinitions, dynamicDefinitions),
        };
    }

    protected virtual Task<IReadOnlyList<TDefinition>> GetStaticDefinitionsAsync(IReadOnlyList<TDefinition> staticDefinitions, IReadOnlyList<TDefinition> dynamicDefinitions)
    {
        var staticDefinitionKeys = staticDefinitions
            .Select(GetDefinitionKey)
            .ToImmutableHashSet();

        return Task.FromResult<IReadOnlyList<TDefinition>>(
            staticDefinitions
                .Concat(dynamicDefinitions.Where(d => !staticDefinitionKeys.Contains(GetDefinitionKey(d))))
                .ToImmutableList()
        );
    }

    protected virtual Task<IReadOnlyList<TDefinition>> GetDynamicDefinitionsAsync(IReadOnlyList<TDefinition> staticDefinitions, IReadOnlyList<TDefinition> dynamicDefinitions)
    {
        var dynamicDefinitionKeys = dynamicDefinitions
            .Select(GetDefinitionKey)
            .ToImmutableHashSet();

        return Task.FromResult<IReadOnlyList<TDefinition>>(
            dynamicDefinitions
                .Concat(staticDefinitions.Where(s => !dynamicDefinitionKeys.Contains(GetDefinitionKey(s))))
                .ToImmutableList()
        );
    }

    protected async virtual Task<IReadOnlyList<TDefinition>> GetMergedDefinitionsAsync(IReadOnlyList<TDefinition> staticDefinitions, IReadOnlyList<TDefinition> dynamicDefinitions)
    {
        var mergedDefinitions = new Dictionary<string, TDefinition>();

        foreach (var staticDefinition in staticDefinitions)
        {
            mergedDefinitions[GetDefinitionKey(staticDefinition)] = staticDefinition;
        }

        foreach (var dynamicDefinition in dynamicDefinitions)
        {
            if (mergedDefinitions.TryGetValue(GetDefinitionKey(dynamicDefinition), out var existingDefinition))
            {
                await BeforeMergeDefinitionAsync(existingDefinition, dynamicDefinition);
                await MergeDefinitionAsync(existingDefinition, dynamicDefinition);
                await AfterMergeDefinitionAsync(existingDefinition, dynamicDefinition);
            }
            else
            {
                mergedDefinitions[GetDefinitionKey(dynamicDefinition)] = dynamicDefinition;
            }
        }

        await AfterMergedDefinitionsAsync(mergedDefinitions);

        return mergedDefinitions.Values.ToImmutableList();
    }

    protected abstract string GetDefinitionKey(TDefinition definition);

    protected abstract Task<TDefinition> MergeDefinitionAsync(TDefinition targetDefinition, TDefinition sourceDefinition);

    protected virtual Task BeforeMergeDefinitionAsync(TDefinition targetDefinition, TDefinition sourceDefinition)
    {
        return Task.CompletedTask;
    }

    protected virtual Task AfterMergeDefinitionAsync(TDefinition targetDefinition, TDefinition sourceDefinition)
    {
        return Task.CompletedTask;
    }

    protected virtual Task AfterMergedDefinitionsAsync(Dictionary<string, TDefinition> mergedDefinitions)
    {
        return Task.CompletedTask;
    }
}
