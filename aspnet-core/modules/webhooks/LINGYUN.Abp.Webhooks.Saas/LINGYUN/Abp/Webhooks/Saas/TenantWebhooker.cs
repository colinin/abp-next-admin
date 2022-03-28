using LINGYUN.Abp.Saas.Tenants;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities.Events.Distributed;
using Volo.Abp.EventBus.Distributed;

namespace LINGYUN.Abp.Webhooks.Saas;

public class TenantWebhooker :
    IDistributedEventHandler<EntityCreatedEto<TenantEto>>,
    IDistributedEventHandler<EntityUpdatedEto<TenantEto>>,
    IDistributedEventHandler<EntityDeletedEto<TenantEto>>,
    ITransientDependency
{
    private readonly IWebhookPublisher _webhookPublisher;

    public TenantWebhooker(
        IWebhookPublisher webhookPublisher)
    {
        _webhookPublisher = webhookPublisher;
    }

    public async virtual Task HandleEventAsync(EntityCreatedEto<TenantEto> eventData)
    {
        await PublishAsync(SaasWebhookNames.Tenant.Create, eventData.Entity);
    }

    public async virtual Task HandleEventAsync(EntityUpdatedEto<TenantEto> eventData)
    {
        await PublishAsync(SaasWebhookNames.Tenant.Update, eventData.Entity);
    }

    public async virtual Task HandleEventAsync(EntityDeletedEto<TenantEto> eventData)
    {
        await PublishAsync(SaasWebhookNames.Tenant.Delete, eventData.Entity);
    }

    protected async virtual Task PublishAsync(string webhookName, TenantEto eto)
    {
        await _webhookPublisher.PublishAsync(
            webhookName,
            new TenantWto
            {
                Id = eto.Id,
                Name = eto.Name
            });
    }
}
