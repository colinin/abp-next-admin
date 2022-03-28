using LINGYUN.Abp.Saas.Editions;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities.Events.Distributed;
using Volo.Abp.EventBus.Distributed;

namespace LINGYUN.Abp.Webhooks.Saas;

public class EditionWebhooker :
    IDistributedEventHandler<EntityCreatedEto<EditionEto>>,
    IDistributedEventHandler<EntityUpdatedEto<EditionEto>>,
    IDistributedEventHandler<EntityDeletedEto<EditionEto>>,
    ITransientDependency
{
    private readonly IWebhookPublisher _webhookPublisher;

    public EditionWebhooker(
        IWebhookPublisher webhookPublisher)
    {
        _webhookPublisher = webhookPublisher;
    }

    public async virtual Task HandleEventAsync(EntityCreatedEto<EditionEto> eventData)
    {
        await PublishAsync(SaasWebhookNames.Edition.Create, eventData.Entity);
    }

    public async virtual Task HandleEventAsync(EntityUpdatedEto<EditionEto> eventData)
    {
        await PublishAsync(SaasWebhookNames.Edition.Update, eventData.Entity);
    }

    public async virtual Task HandleEventAsync(EntityDeletedEto<EditionEto> eventData)
    {
        await PublishAsync(SaasWebhookNames.Edition.Delete, eventData.Entity);
    }

    protected async virtual Task PublishAsync(string webhookName, EditionEto eto)
    {
        await _webhookPublisher.PublishAsync(
            webhookName,
            new EditionWto
            {
                Id = eto.Id,
                DisplayName = eto.DisplayName
            });
    }
}
