using LINGYUN.Abp.TenantManagement;
using System;
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
    IDistributedEventHandler<ConnectionStringCreatedEventData>,
    IDistributedEventHandler<ConnectionStringDeletedEventData>,
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
        await UpdateCacheItemAsync(eventData.Entity.Id, eventData.Entity.Name);
    }

    public virtual async Task HandleEventAsync(EntityCreatedEto<TenantEto> eventData)
    {
        await UpdateCacheItemAsync(eventData.Entity.Id, eventData.Entity.Name);
    }

    public virtual async Task HandleEventAsync(EntityDeletedEto<TenantEto> eventData)
    {
        await RemoveCacheItemAsync(eventData.Entity.Id, eventData.Entity.Name);
    }

    public virtual async Task HandleEventAsync(ConnectionStringCreatedEventData eventData)
    {
        await UpdateCacheItemAsync(eventData.TenantId, eventData.TenantName);
    }

    public virtual async Task HandleEventAsync(ConnectionStringDeletedEventData eventData)
    {
        await RemoveCacheItemAsync(eventData.TenantId, eventData.TenantName);
    }

    protected virtual async Task UpdateCacheItemAsync(Guid tenantId, string tenantName = null)
    {
        using (_currentTenant.Change(null))
        {
            var tenantDto = await _tenantAppService.GetAsync(tenantId);
            var tenantConnectionStringsDto = await _tenantAppService.GetConnectionStringAsync(tenantId);
            var connectionStrings = new ConnectionStrings();
            foreach (var tenantConnectionString in tenantConnectionStringsDto.Items)
            {
                connectionStrings[tenantConnectionString.Name] = tenantConnectionString.Value;
            }
            var cacheItem = new TenantConfigurationCacheItem(tenantDto.Id, tenantDto.Name, connectionStrings);

            await _cache.SetAsync(
                 TenantConfigurationCacheItem.CalculateCacheKey(tenantDto.Id.ToString()),
                 cacheItem);

            await _cache.SetAsync(
                TenantConfigurationCacheItem.CalculateCacheKey(tenantDto.Name),
                cacheItem);
        }
    }

    protected virtual async Task RemoveCacheItemAsync(Guid tenantId, string tenantName = null)
    {
        using (_currentTenant.Change(null))
        {
            await _cache.RemoveAsync(TenantConfigurationCacheItem.CalculateCacheKey(tenantId.ToString()));
            if (!tenantName.IsNullOrWhiteSpace())
            {
                await _cache.RemoveAsync(TenantConfigurationCacheItem.CalculateCacheKey(tenantName));
            }
        }
    }
}
