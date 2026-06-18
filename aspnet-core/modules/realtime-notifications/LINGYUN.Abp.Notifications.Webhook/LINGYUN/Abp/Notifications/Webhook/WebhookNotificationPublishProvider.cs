using LINGYUN.Abp.Webhooks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
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

    protected async override Task PublishAsync(NotificationPublishContext context, CancellationToken cancellationToken = default)
    {
        using var scope = ServiceScopeFactory.CreateScope();

        foreach (var contributor in Options.Value.Contributors)
        {
            var webhookNotificationContext = new WebhookNotificationContext(scope.ServiceProvider, context.Notification);

            await contributor.ContributeAsync(webhookNotificationContext);

            if (!webhookNotificationContext.HasResolved())
            {
                Logger.LogWarning("The Webhook notifies the contributor: {0} that the Webhook data for the given notification: {1} cannot be parsed. Skip it.",
                    contributor.Name, context.Notification.Name);
                continue;
            }
            else
            {
                await WebhookPublisher.PublishAsync(
                    webhookNotificationContext.Webhook.WebhookName,
                    webhookNotificationContext.Webhook.Data,
                    context.Notification.TenantId,
                    webhookNotificationContext.Webhook.SendExactSameData,
                    webhookNotificationContext.Webhook.Headers);

                Logger.LogDebug("The webhook: {webhookName} with contributor: {name} has successfully published!", webhookNotificationContext.Webhook.WebhookName, Name);
            }
        }
    }
}
