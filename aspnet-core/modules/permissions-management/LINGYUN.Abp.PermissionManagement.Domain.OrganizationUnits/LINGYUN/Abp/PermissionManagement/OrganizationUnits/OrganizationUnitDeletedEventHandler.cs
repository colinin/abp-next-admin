using LINGYUN.Abp.Authorization.Permissions;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities.Events.Distributed;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.Identity;
using Volo.Abp.PermissionManagement;

namespace LINGYUN.Abp.PermissionManagement.OrganizationUnits;

public class OrganizationUnitDeletedEventHandler :
    IDistributedEventHandler<EntityDeletedEto<OrganizationUnitEto>>,
    ITransientDependency
{
    protected IPermissionManager PermissionManager { get; }

    public OrganizationUnitDeletedEventHandler(IPermissionManager permissionManager)
    {
        PermissionManager = permissionManager;
    }

    public async Task HandleEventAsync(EntityDeletedEto<OrganizationUnitEto> eventData)
    {
        await PermissionManager.DeleteAsync(OrganizationUnitPermissionValueProvider.ProviderName, eventData.Entity.Id.ToString());
    }
}
