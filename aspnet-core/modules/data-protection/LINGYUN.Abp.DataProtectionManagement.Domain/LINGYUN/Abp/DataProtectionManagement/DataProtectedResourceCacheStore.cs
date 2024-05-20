using LINGYUN.Abp.DataProtection;
using Microsoft.Extensions.DependencyInjection;
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

    public DataAccessResource Get(string subjectName, string subjectId, string entityTypeFullName, DataAccessOperation operation)
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
            AllowProperties = cacheItem.AllowProperties,
        };
    }

    public void Remove(DataAccessResource resource)
    {
        _cache.RemoveCache(resource);
    }

    public void Set(DataAccessResource resource)
    {
        _cache.SetCache(resource);
    }
}
