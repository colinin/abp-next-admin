using JetBrains.Annotations;
using LINGYUN.Abp.MultiTenancy.Editions;
using System;
using System.Threading.Tasks;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;
using Volo.Abp.MultiTenancy;
using Volo.Abp.ObjectMapping;

namespace LINGYUN.Abp.Saas.Editions;

public class EditionStore : IEditionStore, ITransientDependency
{
    protected IEditionRepository EditionRepository { get; }
    protected IObjectMapper<AbpSaasDomainModule> ObjectMapper { get; }
    protected ICurrentTenant CurrentTenant { get; }
    protected IDistributedCache<EditionCacheItem> Cache { get; }

    public EditionStore(
        IEditionRepository editionRepository,
        IObjectMapper<AbpSaasDomainModule> objectMapper,
        ICurrentTenant currentTenant,
        IDistributedCache<EditionCacheItem> cache)
    {
        EditionRepository = editionRepository;
        ObjectMapper = objectMapper;
        CurrentTenant = currentTenant;
        Cache = cache;
    }

    public async virtual Task<EditionInfo> FindByTenantAsync(Guid tenantId)
    {
        return (await GetCacheItemAsync(tenantId)).Value;
    }

    protected async virtual Task<EditionCacheItem> GetCacheItemAsync(Guid tenantId)
    {
        var cacheKey = CalculateCacheKey(tenantId);

        var cacheItem = await Cache.GetAsync(cacheKey, considerUow: true);
        if (cacheItem != null)
        {
            return cacheItem;
        }

        using (CurrentTenant.Change(null))
        {
            var edition = await EditionRepository.FindByTenantIdAsync(tenantId);
            return await SetCacheAsync(cacheKey, edition);
        }
    }

    protected async virtual Task<EditionCacheItem> SetCacheAsync(string cacheKey, [CanBeNull] Edition edition)
    {
        var editionInfo = edition != null ? ObjectMapper.Map<Edition, EditionInfo>(edition) : null;
        var cacheItem = new EditionCacheItem(editionInfo);
        await Cache.SetAsync(cacheKey, cacheItem, considerUow: true);
        return cacheItem;
    }

    protected virtual string CalculateCacheKey(Guid tenantId)
    {
        return EditionCacheItem.CalculateCacheKey(tenantId);
    }
}
