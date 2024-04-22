using JetBrains.Annotations;
using Volo.Abp.GlobalFeatures;

namespace LINGYUN.Abp.MultiTenancy.Editions.GlobalFeatures;

[GlobalFeatureName(Name)]
public class EditionsFeature : GlobalFeature
{
    public const string Name = "Abp.Editions";

    internal EditionsFeature([NotNull] GlobalEditionsFeatures module)
        : base(module)
    {
    }
}
