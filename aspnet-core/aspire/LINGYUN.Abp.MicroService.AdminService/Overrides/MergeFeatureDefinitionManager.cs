using System.Collections.Immutable;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Features;

namespace LINGYUN.Abp.MicroService.AdminService.Overrides;

[Dependency(ReplaceServices = true)]
public class MergeFeatureDefinitionManager : FeatureDefinitionManager
{
    public MergeFeatureDefinitionManager(
        IStaticFeatureDefinitionStore staticStore,
        IDynamicFeatureDefinitionStore dynamicStore)
        : base(staticStore, dynamicStore)
    {
    }

    public async override Task<IReadOnlyList<FeatureDefinition>> GetAllAsync()
    {
        var staticFeatures = await StaticStore.GetFeaturesAsync();
        var dynamicFeatures = await DynamicStore.GetFeaturesAsync();

        var mergedFeatures = new Dictionary<string, FeatureDefinition>();

        foreach (var staticFeature in staticFeatures)
        {
            mergedFeatures[staticFeature.Name] = staticFeature;
        }

        foreach (var dynamicFeature in dynamicFeatures)
        {
            if (mergedFeatures.TryGetValue(dynamicFeature.Name, out var existingFeature))
            {
                MergeFeatureMetadata(existingFeature, dynamicFeature);

                foreach (var child in dynamicFeature.Children)
                {
                    MergeChildFeature(existingFeature, child);
                }
            }
            else
            {
                mergedFeatures[dynamicFeature.Name] = dynamicFeature;
            }
        }

        return mergedFeatures.Values.ToImmutableList();
    }

    public async override Task<IReadOnlyList<FeatureGroupDefinition>> GetGroupsAsync()
    {
        var staticGroups = await StaticStore.GetGroupsAsync();
        var dynamicGroups = await DynamicStore.GetGroupsAsync();

        var mergedGroups = new Dictionary<string, FeatureGroupDefinition>();

        foreach (var staticGroup in staticGroups)
        {
            mergedGroups[staticGroup.Name] = staticGroup;
        }

        foreach (var dynamicGroup in dynamicGroups)
        {
            if (mergedGroups.TryGetValue(dynamicGroup.Name, out var existingGroup))
            {
                MergeGroupFeatures(existingGroup, dynamicGroup);
            }
            else
            {
                mergedGroups[dynamicGroup.Name] = dynamicGroup;
            }
        }

        return mergedGroups.Values.ToImmutableList();
    }

    private static void MergeGroupFeatures(FeatureGroupDefinition target, FeatureGroupDefinition source)
    {
        foreach (var sourceFeature in source.Features)
        {
            var existingFeature = target.GetFeatureOrNull(sourceFeature.Name);

            if (existingFeature == null)
            {
                var newFeature = target.AddFeature(
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
                MergeFeatureMetadata(existingFeature, sourceFeature);

                foreach (var child in sourceFeature.Children)
                {
                    MergeChildFeature(existingFeature, child);
                }
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

    private static void MergeChildFeature(FeatureDefinition parent, FeatureDefinition sourceChild)
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
            MergeFeatureMetadata(existingChild, sourceChild);

            foreach (var grandchild in sourceChild.Children)
            {
                MergeChildFeature(existingChild, grandchild);
            }
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

    private static void MergeFeatureMetadata(FeatureDefinition target, FeatureDefinition source)
    {
        if (source.DisplayName != null)
        {
            target.DisplayName = source.DisplayName;
        }

        if (source.Description != null)
        {
            target.Description = source.Description;
        }

        if (source.DefaultValue != null)
        {
            target.DefaultValue = source.DefaultValue;
        }

        if (source.ValueType != null)
        {
            target.ValueType = source.ValueType;
        }

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

        target.IsVisibleToClients = target.IsVisibleToClients || source.IsVisibleToClients;
        target.IsAvailableToHost = target.IsAvailableToHost || source.IsAvailableToHost;
    }
}
