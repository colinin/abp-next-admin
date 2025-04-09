using Microsoft.Extensions.Caching.Distributed;
using System.Threading.Tasks;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.DataProtection.Stores;
public class DataProtectedResourceCache : IDataProtectedResourceCache, ITransientDependency
{
    private readonly IDistributedCache<DataProtectedResourceCacheItem> _cache;

    public DataProtectedResourceCache(IDistributedCache<DataProtectedResourceCacheItem> cache)
    {
        _cache = cache;
    }

    public virtual DataProtectedResourceCacheItem GetCache(string subjectName, string subjectId, string entityTypeFullName, DataAccessOperation operation)
    {
        var cacheKey = DataProtectedResourceCacheItem.CalculateCacheKey(subjectName, subjectId, entityTypeFullName, operation);
        var cacheItem = _cache.Get(cacheKey);
        return cacheItem;
    }

    public async virtual Task<DataProtectedResourceCacheItem> GetCacheAsync(string subjectName, string subjectId, string entityTypeFullName, DataAccessOperation operation)
    {
        var cacheKey = DataProtectedResourceCacheItem.CalculateCacheKey(subjectName, subjectId, entityTypeFullName, operation);
        var cacheItem = await _cache.GetAsync(cacheKey);
        return cacheItem;
    }

    public virtual void RemoveCache(DataAccessResource resource)
    {
        var cacheKey = DataProtectedResourceCacheItem.CalculateCacheKey(resource.SubjectName, resource.SubjectId, resource.EntityTypeFullName, resource.Operation);
        _cache.Remove(cacheKey);
    }

    public async Task RemoveCacheAsync(DataAccessResource resource)
    {
        var cacheKey = DataProtectedResourceCacheItem.CalculateCacheKey(resource.SubjectName, resource.SubjectId, resource.EntityTypeFullName, resource.Operation);
        await _cache.RemoveAsync(cacheKey);
    }

    public virtual void SetCache(DataAccessResource resource)
    {
        var cacheKey = DataProtectedResourceCacheItem.CalculateCacheKey(resource.SubjectName, resource.SubjectId, resource.EntityTypeFullName, resource.Operation);
        var cacheItem = new DataProtectedResourceCacheItem(
            resource.SubjectName,
            resource.SubjectId,
            resource.EntityTypeFullName,
            resource.Operation,
            resource.FilterGroup)
        {
            AccessdProperties = resource.AccessedProperties,
        };
       _cache.Set(cacheKey, cacheItem, new DistributedCacheEntryOptions());
    }

    public async virtual Task SetCacheAsync(DataAccessResource resource)
    {
        var cacheKey = DataProtectedResourceCacheItem.CalculateCacheKey(resource.SubjectName, resource.SubjectId, resource.EntityTypeFullName, resource.Operation);
        var cacheItem = new DataProtectedResourceCacheItem(
            resource.SubjectName,
            resource.SubjectId,
            resource.EntityTypeFullName,
            resource.Operation,
            resource.FilterGroup)
        {
            AccessdProperties = resource.AccessedProperties,
        };
        await _cache.SetAsync(cacheKey, cacheItem, new DistributedCacheEntryOptions());
    }
}
