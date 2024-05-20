using LINGYUN.Abp.Authorization.Permissions;
using LINGYUN.Abp.DataProtection;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities.Events;
using Volo.Abp.EventBus;

namespace LINGYUN.Abp.DataProtectionManagement;
public class DataProtectedResourceCacheItemInvalidator :
    ILocalEventHandler<EntityChangedEventData<RoleEntityRule>>,
    ILocalEventHandler<EntityChangedEventData<OrganizationUnitEntityRule>>,
    ITransientDependency
{
    private readonly IDataProtectedResourceCache _resourceCache;

    public DataProtectedResourceCacheItemInvalidator(IDataProtectedResourceCache resourceCache)
    {
        _resourceCache = resourceCache;
    }

    public virtual Task HandleEventAsync(EntityChangedEventData<RoleEntityRule> eventData)
    {
        var dataResource = new DataAccessResource(
            RolePermissionValueProvider.ProviderName,
            eventData.Entity.RoleName,
            eventData.Entity.EntityTypeFullName,
            eventData.Entity.Operation,
            eventData.Entity.FilterGroup)
        {
            AllowProperties = eventData.Entity.AllowProperties?.Split(",").ToList(),
        };

        _resourceCache.SetCache(dataResource);

        return Task.CompletedTask;
    }

    public virtual Task HandleEventAsync(EntityChangedEventData<OrganizationUnitEntityRule> eventData)
    {
        var dataResource = new DataAccessResource(
            OrganizationUnitPermissionValueProvider.ProviderName,
            eventData.Entity.OrgCode,
            eventData.Entity.EntityTypeFullName,
            eventData.Entity.Operation,
            eventData.Entity.FilterGroup)
        {
            AllowProperties = eventData.Entity.AllowProperties?.Split(",").ToList(),
        };

        _resourceCache.SetCache(dataResource);

        return Task.CompletedTask;
    }
}
