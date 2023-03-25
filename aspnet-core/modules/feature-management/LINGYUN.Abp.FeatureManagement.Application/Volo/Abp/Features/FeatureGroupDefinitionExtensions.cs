using JetBrains.Annotations;
using System.Collections.Generic;

namespace Volo.Abp.Features;
public static class FeatureGroupDefinitionExtensions
{
    public static FeatureDefinition GetFeatureOrNull(
        this FeatureGroupDefinition group,
        [NotNull] string name)
    {
        Check.NotNull(name, nameof(name));

        return GetFeatureOrNullRecursively(group.Features, name);
    }

    private static FeatureDefinition GetFeatureOrNullRecursively(
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
