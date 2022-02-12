using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Volo.Abp.Caching;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities.Events.Distributed;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.MultiTenancy;
using Volo.Abp.TenantManagement;
using Volo.Abp.Uow;

namespace LINGYUN.Abp.MultiTenancy.DbFinder.EventBus.Distributed;

public class TenantSynchronizer :
    IDistributedEventHandler<EntityCreatedEto<TenantEto>>,
    IDistributedEventHandler<EntityUpdatedEto<TenantEto>>,
    IDistributedEventHandler<EntityDeletedEto<TenantEto>>,
    ITransientDependency
{
    private readonly ILogger<TenantSynchronizer> _logger;
    private readonly ICurrentTenant _currentTenant;
    private readonly ITenantRepository _tenantRepository;
    private readonly IDistributedCache<TenantConfigurationCacheItem> _cache;

    public TenantSynchronizer(
        ICurrentTenant currentTenant,
        ITenantRepository tenantRepository,
        ILogger<TenantSynchronizer> logger,
        IDistributedCache<TenantConfigurationCacheItem> cache)
    {
        _cache = cache;
        _logger = logger;
        _currentTenant = currentTenant;
        _tenantRepository = tenantRepository;
    }

    [UnitOfWork]
    public virtual async Task HandleEventAsync(EntityUpdatedEto<TenantEto> eventData)
    {
        await UpdateCacheItemAsync(eventData.Entity);
    }

    [UnitOfWork]
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
        try
        {
            using (_currentTenant.Change(null))
            {
                var findTenant = await _tenantRepository.FindAsync(tenant.Id, true);
                if (findTenant == null)
                {
                    return;
                }
                var connectionStrings = new ConnectionStrings();
                foreach (var tenantConnectionString in findTenant.ConnectionStrings)
                {
                    connectionStrings[tenantConnectionString.Name] = tenantConnectionString.Value;
                }
                var cacheItem = new TenantConfigurationCacheItem(tenant.Id, tenant.Name, connectionStrings);

                await _cache.SetAsync(
                    TenantConfigurationCacheItem.CalculateCacheKey(findTenant.Id.ToString()),
                    cacheItem);

                await _cache.SetAsync(
                    TenantConfigurationCacheItem.CalculateCacheKey(findTenant.Name),
                    cacheItem);
            }
        }
        catch (Exception ex)
        {
            _logger.LogException(ex);
        }
    }
}
