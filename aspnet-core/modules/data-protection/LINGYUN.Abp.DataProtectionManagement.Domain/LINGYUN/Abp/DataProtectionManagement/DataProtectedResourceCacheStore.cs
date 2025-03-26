using LINGYUN.Abp.DataProtection;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.DataProtectionManagement;

[Dependency(ServiceLifetime.Transient, ReplaceServices = true)]
[ExposeServices(typeof(IDataProtectedResourceStore), typeof(DataProtectedResourceCacheStore))]
public class DataProtectedResourceCacheStore : IDataProtectedResourceStore, ITransientDependency
{
    private readonly IDataProtectedResourceCache _cache;

    public DataProtectedResourceCacheStore(IDataProtectedResourceCache cache)
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
