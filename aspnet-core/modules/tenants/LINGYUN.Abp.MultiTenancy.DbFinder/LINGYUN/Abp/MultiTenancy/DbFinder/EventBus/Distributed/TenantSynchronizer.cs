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
    IDistributedEventHandler<ConnectionStringCreatedEventData>,
    IDistributedEventHandler<ConnectionStringDeletedEventData>,
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

    /// <summary>
    /// 处理租户变更事件
    /// </summary>
    /// <remarks>
    /// 更新租户缓存
    /// </remarks>
    /// <param name="eventData"></param>
    /// <returns></returns>
    [UnitOfWork]
    public virtual async Task HandleEventAsync(EntityUpdatedEto<TenantEto> eventData)
    {
        await UpdateCacheItemAsync(eventData.Entity.Id, eventData.Entity.Name);
    }

    /// <summary>
    /// 处理租户新增事件
    /// </summary>
    /// <remarks>
    /// 更新租户缓存
    /// </remarks>
    /// <param name="eventData"></param>
    /// <returns></returns>
    [UnitOfWork]
    public virtual async Task HandleEventAsync(EntityCreatedEto<TenantEto> eventData)
    {
        await UpdateCacheItemAsync(eventData.Entity.Id, eventData.Entity.Name);
    }

    /// <summary>
    /// 处理租户删除事件
    /// </summary>
    /// <remarks>
    /// 删除租户缓存
    /// </remarks>
    /// <param name="eventData"></param>
    /// <returns></returns>
    public virtual async Task HandleEventAsync(EntityDeletedEto<TenantEto> eventData)
    {
        await RemoveCacheItemAsync(eventData.Entity.Id, eventData.Entity.Name);
    }

    /// <summary>
    /// 处理租户连接字符串创建事件
    /// </summary>
    /// <remarks>
    /// 更新租户缓存
    /// </remarks>
    /// <param name="eventData"></param>
    /// <returns></returns>
    [UnitOfWork]
    public virtual async Task HandleEventAsync(ConnectionStringCreatedEventData eventData)
    {
        await UpdateCacheItemAsync(eventData.TenantId, eventData.TenantName);
    }

    /// <summary>
    /// 处理租户连接字符串删除事件
    /// </summary>
    /// <remarks>
    /// 删除租户缓存
    /// </remarks>
    /// <param name="eventData"></param>
    /// <returns></returns>
    public virtual async Task HandleEventAsync(ConnectionStringDeletedEventData eventData)
    {
        // TODO: 用更新还是用删除?
        await RemoveCacheItemAsync(eventData.TenantId, eventData.TenantName);
    }

    protected virtual async Task UpdateCacheItemAsync(Guid tenantId, string tenantName = null)
    {
        try
        {
            using (_currentTenant.Change(null))
            {
                var findTenant = await _tenantRepository.FindAsync(tenantId, true);
                if (findTenant == null)
                {
                    return;
                }
                var connectionStrings = new ConnectionStrings();
                foreach (var tenantConnectionString in findTenant.ConnectionStrings)
                {
                    connectionStrings[tenantConnectionString.Name] = tenantConnectionString.Value;
                }
                var cacheItem = new TenantConfigurationCacheItem(findTenant.Id, findTenant.Name, connectionStrings);

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
