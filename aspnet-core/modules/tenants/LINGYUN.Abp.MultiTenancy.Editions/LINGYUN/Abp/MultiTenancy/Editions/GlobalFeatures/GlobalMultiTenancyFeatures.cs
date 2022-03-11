using JetBrains.Annotations;
using Volo.Abp.GlobalFeatures;

namespace LINGYUN.Abp.MultiTenancy.Editions.GlobalFeatures;

public class GlobalMultiTenancyFeatures : GlobalModuleFeatures
{
    public const string ModuleName = "Abp.MultiTenancy";

    public EditionsFeature Editions => GetFeature<EditionsFeature>();

    public GlobalMultiTenancyFeatures([NotNull] GlobalFeatureManager featureManager)
        : base(featureManager)
    {
        AddFeature(new EditionsFeature(this));
    }
}
