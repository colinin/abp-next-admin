using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;
using Volo.Abp.MultiTenancy;
using Volo.Abp.ObjectMapping;

namespace LINGYUN.Abp.Saas.Tenants;

public class TenantStore : ITenantStore, ITransientDependency
{
    protected ITenantRepository TenantRepository { get; }
    protected IObjectMapper<AbpSaasDomainModule> ObjectMapper { get; }
    protected ICurrentTenant CurrentTenant { get; }
    protected IDistributedCache<TenantCacheItem> TenantCache { get; }
    protected IDistributedCache<TenantsCacheItem> TenantsCache { get; }

    public TenantStore(
        ITenantRepository tenantRepository,
        IObjectMapper<AbpSaasDomainModule> objectMapper,
        ICurrentTenant currentTenant,
        IDistributedCache<TenantCacheItem> tenantCache,
        IDistributedCache<TenantsCacheItem> tenantsCache)
    {
        TenantRepository = tenantRepository;
        ObjectMapper = objectMapper;
        CurrentTenant = currentTenant;
        TenantCache = tenantCache;
        TenantsCache = tenantsCache;
    }

    public async virtual Task<TenantConfiguration> FindAsync(string name)
    {
        return (await GetCacheItemAsync(null, name)).Value;
    }

    public async virtual Task<TenantConfiguration> FindAsync(Guid id)
    {
        return (await GetCacheItemAsync(id, null)).Value;
    }

    public async virtual Task<IReadOnlyList<TenantConfiguration>> GetListAsync(bool includeDetails = false)
    {
        var cacheKey = TenantsCacheItem.CalculateCacheKey(includeDetails);
        var cacheItem = await TenantsCache.GetAsync(cacheKey);
        if (cacheItem == null)
        {
            var tenants = await TenantRepository.GetListAsync(includeDetails);
            var tenantConfiurations = ObjectMapper.Map<List<Tenant>, List<TenantConfiguration>>(tenants);
            cacheItem = new TenantsCacheItem(tenantConfiurations);
            await TenantsCache.SetAsync(cacheKey, cacheItem, considerUow: true);
        }
        return [.. cacheItem.Tenants];
    }

    [Obsolete("Use FindAsync method.")]
    public virtual TenantConfiguration Find(string name)
    {
        return (GetCacheItem(null, name)).Value;
    }

    [Obsolete("Use FindAsync method.")]
    public virtual TenantConfiguration Find(Guid id)
    {
        return (GetCacheItem(id, null)).Value;
    }

    protected async virtual Task<TenantCacheItem> GetCacheItemAsync(Guid? id, string name)
    {
        var cacheKey = CalculateCacheKey(id, name);

        var cacheItem = await TenantCache.GetAsync(cacheKey, considerUow: true);
        if (cacheItem != null)
        {
            return cacheItem;
        }

        if (id.HasValue)
        {
            using (CurrentTenant.Change(null)) //TODO: No need this if we can implement to define host side (or tenant-independent) entities!
            {
                var tenant = await TenantRepository.FindAsync(id.Value);
                return await SetCacheAsync(cacheKey, tenant);
            }
        }

        if (!name.IsNullOrWhiteSpace())
        {
            using (CurrentTenant.Change(null)) //TODO: No need this if we can implement to define host side (or tenant-independent) entities!
            {
                var tenant = await TenantRepository.FindByNameAsync(name);
                return await SetCacheAsync(cacheKey, tenant);
            }
        }

        throw new AbpException("Both id and name can't be invalid.");
    }

    protected async virtual Task<TenantCacheItem> SetCacheAsync(string cacheKey, [CanBeNull] Tenant tenant)
    {
        var tenantConfiguration = tenant != null ? ObjectMapper.Map<Tenant, TenantConfiguration>(tenant) : null;
        var cacheItem = new TenantCacheItem(tenantConfiguration);
        await TenantCache.SetAsync(cacheKey, cacheItem, considerUow: true);
        return cacheItem;
    }

    [Obsolete("Use GetCacheItemAsync method.")]
    protected virtual TenantCacheItem GetCacheItem(Guid? id, string name)
    {
        var cacheKey = CalculateCacheKey(id, name);

        var cacheItem = TenantCache.Get(cacheKey, considerUow: true);
        if (cacheItem != null)
        {
            return cacheItem;
        }

        if (id.HasValue)
        {
            using (CurrentTenant.Change(null)) //TODO: No need this if we can implement to define host side (or tenant-independent) entities!
            {
                var tenant = TenantRepository.FindById(id.Value);
                return SetCache(cacheKey, tenant);
            }
        }

        if (!name.IsNullOrWhiteSpace())
        {
            using (CurrentTenant.Change(null)) //TODO: No need this if we can implement to define host side (or tenant-independent) entities!
            {
                var tenant = TenantRepository.FindByName(name);
                return SetCache(cacheKey, tenant);
            }
        }

        throw new AbpException("Both id and name can't be invalid.");
    }

    [Obsolete("Use SetCacheAsync method.")]
    protected virtual TenantCacheItem SetCache(string cacheKey, [CanBeNull] Tenant tenant)
    {
        var tenantConfiguration = tenant != null ? ObjectMapper.Map<Tenant, TenantConfiguration>(tenant) : null;
        var cacheItem = new TenantCacheItem(tenantConfiguration);
        TenantCache.Set(cacheKey, cacheItem, considerUow: true);
        return cacheItem;
    }

    protected virtual string CalculateCacheKey(Guid? id, string name)
    {
        return TenantCacheItem.CalculateCacheKey(id, name);
    }
}
