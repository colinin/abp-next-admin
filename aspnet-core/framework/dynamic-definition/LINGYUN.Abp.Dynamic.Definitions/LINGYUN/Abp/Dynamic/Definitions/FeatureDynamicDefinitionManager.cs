using JetBrains.Annotations;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Features;

namespace LINGYUN.Abp.Dynamic.Definitions;

[Dependency(ReplaceServices = true)]
[ExposeServices(
    typeof(IFeatureDefinitionManager),
    typeof(FeatureDynamicDefinitionManager))]
public class FeatureDynamicDefinitionManager : 
    DynamicDefinitionManager<FeatureGroupDefinition, FeatureDefinition>, 
    IFeatureDefinitionManager,
    ISingletonDependency
{
    protected IStaticFeatureDefinitionStore StaticStore { get; }
    protected IDynamicFeatureDefinitionStore DynamicStore { get; }
    public FeatureDynamicDefinitionManager(
        IStaticFeatureDefinitionStore staticStore,
        IDynamicFeatureDefinitionStore dynamicStore,
        IOptions<AbpDynamicDefinitionsOptions> options)
        : base(options)
    {
        StaticStore = staticStore;
        DynamicStore = dynamicStore;
    }

    public async virtual Task<IReadOnlyList<FeatureDefinition>> GetAllAsync()
    {
        var staticDefinitions = await StaticStore.GetFeaturesAsync();
        var dynamicDefinitions = await DynamicStore.GetFeaturesAsync();

        return await GetDefinitionsAsync(staticDefinitions, dynamicDefinitions);
    }

    public async virtual Task<FeatureDefinition> GetAsync(string name)
    {
        return await GetOrNullAsync(name) ?? throw new AbpException("Undefined feature: " + name);
    }

    public async virtual Task<IReadOnlyList<FeatureGroupDefinition>> GetGroupsAsync()
    {
        var staticGroupDefinitions = await StaticStore.GetGroupsAsync();
        var dynamicGroupDefinitions = await DynamicStore.GetGroupsAsync();

        return await GetGroupDefinitionsAsync(staticGroupDefinitions, dynamicGroupDefinitions);
    }

    public async virtual Task<FeatureDefinition?> GetOrNullAsync(string name)
    {
        Check.NotNull(name, nameof(name));

        var staticDefinition = await StaticStore.GetOrNullAsync(name);
        var dynamicDefinition = await DynamicStore.GetOrNullAsync(name);

        return await GetDefinitionAsync(staticDefinition, dynamicDefinition);
    }

    protected override string GetDefinitionKey(FeatureDefinition definition)
    {
        return definition.Name;
    }

    protected override string GetGroupDefinitionKey(FeatureGroupDefinition groupDefinition)
    {
        return groupDefinition.Name;
    }

    protected override Task<FeatureDefinition> MergeDefinitionAsync(FeatureDefinition targetDefinition, FeatureDefinition sourceDefinition)
    {
        targetDefinition.DisplayName = sourceDefinition.DisplayName;

        if (sourceDefinition.Description != null)
        {
            targetDefinition.Description = sourceDefinition.Description;
        }

        if (sourceDefinition.DefaultValue != null)
        {
            targetDefinition.DefaultValue = sourceDefinition.DefaultValue;
        }

        if (sourceDefinition.ValueType != null)
        {
            targetDefinition.ValueType = sourceDefinition.ValueType;
        }

        targetDefinition.IsVisibleToClients = targetDefinition.IsVisibleToClients || sourceDefinition.IsVisibleToClients;
        targetDefinition.IsAvailableToHost = targetDefinition.IsAvailableToHost || sourceDefinition.IsAvailableToHost;

        CopyFeatureDetails(targetDefinition, sourceDefinition);

        return Task.FromResult(targetDefinition);
    }

    protected async override Task AfterMergeDefinitionAsync(FeatureDefinition targetDefinition, FeatureDefinition sourceDefinition)
    {
        foreach (var child in sourceDefinition.Children)
        {
            await MergeChildFeature(targetDefinition, child);
        }
    }

    protected async override Task MergeGroupDefinitionAsync(FeatureGroupDefinition targetGroupDefinition, FeatureGroupDefinition sourceGroupDefinition)
    {
        if (sourceGroupDefinition.DisplayName != null)
        {
            targetGroupDefinition.DisplayName = sourceGroupDefinition.DisplayName;
        }
        foreach (var property in sourceGroupDefinition.Properties)
        {
            targetGroupDefinition.Properties[property.Key] = property.Value;
        }

        foreach (var sourceFeature in sourceGroupDefinition.Features)
        {
            var existingFeature = GetFeatureOrNull(targetGroupDefinition, sourceFeature.Name);

            if (existingFeature == null)
            {
                var newFeature = targetGroupDefinition.AddFeature(
                    sourceFeature.Name,
                    sourceFeature.DefaultValue,
                    sourceFeature.DisplayName,
                    sourceFeature.Description,
                    sourceFeature.ValueType,
                    sourceFeature.IsVisibleToClients,
                    sourceFeature.IsAvailableToHost
                );

                CopyFeatureDetails(sourceFeature, newFeature);

                foreach (var child in sourceFeature.Children)
                {
                    AddChildFeatureRecursively(newFeature, child);
                }
            }
            else
            {
                await MergeDefinitionAsync(existingFeature, sourceFeature);

                foreach (var child in sourceFeature.Children)
                {
                    await MergeChildFeature(existingFeature, child);
                }
            }
        }
    }

    private async Task MergeChildFeature(FeatureDefinition parent, FeatureDefinition sourceChild)
    {
        var existingChild = parent.Children.FirstOrDefault(c => c.Name == sourceChild.Name);

        if (existingChild == null)
        {
            var newChild = parent.CreateChild(
                sourceChild.Name,
                sourceChild.DefaultValue,
                sourceChild.DisplayName,
                sourceChild.Description,
                sourceChild.ValueType,
                sourceChild.IsVisibleToClients,
                sourceChild.IsAvailableToHost
            );
            CopyFeatureDetails(sourceChild, newChild);

            foreach (var grandchild in sourceChild.Children)
            {
                AddChildFeatureRecursively(newChild, grandchild);
            }
        }
        else
        {
            await MergeDefinitionAsync(existingChild, sourceChild);

            foreach (var grandchild in sourceChild.Children)
            {
                await MergeChildFeature(existingChild, grandchild);
            }
        }
    }

    private static void AddChildFeatureRecursively(FeatureDefinition parent, FeatureDefinition sourceChild)
    {
        var newChild = parent.CreateChild(
            sourceChild.Name,
            sourceChild.DefaultValue,
            sourceChild.DisplayName,
            sourceChild.Description,
            sourceChild.ValueType,
            sourceChild.IsVisibleToClients,
            sourceChild.IsAvailableToHost
        );

        CopyFeatureDetails(sourceChild, newChild);

        foreach (var grandchild in sourceChild.Children)
        {
            AddChildFeatureRecursively(newChild, grandchild);
        }
    }

    private static void CopyFeatureDetails(FeatureDefinition source, FeatureDefinition target)
    {
        foreach (var property in source.Properties)
        {
            target.Properties[property.Key] = property.Value;
        }

        foreach (var provider in source.AllowedProviders)
        {
            if (!target.AllowedProviders.Contains(provider))
            {
                target.AllowedProviders.Add(provider);
            }
        }
    }

    public static FeatureDefinition? GetFeatureOrNull(
        FeatureGroupDefinition group,
        [NotNull] string name)
    {
        Check.NotNull(name, nameof(name));

        return GetFeatureOrNullRecursively(group.Features, name);
    }

    private static FeatureDefinition? GetFeatureOrNullRecursively(
        IReadOnlyList<FeatureDefinition> features,
        string name)
    {
        foreach (var feature in features)
        {
            if (feature.Name == name)
            {
                return feature;
            }

            var childFeature = GetFeatureOrNullRecursively(feature.Children, name);
            if (childFeature != null)
            {
                return childFeature;
            }
        }

        return null;
    }
}
