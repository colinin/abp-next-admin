using System;
using Volo.Abp.Auditing;
using Volo.Abp.Guids;

namespace LINGYUN.Abp.AuditLogging;

[DisableAuditing]
public class EntityPropertyChange
{
    public Guid Id { get; set; }

    public Guid? TenantId { get; set; }

    public Guid EntityChangeId { get; set; }

    public string? NewValue { get; set; }

    public string? OriginalValue { get; set; }

    public string PropertyName { get; set; } = default!;

    public string PropertyTypeFullName { get; set; } = default!;

    public EntityPropertyChange()
    {
    }

    public EntityPropertyChange(
        IGuidGenerator guidGenerator,
        Guid entityChangeId,
        EntityPropertyChangeInfo entityChangeInfo,
        Guid? tenantId = null)
    {
        Id = guidGenerator.Create();
        TenantId = tenantId;
        EntityChangeId = entityChangeId;
        NewValue = entityChangeInfo.NewValue;
        OriginalValue = entityChangeInfo.OriginalValue;
        PropertyName = entityChangeInfo.PropertyName;
        PropertyTypeFullName = entityChangeInfo.PropertyTypeFullName;
    }
}