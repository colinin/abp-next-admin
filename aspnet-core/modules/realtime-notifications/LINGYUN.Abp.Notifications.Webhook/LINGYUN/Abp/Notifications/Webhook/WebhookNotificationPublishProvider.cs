using LINGYUN.Abp.Webhooks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace LINGYUN.Abp.Notifications.Webhook;
public class WebhookNotificationPublishProvider : NotificationPublishProvider
{
    public const string ProviderName = "Webhook";
    public override string Name => ProviderName;

    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly AbpNotificationsWebhookOptions _options;
    private readonly IWebhookPublisher _webhookPublisher;

    public WebhookNotificationPublishProvider(
        IServiceScopeFactory serviceScopeFactory,
        IWebhookPublisher webhookPublisher,
        IOptions<AbpNotificationsWebhookOptions> options)
    {
        _serviceScopeFactory = serviceScopeFactory;
        _webhookPublisher = webhookPublisher;
        _options = options.Value;
    }

    protected override async Task PublishAsync(NotificationInfo notification, IEnumerable<UserIdentifier> identifiers, CancellationToken cancellationToken = default)
    {
        using var scope = _serviceScopeFactory.CreateScope();

        foreach (var contributor in _options.Contributors)
        {
            var context = new WebhookNotificationContext(scope.ServiceProvider, notification);

            await contributor.ContributeAsync(context);

            if (context.HasResolved())
            {
                await _webhookPublisher.PublishAsync(
                    context.Webhook.WebhookName,
                    context.Webhook.Data,
                    notification.TenantId,
                    context.Webhook.SendExactSameData,
                    context.Webhook.Headers);
            }
        }
    }
}
