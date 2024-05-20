using LINGYUN.Abp.DataProtection;
using System;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Abp.DataProtectionManagement;
public abstract class EntityRuleBase : AuditedAggregateRoot<Guid>, IMultiTenant
{
    public virtual Guid? TenantId { get; protected set; }
    public virtual bool IsEnabled { get; set; }
    public virtual DataAccessOperation Operation { get; set; }
    public virtual DataAccessFilterGroup FilterGroup { get; set; }
    public virtual Guid EntityTypeId { get; protected set; }
    public virtual string EntityTypeFullName { get; protected set; }
    public virtual EntityTypeInfo EntityTypeInfo { get; protected set; }
    public virtual string AllowProperties { get; set; }
    protected EntityRuleBase()
    {
    }

    protected EntityRuleBase(
        Guid id, 
        Guid entityTypeId, 
        string enetityTypeFullName, 
        DataAccessOperation operation, 
        string allowProperties = null,
        DataAccessFilterGroup filterGroup = null, 
        Guid? tenantId = null)
        : base(id)
    {
        Operation = operation;
        FilterGroup = filterGroup;
        TenantId = tenantId;

        EntityTypeId = entityTypeId;
        EntityTypeFullName = Check.NotNullOrWhiteSpace(enetityTypeFullName, nameof(enetityTypeFullName), EntityRuleConsts.MaxEntityTypeFullNameLength);
        AllowProperties = Check.Length(allowProperties, nameof(allowProperties), EntityRuleConsts.MaxAllowPropertiesLength);
    }


}
