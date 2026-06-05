namespace LINGYUN.Abp.Identity.Session;

public class IdentitySessionUserInactiveSynchronizer :
    IDistributedEventHandler<EntityCreatedEto<IdentitySessionEto>>,
    ITransientDependency
{
    private readonly IIdentityUserInactiveRepository _identityUserInactiveRepository;

    public IdentitySessionUserInactiveSynchronizer(IIdentityUserInactiveRepository identityUserInactiveRepository)
    {
        _identityUserInactiveRepository = identityUserInactiveRepository;
    }

    [UnitOfWork]
    public async virtual Task HandleEventAsync(EntityCreatedEto<IdentitySessionEto> eventData)
    {
        var userInactive = await _identityUserInactiveRepository.FindByUserIdAsync(eventData.Entity.UserId);
        if (userInactive != null)
        {
            await _identityUserInactiveRepository.DeleteAsync(userInactive);
        }
    }
}
