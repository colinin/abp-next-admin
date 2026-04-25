using LINGYUN.Abp.BlobManagement;
using LINGYUN.Abp.BlobManagement.Permissions;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities.Events;
using Volo.Abp.EventBus;
using Volo.Abp.PermissionManagement;
using Volo.Abp.Users;

namespace LINGYUN.Abp.MicroService.PlatformService.Handlers;

public class PrivateBlobProtecteder :
    ILocalEventHandler<EntityCreatedEventData<Blob>>,
    ILocalEventHandler<EntityDeletedEventData<Blob>>,
    ITransientDependency
{
    private readonly IResourcePermissionManager _resourcePermissionManager;
    private readonly IBlobContainerRepository _blobContainerRepository;
    private readonly ICurrentUser _currentUser;

    public PrivateBlobProtecteder(
        IResourcePermissionManager resourcePermissionManager,
        IBlobContainerRepository blobContainerRepository,
        ICurrentUser currentUser)
    {
        _resourcePermissionManager = resourcePermissionManager;
        _blobContainerRepository = blobContainerRepository;
        _currentUser = currentUser;
    }

    public async virtual Task HandleEventAsync(EntityCreatedEventData<Blob> eventData)
    {
        if (await ShouldSetResourcePermissions(eventData.Entity))
        {
            await SetResourcePermissions(eventData.Entity, true);
        }
    }

    public async virtual Task HandleEventAsync(EntityDeletedEventData<Blob> eventData)
    {
        if (await ShouldSetResourcePermissions(eventData.Entity))
        {
            await SetResourcePermissions(eventData.Entity, false);
        }
    }

    protected async virtual Task<bool> ShouldSetResourcePermissions(Blob blob)
    {
        if (!_currentUser.IsAuthenticated)
        {
            return false;
        }
        var blobContainer = await _blobContainerRepository.GetAsync(blob.ContainerId);
        if (!string.Equals("users", blobContainer.Name, StringComparison.InvariantCultureIgnoreCase))
        {
            return false;
        }

        return true;
    }

    protected async virtual Task SetResourcePermissions(Blob blob, bool isGranted)
    {
        await _resourcePermissionManager.SetForUserAsync(
            userId: _currentUser.Id!.Value,
            name: BlobManagementPermissionNames.Blob.Resources.Delete,
            resourceName: BlobManagementPermissionNames.Blob.Resources.Name,
            resourceKey: blob.Id.ToString(),
            isGranted: isGranted);
        await _resourcePermissionManager.SetForUserAsync(
            userId: _currentUser.Id!.Value,
            name: BlobManagementPermissionNames.Blob.Resources.View,
            resourceName: BlobManagementPermissionNames.Blob.Resources.Name,
            resourceKey: blob.Id.ToString(),
            isGranted: isGranted);
    }
}
