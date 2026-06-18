using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.DistributedLocking;
using Volo.Abp.Domain.Entities.Events.Distributed;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.Uow;

namespace LINGYUN.Abp.Identity.Session;

public class IdentitySessionUserInactiveSynchronizer :
    IDistributedEventHandler<EntityCreatedEto<IdentitySessionEto>>,
    ITransientDependency
{
    private readonly IAbpDistributedLock _distributedLock;
    private readonly IIdentityUserInactiveRepository _identityUserInactiveRepository;

    public IdentitySessionUserInactiveSynchronizer(
        IAbpDistributedLock distributedLock,
        IIdentityUserInactiveRepository identityUserInactiveRepository)
    {
        _distributedLock = distributedLock;
        _identityUserInactiveRepository = identityUserInactiveRepository;
    }

    [UnitOfWork]
    public async virtual Task HandleEventAsync(EntityCreatedEto<IdentitySessionEto> eventData)
    {
        await using var lockHandle = await _distributedLock.TryAcquireAsync(
            $"{nameof(IdentitySessionUserInactiveSynchronizer)}_{nameof(IdentitySessionEto)}");
        if (lockHandle != null)
        {
            var userInactive = await _identityUserInactiveRepository.FindByUserIdAsync(eventData.Entity.UserId);
            if (userInactive != null)
            {
                await _identityUserInactiveRepository.DeleteAsync(userInactive);
            }
        }
    }
}
