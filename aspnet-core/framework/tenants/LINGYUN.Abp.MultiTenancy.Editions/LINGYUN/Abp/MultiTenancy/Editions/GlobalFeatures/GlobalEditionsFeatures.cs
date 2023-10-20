using JetBrains.Annotations;
using Volo.Abp.GlobalFeatures;

namespace LINGYUN.Abp.MultiTenancy.Editions.GlobalFeatures;

public class GlobalEditionsFeatures : GlobalModuleFeatures
{
    public const string ModuleName = "Abp.Editions";

    public EditionsFeature Editions => GetFeature<EditionsFeature>();

    public GlobalEditionsFeatures([NotNull] GlobalFeatureManager featureManager)
        : base(featureManager)
    {
        AddFeature(new EditionsFeature(this));
    }
}
