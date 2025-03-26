using System;

namespace LINGYUN.Abp.DataProtection;

public class DataAccessEntitTypeInfoContext
{
    public Type EntityType { get; }
    public Type ResourceType { get; }
    public DataAccessOperation Operation { get; }
    public IServiceProvider ServiceProvider { get; }
    public DataAccessEntitTypeInfoContext(
        Type entityType,
        Type resourceType,
        DataAccessOperation operation,
        IServiceProvider serviceProvider)
    {
        EntityType = entityType;
        ResourceType = resourceType;
        Operation = operation;
        ServiceProvider = serviceProvider;
    }
}
