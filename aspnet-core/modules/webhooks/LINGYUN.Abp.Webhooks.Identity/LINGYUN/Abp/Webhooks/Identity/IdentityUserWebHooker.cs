using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities.Events.Distributed;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.Users;

namespace LINGYUN.Abp.Webhooks.Identity;

public class IdentityUserWebhooker :
    IDistributedEventHandler<EntityCreatedEto<UserEto>>,
    IDistributedEventHandler<EntityUpdatedEto<UserEto>>,
    IDistributedEventHandler<EntityDeletedEto<UserEto>>,
    ITransientDependency
{
    private readonly IWebhookPublisher _webhookPublisher;

    public IdentityUserWebhooker(
        IWebhookPublisher webhookPublisher)
    {
        _webhookPublisher = webhookPublisher;
    }

    public async virtual Task HandleEventAsync(EntityCreatedEto<UserEto> eventData)
    {
        await PublishAsync(IdentityWebhookNames.IdentityUser.Create, eventData.Entity);
    }

    public async virtual Task HandleEventAsync(EntityUpdatedEto<UserEto> eventData)
    {
        await PublishAsync(IdentityWebhookNames.IdentityUser.Update, eventData.Entity);
    }

    public async virtual Task HandleEventAsync(EntityDeletedEto<UserEto> eventData)
    {
        await PublishAsync(IdentityWebhookNames.IdentityUser.Delete, eventData.Entity);
    }

    protected async virtual Task PublishAsync(string webhookName, UserEto eto)
    {
        await _webhookPublisher.PublishAsync(
            webhookName,
            new IdentityUserWto
            {
                Id = eto.Id,
                Name = eto.Name,
                Email = eto.Email,
                EmailConfirmed  = eto.EmailConfirmed,
                UserName = eto.UserName,
                PhoneNumber = eto.PhoneNumber,
                PhoneNumberConfirmed = eto.PhoneNumberConfirmed,
                Surname = eto.Surname,
            },
            eto.TenantId);
    }
}
