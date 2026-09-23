using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities.Events.Distributed;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.Identity;
using Volo.Abp.Uow;
using Volo.Abp.Users;

namespace LY.MicroService.Applications.Single.EventBus.Distributed;

public class IdentityUserChangedEventHandler :
    IDistributedEventHandler<EntityCreatedEto<UserEto>>,
    ITransientDependency
{
    protected IdentityUserManager UserManager { get; }

    public IdentityUserChangedEventHandler(IdentityUserManager userManager)
    {
        UserManager = userManager;
    }

    [UnitOfWork]
    public async virtual Task HandleEventAsync(EntityCreatedEto<UserEto> eventData)
    {
        var user = await UserManager.GetByIdAsync(eventData.Entity.Id);
        await UserManager.AddDefaultRolesAsync(user);
    }
}
