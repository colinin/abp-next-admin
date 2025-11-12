using LINGYUN.Abp.Webhooks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Features;

namespace LINGYUN.Abp.Notifications.Webhook;
public class WebhookNotificationPublishProvider : NotificationPublishProvider
{
    public const string ProviderName = "Webhook";
    public override string Name => ProviderName;

    protected IFeatureChecker FeatureChecker => ServiceProvider.LazyGetRequiredService<IFeatureChecker>();
    protected IWebhookPublisher WebhookPublisher => ServiceProvider.LazyGetRequiredService<IWebhookPublisher>();
    protected IServiceScopeFactory ServiceScopeFactory => ServiceProvider.LazyGetRequiredService<IServiceScopeFactory>();
    protected IOptions<AbpNotificationsWebhookOptions> Options => ServiceProvider.LazyGetRequiredService<IOptions<AbpNotificationsWebhookOptions>>();

    protected override Task<bool> CanPublishAsync(NotificationInfo notification, CancellationToken cancellationToken = default)
    {
        if (Options.Value.Contributors.Count == 0)
        {
            Logger.LogWarning("The Webhook notification publishing contributor is empty, and the Webhook notification cannot be sent!");
            return Task.FromResult(false);
        }

        return base.CanPublishAsync(notification, cancellationToken);
    }

    protected async override Task PublishAsync(NotificationInfo notification, IEnumerable<UserIdentifier> identifiers, CancellationToken cancellationToken = default)
    {
        using var scope = ServiceScopeFactory.CreateScope();

        foreach (var contributor in Options.Value.Contributors)
        {
            var context = new WebhookNotificationContext(scope.ServiceProvider, notification);

            await contributor.ContributeAsync(context);

            if (!context.HasResolved())
            {
                Logger.LogWarning("The Webhook notifies the contributor: {0} that the Webhook data for the given notification: {1} cannot be parsed. Skip it.",
                    contributor.Name, notification.Name);
                continue;
            }
            else
            {
                await WebhookPublisher.PublishAsync(
                    context.Webhook.WebhookName,
                    context.Webhook.Data,
                    notification.TenantId,
                    context.Webhook.SendExactSameData,
                    context.Webhook.Headers);

                Logger.LogDebug("The webhook: {webhookName} with contributor: {name} has successfully published!", context.Webhook.WebhookName, Name);
            }
        }
    }
}
