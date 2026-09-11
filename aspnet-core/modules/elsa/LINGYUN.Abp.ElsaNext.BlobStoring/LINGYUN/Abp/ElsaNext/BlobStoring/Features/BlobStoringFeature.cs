using Elsa.Extensions;
using Elsa.Features.Abstractions;
using Elsa.Features.Services;

namespace LINGYUN.Abp.ElsaNext.BlobStoring.Features;

public class BlobStoringFeature : FeatureBase
{
    public BlobStoringFeature(IModule module) : base(module)
    {
    }

    public override void Configure()
    {
        Module.AddActivitiesFrom<BlobStoringFeature>();
    }
}
