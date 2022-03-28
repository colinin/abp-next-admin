using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities.Events.Distributed;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.Identity;

namespace LINGYUN.Abp.Webhooks.Identity;

public class OrganizationUnitWebhooker :
    IDistributedEventHandler<EntityCreatedEto<OrganizationUnitEto>>,
    IDistributedEventHandler<EntityUpdatedEto<OrganizationUnitEto>>,
    IDistributedEventHandler<EntityDeletedEto<OrganizationUnitEto>>,
    ITransientDependency
{
    private readonly IWebhookPublisher _webhookPublisher;

    public OrganizationUnitWebhooker(
        IWebhookPublisher webhookPublisher)
    {
        _webhookPublisher = webhookPublisher;
    }

    public async virtual Task HandleEventAsync(EntityCreatedEto<OrganizationUnitEto> eventData)
    {
        await PublishAsync(IdentityWebhookNames.IdentityRole.Create, eventData.Entity);
    }

    public async virtual Task HandleEventAsync(EntityUpdatedEto<OrganizationUnitEto> eventData)
    {
        await PublishAsync(IdentityWebhookNames.IdentityRole.Update, eventData.Entity);
    }

    public async virtual Task HandleEventAsync(EntityDeletedEto<OrganizationUnitEto> eventData)
    {
        await PublishAsync(IdentityWebhookNames.IdentityRole.Delete, eventData.Entity);
    }

    protected async virtual Task PublishAsync(string webhookName, OrganizationUnitEto eto)
    {
        await _webhookPublisher.PublishAsync(
            webhookName,
            new OrganizationUnitWto
            {
                Id = eto.Id,
                DisplayName = eto.DisplayName
            },
            eto.TenantId);
    }
}
