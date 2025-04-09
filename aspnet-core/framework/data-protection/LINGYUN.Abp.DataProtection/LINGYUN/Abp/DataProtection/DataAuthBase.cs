using System;
using Volo.Abp.Domain.Entities;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Abp.DataProtection;

public abstract class DataAuthBase<TEntity, TKey> : Entity<long>, IMultiTenant
{
    public virtual Guid? TenantId { get; protected set; }
    public virtual TKey EntityId { get; protected set; }
    public virtual TEntity Entity { get; protected set; }
    public virtual string EntityType { get; protected set; }
    public virtual string Role { get; protected set; }
    public virtual string OrganizationUnit { get; protected set; }
    protected DataAuthBase()
    {

    }

    protected DataAuthBase(
        TKey entityId, 
        string role, 
        string organizationUnit,
        Guid? tenantId = null)
    {
        TenantId = tenantId;
        EntityId = entityId;
        Role = role;
        OrganizationUnit = organizationUnit;

        EntityType = typeof(TEntity).FullName;
    }
}
