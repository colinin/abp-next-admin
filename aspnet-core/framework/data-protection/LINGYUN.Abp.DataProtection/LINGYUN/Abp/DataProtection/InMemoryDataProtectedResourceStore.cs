using Microsoft.Extensions.DependencyInjection;
using System.Collections.Concurrent;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.DataProtection;

[Dependency(ServiceLifetime.Singleton, TryRegister = true)]
public class InMemoryDataProtectedResourceStore : IDataProtectedResourceStore
{
    private readonly static ConcurrentDictionary<string, DataAccessResource> _cache = new ConcurrentDictionary<string, DataAccessResource>();
    public DataAccessResource Get(string subjectName, string subjectId, string entityTypeFullName, DataAccessOperation operation)
    {
        var key = NormalizeKey(subjectName, subjectId, entityTypeFullName, operation);
        if (_cache.TryGetValue(key, out var resource))
        {
            return resource;
        }
        return null;
    }

    public void Remove(DataAccessResource resource)
    {
        var key = NormalizeKey(resource.SubjectName, resource.SubjectId, resource.EntityTypeFullName, resource.Operation);
        _cache.TryRemove(key, out var _);
    }

    public void Set(DataAccessResource resource)
    {
        var key = NormalizeKey(resource.SubjectName, resource.SubjectId, resource.EntityTypeFullName, resource.Operation);
        _cache.TryAdd(key, resource);
    }

    private static string NormalizeKey(string subjectName, string subjectId, string entityTypeFullName, DataAccessOperation operation)
    {
        return $"{subjectName}_{subjectId}_{entityTypeFullName}_{operation}";
    }
}
