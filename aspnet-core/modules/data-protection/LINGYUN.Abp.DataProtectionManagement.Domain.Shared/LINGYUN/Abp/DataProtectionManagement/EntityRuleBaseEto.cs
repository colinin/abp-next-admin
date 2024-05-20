using LINGYUN.Abp.DataProtection;
using System;
using Volo.Abp.Domain.Entities.Events.Distributed;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Abp.DataProtectionManagement;

[Serializable]
public abstract class EntityRuleBaseEto : EntityEto<Guid>, IMultiTenant
{
    public Guid? TenantId { get; set; }
    public bool IsEnabled { get; set; }
    public DataAccessOperation Operation { get; set; }
    public Guid EntityTypeId { get; set; }
    public string EntityTypeFullName { get; set; }
}
