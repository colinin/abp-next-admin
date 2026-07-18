using LINGYUN.Abp.Dingtalk.Features;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Features;

namespace LINGYUN.Abp.Notifications.Dingtalk;

public class DingtalkNotificationPublishProvider : NotificationPublishProvider
{
    public const string ProviderName = "Dingtalk";
    public override string Name => ProviderName;
    protected IFeatureChecker FeatureChecker => ServiceProvider.LazyGetRequiredService<IFeatureChecker>();

    protected async override Task<bool> CanPublishAsync(NotificationInfo notification, CancellationToken cancellationToken = default)
    {
        if (!await FeatureChecker.IsEnabledAsync(DingtalkFeatureNames.Enable))
        {
            Logger.LogWarning(
                "{name} cannot push messages because the feature {featureName} is not enabled",
                Name,
                DingtalkFeatureNames.Enable);
            return false;
        }
        return true;
    }

    protected override Task PublishAsync(NotificationPublishContext context, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
