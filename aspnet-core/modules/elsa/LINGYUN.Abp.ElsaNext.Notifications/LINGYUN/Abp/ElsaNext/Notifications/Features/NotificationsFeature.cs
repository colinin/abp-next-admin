using Elsa.Extensions;
using Elsa.Features.Abstractions;
using Elsa.Features.Services;

namespace LINGYUN.Abp.ElsaNext.Notifications.Features;

public class NotificationsFeature : FeatureBase
{
    public NotificationsFeature(IModule module) : base(module)
    {
    }

    public override void Configure()
    {
        Module.AddActivitiesFrom<NotificationsFeature>();
    }
}
