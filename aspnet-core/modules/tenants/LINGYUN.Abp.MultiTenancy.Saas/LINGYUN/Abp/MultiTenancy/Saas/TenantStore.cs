using JetBrains.Annotations;
using LINGYUN.Abp.Saas.Tenants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Threading;

namespace LINGYUN.Abp.MultiTenancy.Saas;

public class TenantStore : ITenantStore, ITransientDependency
{
    protected ICurrentTenant CurrentTenant { get; }
    protected ITenantAppService TenantAppService { get; }
    protected IDistributedCache<TenantCacheItem> Cache { get; }

    public TenantStore(
        ICurrentTenant currentTenant,
        ITenantAppService tenantAppService,
        IDistributedCache<TenantCacheItem> cache)
    {
        Cache = cache;
        CurrentTenant = currentTenant;
        TenantAppService = tenantAppService;
    }

    public virtual async Task<TenantConfiguration> FindAsync(string name)
    {
        return (await GetCacheItemAsync(null, name)).Value;
    }

    public virtual async Task<TenantConfiguration> FindAsync(Guid id)
    {
        return (await GetCacheItemAsync(id, null)).Value;
    }

    [Obsolete("Use FindAsync method.")]
    public virtual TenantConfiguration Find(string name)
    {
        return AsyncHelper.RunSync(async () => await FindAsync(name));
    }

    [Obsolete("Use FindAsync method.")]
    public virtual TenantConfiguration Find(Guid id)
    {
        return AsyncHelper.RunSync(async () => await FindAsync(id));
    }

    protected virtual async Task<TenantCacheItem> GetCacheItemAsync(Guid? id, string name)
    {
        var cacheKey = CalculateCacheKey(id, name);

        var cacheItem = await Cache.GetAsync(cacheKey, considerUow: true);
        if (cacheItem != null)
        {
            return cacheItem;
        }

        if (id.HasValue)
        {
            using (CurrentTenant.Change(null)) //TODO: No need this if we can implement to define host side (or tenant-independent) entities!
            {
                var tenant = await TenantAppService.GetAsync(id.Value);
                var connectionStrings = await TenantAppService.GetConnectionStringAsync(id.Value);
                return await SetCacheAsync(cacheKey, tenant, connectionStrings.Items);
            }
        }

        if (!name.IsNullOrWhiteSpace())
        {
            using (CurrentTenant.Change(null)) //TODO: No need this if we can implement to define host side (or tenant-independent) entities!
            {
                var tenant = await TenantAppService.GetAsync(name);
                IReadOnlyList<TenantConnectionStringDto> connectionStrings = new List<TenantConnectionStringDto>();
                if (tenant != null)
                {
                    var connectionStringsResult = await TenantAppService.GetConnectionStringAsync(tenant.Id);
                    connectionStrings = connectionStringsResult.Items;
                }
                return await SetCacheAsync(cacheKey, tenant, connectionStrings);
            }
        }

        throw new AbpException("Both id and name can't be invalid.");
    }

    protected virtual async Task<TenantCacheItem> SetCacheAsync(
        string cacheKey,
        [CanBeNull] TenantDto tenant,
        [CanBeNull] IReadOnlyList<TenantConnectionStringDto> connectionStrings)
    {
        var tenantConfiguration = tenant != null
            ? new TenantConfiguration(tenant.Id, tenant.Name)
            {
                IsActive = tenant.IsActive,
            }
            : null;
        if (tenantConfiguration != null && connectionStrings?.Any() == true)
        {
            foreach (var connectionString in connectionStrings)
            {
                tenantConfiguration.ConnectionStrings.Add(
                    connectionString.Name,
                    connectionString.Value);
            }
        }
        var cacheItem = new TenantCacheItem(tenantConfiguration);
        await Cache.SetAsync(cacheKey, cacheItem, considerUow: true);
        return cacheItem;
    }

    protected virtual string CalculateCacheKey(Guid? id, string name)
    {
        return TenantCacheItem.CalculateCacheKey(id, name);
    }
}
