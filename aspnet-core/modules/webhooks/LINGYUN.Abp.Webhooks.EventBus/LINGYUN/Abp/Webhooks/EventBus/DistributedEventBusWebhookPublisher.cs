using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus.Distributed;

namespace LINGYUN.Abp.Webhooks.EventBus;

[Dependency(ReplaceServices = true)]
public class DistributedEventBusWebhookPublisher : IWebhookPublisher, ITransientDependency
{
    protected IDistributedEventBus EventBus { get; }

    public DistributedEventBusWebhookPublisher(IDistributedEventBus eventBus)
    {
        EventBus = eventBus;
    }

    public async virtual Task PublishAsync(
        string webhookName, 
        object data, 
        bool sendExactSameData = false, 
        WebhookHeader headers = null)
    {
        var eventData = new WebhooksEventData(
            webhookName,
            JsonConvert.SerializeObject(data),
            sendExactSameData,
            headers);

        await PublishAsync(eventData);
    }

    public async virtual Task PublishAsync(
        string webhookName, 
        object data, 
        Guid? tenantId, 
        bool sendExactSameData = false, 
        WebhookHeader headers = null)
    {
        var eventData = new WebhooksEventData(
            webhookName,
            JsonConvert.SerializeObject(data),
            sendExactSameData,
            headers,
            new Guid?[] { tenantId });

        await PublishAsync(eventData);
    }

    public async virtual Task PublishAsync(
        Guid?[] tenantIds, 
        string webhookName, 
        object data, 
        bool sendExactSameData = false, 
        WebhookHeader headers = null)
    {
        var eventData = new WebhooksEventData(
            webhookName,
            JsonConvert.SerializeObject(data),
            sendExactSameData,
            headers,
            tenantIds);

        await PublishAsync(eventData);
    }

    protected async virtual Task PublishAsync(WebhooksEventData eventData)
    {
        await EventBus.PublishAsync(eventData);
    }
}
