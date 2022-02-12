using LINGYUN.Abp.TenantManagement;
using System.Threading.Tasks;
using Volo.Abp.Caching;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities.Events.Distributed;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.MultiTenancy;
using Volo.Abp.TenantManagement;

namespace LINGYUN.Abp.MultiTenancy.RemoteService.EventBus.Distributed;

public class TenantSynchronizer :
    IDistributedEventHandler<EntityCreatedEto<TenantEto>>,
    IDistributedEventHandler<EntityUpdatedEto<TenantEto>>,
    IDistributedEventHandler<EntityDeletedEto<TenantEto>>,
    ITransientDependency
{
    private readonly ICurrentTenant _currentTenant;
    private readonly ITenantAppService _tenantAppService;
    private readonly IDistributedCache<TenantConfigurationCacheItem> _cache;

    public TenantSynchronizer(
        ICurrentTenant currentTenant,
        ITenantAppService tenantAppService,
        IDistributedCache<TenantConfigurationCacheItem> cache)
    {
        _cache = cache;
        _currentTenant = currentTenant;
        _tenantAppService = tenantAppService;
    }

    public virtual async Task HandleEventAsync(EntityUpdatedEto<TenantEto> eventData)
    {
        await UpdateCacheItemAsync(eventData.Entity);
    }

    public virtual async Task HandleEventAsync(EntityCreatedEto<TenantEto> eventData)
    {
        await UpdateCacheItemAsync(eventData.Entity);
    }

    public virtual async Task HandleEventAsync(EntityDeletedEto<TenantEto> eventData)
    {
        using (_currentTenant.Change(null))
        {
            await _cache.RemoveManyAsync(
                 new string[]
                 {
                    TenantConfigurationCacheItem.CalculateCacheKey(eventData.Entity.Name),
                    TenantConfigurationCacheItem.CalculateCacheKey(eventData.Entity.Id.ToString()),
                 });
        }
    }

    protected virtual async Task UpdateCacheItemAsync(TenantEto tenant)
    {
        using (_currentTenant.Change(null))
        {
            var tenantDto = await _tenantAppService.GetAsync(tenant.Id);
            var tenantConnectionStringsDto = await _tenantAppService.GetConnectionStringAsync(tenant.Id);
            var connectionStrings = new ConnectionStrings();
            foreach (var tenantConnectionString in tenantConnectionStringsDto.Items)
            {
                connectionStrings[tenantConnectionString.Name] = tenantConnectionString.Value;
            }
            var cacheItem = new TenantConfigurationCacheItem(tenantDto.Id, tenantDto.Name, connectionStrings);

            await _cache.SetAsync(
                 TenantConfigurationCacheItem.CalculateCacheKey(tenant.Id.ToString()),
                 cacheItem);

            await _cache.SetAsync(
                TenantConfigurationCacheItem.CalculateCacheKey(tenant.Name),
                cacheItem);
        }
    }
}
