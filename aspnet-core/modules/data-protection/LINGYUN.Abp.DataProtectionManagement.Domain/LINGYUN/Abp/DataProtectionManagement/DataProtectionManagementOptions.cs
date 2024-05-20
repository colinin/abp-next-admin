using System;
using System.Collections.Generic;

namespace LINGYUN.Abp.DataProtectionManagement;

public class DataProtectionManagementOptions
{
    /// <summary>
    /// 受保护的实体类型
    /// </summary>
    public IDictionary<Type, List<Type>> ProtectedEntities { get; }

    public DataProtectionManagementOptions()
    {
        ProtectedEntities = new Dictionary<Type, List<Type>>();
    }

    public void AddEntity<TResource, TEntity>()
    {
        AddEntity(typeof(TResource), typeof(TEntity));
    }

    public void AddEntity(Type resourceType, Type entityType)
    {
        if (!ProtectedEntities.TryGetValue(resourceType, out var entityTypes))
        {
            entityTypes = new List<Type>();
        }
        entityTypes.Add(entityType);
        ProtectedEntities[resourceType] = entityTypes;
    }

    public void AddEntities(Type resourceType, params Type[] entityTypes)
    {
        if (!ProtectedEntities.TryGetValue(resourceType, out var findTypes))
        {
            findTypes = new List<Type>();
        }
        findTypes.AddIfNotContains(entityTypes);
        ProtectedEntities[resourceType] = findTypes;
    }
}
