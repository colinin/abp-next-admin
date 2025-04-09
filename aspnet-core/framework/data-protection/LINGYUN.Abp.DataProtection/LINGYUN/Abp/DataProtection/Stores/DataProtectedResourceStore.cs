using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.DataProtection.Stores;

public class DataProtectedResourceStore : IDataProtectedResourceStore, ITransientDependency
{
    private readonly IDataProtectedResourceCache _cache;

    public DataProtectedResourceStore(IDataProtectedResourceCache cache)
    {
        _cache = cache;
    }

    public virtual DataAccessResource Get(string subjectName, string subjectId, string entityTypeFullName, DataAccessOperation operation)
    {
        var cacheItem = _cache.GetCache(subjectName, subjectId, entityTypeFullName, operation);
        if (cacheItem == null)
        {
            return null;
        }
        return new DataAccessResource(
            cacheItem.SubjectName,
            cacheItem.SubjectId,
            cacheItem.EntityTypeFullName,
            cacheItem.Operation,
            cacheItem.FilterGroup)
        {
            AccessedProperties = cacheItem.AccessdProperties,
        };
    }

    public async virtual Task<DataAccessResource> GetAsync(string subjectName, string subjectId, string entityTypeFullName, DataAccessOperation operation)
    {
        var cacheItem = await _cache.GetCacheAsync(subjectName, subjectId, entityTypeFullName, operation);
        if (cacheItem == null)
        {
            return null;
        }
        return new DataAccessResource(
            cacheItem.SubjectName,
            cacheItem.SubjectId,
            cacheItem.EntityTypeFullName,
            cacheItem.Operation,
            cacheItem.FilterGroup)
        {
            AccessedProperties = cacheItem.AccessdProperties,
        };
    }

    public virtual void Remove(DataAccessResource resource)
    {
        _cache.RemoveCache(resource);
    }

    public async virtual Task RemoveAsync(DataAccessResource resource)
    {
        await _cache.RemoveCacheAsync(resource);
    }

    public void Set(DataAccessResource resource)
    {
        _cache.SetCache(resource);
    }

    public async virtual Task SetAsync(DataAccessResource resource)
    {
        await _cache.SetCacheAsync(resource);
    }
}
