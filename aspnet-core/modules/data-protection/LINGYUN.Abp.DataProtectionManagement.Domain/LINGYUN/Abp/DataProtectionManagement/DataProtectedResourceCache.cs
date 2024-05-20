using LINGYUN.Abp.DataProtection;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.DataProtectionManagement;
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

    public virtual void RemoveCache(DataAccessResource resource)
    {
        var cacheKey = DataProtectedResourceCacheItem.CalculateCacheKey(resource.SubjectName, resource.SubjectId, resource.EntityTypeFullName, resource.Operation);
        _cache.Remove(cacheKey);
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
            AllowProperties = resource.AllowProperties,
        };
       _cache.Set(cacheKey, cacheItem);
    }
}
