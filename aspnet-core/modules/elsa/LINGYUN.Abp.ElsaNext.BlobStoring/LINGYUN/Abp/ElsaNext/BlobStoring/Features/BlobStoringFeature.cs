using Elsa.Common.Serialization;
using Elsa.Extensions;
using Elsa.Features.Abstractions;
using Elsa.Features.Services;
using LINGYUN.Abp.ElsaNext.BlobStoring.Models;
using Microsoft.Extensions.DependencyInjection;

namespace LINGYUN.Abp.ElsaNext.BlobStoring.Features;

public class BlobStoringFeature : FeatureBase
{
    public BlobStoringFeature(IModule module) : base(module)
    {
    }

    public override void Configure()
    {
        Module.UseWorkflowManagement(management =>
        {
            management.AddVariableTypes([
                typeof(BlobContent)
            ], "BlobStoring");

            management.AddActivitiesFrom<BlobStoringFeature>();
        });
    }

    public override void Apply()
    {
        Services.Configure<SerializationTypeOptions>(options =>
        {
            options.RegisterTypeAlias(typeof(BlobContent), "BlobContent");
        });
    }
}
