using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities.Events.Distributed;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.Identity;

namespace LINGYUN.Abp.Webhooks.Identity;

public class IdentityRoleWebhooker :
    IDistributedEventHandler<EntityCreatedEto<IdentityRoleEto>>,
    IDistributedEventHandler<EntityUpdatedEto<IdentityRoleEto>>,
    IDistributedEventHandler<EntityDeletedEto<IdentityRoleEto>>,
    IDistributedEventHandler<IdentityRoleNameChangedEto>,
    ITransientDependency
{
    private readonly IWebhookPublisher _webhookPublisher;

    public IdentityRoleWebhooker(
        IWebhookPublisher webhookPublisher)
    {
        _webhookPublisher = webhookPublisher;
    }

    public async virtual Task HandleEventAsync(EntityCreatedEto<IdentityRoleEto> eventData)
    {
        await PublishAsync(IdentityWebhookNames.IdentityRole.Create, eventData.Entity);
    }

    public async virtual Task HandleEventAsync(EntityUpdatedEto<IdentityRoleEto> eventData)
    {
        await PublishAsync(IdentityWebhookNames.IdentityRole.Update, eventData.Entity);
    }

    public async virtual Task HandleEventAsync(EntityDeletedEto<IdentityRoleEto> eventData)
    {
        await PublishAsync(IdentityWebhookNames.IdentityRole.Delete, eventData.Entity);
    }

    public async virtual Task HandleEventAsync(IdentityRoleNameChangedEto eventData)
    {
        await _webhookPublisher.PublishAsync(
            IdentityWebhookNames.IdentityRole.ChangeName,
            new IdentityRoleNameChangedWto
            {
                Id = eventData.Id,
                Name = eventData.Name,
                OldName = eventData.OldName,
            },
            eventData.TenantId);
    }

    protected async virtual Task PublishAsync(string webhookName, IdentityRoleEto eto)
    {
        await _webhookPublisher.PublishAsync(
            webhookName,
            new IdentityRoleWto
            {
                Id = eto.Id,
                Name = eto.Name
            },
            eto.TenantId);
    }
}
