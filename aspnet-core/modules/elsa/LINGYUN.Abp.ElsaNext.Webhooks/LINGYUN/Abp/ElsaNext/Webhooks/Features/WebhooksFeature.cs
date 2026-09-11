using Elsa.Extensions;
using Elsa.Features.Abstractions;
using Elsa.Features.Services;

namespace LINGYUN.Abp.ElsaNext.Webhooks.Features;

public class WebhooksFeature : FeatureBase
{
    public WebhooksFeature(IModule module) : base(module)
    {
    }

    public override void Configure()
    {
        Module.AddActivitiesFrom<WebhooksFeature>();
    }
}
