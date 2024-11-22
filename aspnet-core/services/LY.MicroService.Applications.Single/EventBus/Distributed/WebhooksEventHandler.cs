using LINGYUN.Abp.Webhooks;
using LINGYUN.Abp.Webhooks.EventBus;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.MultiTenancy;

namespace LY.MicroService.Applications.Single.EventBus.Distributed;

public class WebhooksEventHandler :
    IDistributedEventHandler<WebhooksEventData>,
    ITransientDependency
{
    public IWebhookEventStore WebhookEventStore { get; set; }

    private readonly ICurrentTenant _currentTenant;
    private readonly IBackgroundJobManager _backgroundJobManager;
    private readonly IWebhookSubscriptionManager _webhookSubscriptionManager;

    public WebhooksEventHandler(
        IWebhookSubscriptionManager webhookSubscriptionManager,
        ICurrentTenant currentTenant,
        IBackgroundJobManager backgroundJobManager)
    {
        _currentTenant = currentTenant;
        _backgroundJobManager = backgroundJobManager;
        _webhookSubscriptionManager = webhookSubscriptionManager;

        WebhookEventStore = NullWebhookEventStore.Instance;
    }

    public async virtual Task HandleEventAsync(WebhooksEventData eventData)
    {
        var subscriptions = await _webhookSubscriptionManager
            .GetAllSubscriptionsOfTenantsIfFeaturesGrantedAsync(
                eventData.TenantIds,
                eventData.WebhookName);

        await PublishAsync(eventData.WebhookName, eventData.Data, subscriptions, eventData.SendExactSameData, eventData.Headers);
    }

    protected async virtual Task PublishAsync(
        string webhookName,
        string data,
        List<WebhookSubscriptionInfo> webhookSubscriptions,
        bool sendExactSameData = false,
        WebhookHeader headers = null)
    {
        if (webhookSubscriptions.IsNullOrEmpty())
        {
            return;
        }

        var subscriptionsGroupedByTenant = webhookSubscriptions.GroupBy(x => x.TenantId);

        foreach (var subscriptionGroupedByTenant in subscriptionsGroupedByTenant)
        {
            var webhookInfo = await SaveAndGetWebhookAsync(subscriptionGroupedByTenant.Key, webhookName, data);

            foreach (var webhookSubscription in subscriptionGroupedByTenant)
            {
                var headersToSend = webhookSubscription.Headers;
                if (headers != null)
                {
                    if (headers.UseOnlyGivenHeaders)//do not use the headers defined in subscription
                    {
                        headersToSend = headers.Headers;
                    }
                    else
                    {
                        //use the headers defined in subscription. If additional headers has same header, use additional headers value.
                        foreach (var additionalHeader in headers.Headers)
                        {
                            headersToSend[additionalHeader.Key] = additionalHeader.Value;
                        }
                    }
                }

                await _backgroundJobManager.EnqueueAsync(new WebhookSenderArgs
                {
                    TenantId = webhookSubscription.TenantId,
                    WebhookEventId = webhookInfo.Id,
                    Data = webhookInfo.Data,
                    WebhookName = webhookInfo.WebhookName,
                    WebhookSubscriptionId = webhookSubscription.Id,
                    Headers = headersToSend,
                    Secret = webhookSubscription.Secret,
                    WebhookUri = webhookSubscription.WebhookUri,
                    SendExactSameData = sendExactSameData
                });
            }
        }
    }

    protected async virtual Task<WebhookEvent> SaveAndGetWebhookAsync(
        Guid? tenantId,
        string webhookName,
        string data)
    {
        var webhookInfo = new WebhookEvent
        {
            WebhookName = webhookName,
            Data = data,
            TenantId = tenantId
        };

        var webhookId = await WebhookEventStore.InsertAndGetIdAsync(webhookInfo);
        webhookInfo.Id = webhookId;

        return webhookInfo;
    }
}
