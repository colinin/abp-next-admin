using System;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Abp.Identity;

public class IdentityUserInactive : CreationAuditedEntity<long>, IMultiTenant
{
    public virtual Guid UserId { get; protected set; }

    public virtual Guid? TenantId { get; protected set; }

    public virtual DateTimeOffset LastSignInTime { get; protected set; }

    public virtual DateTimeOffset? LastNotificationTime { get; protected set; }

    public virtual InactiveUserStatus Status { get; protected set; }

    public virtual DateTimeOffset? DeactivatedTime { get; protected set; }

    protected IdentityUserInactive()
    {

    }

    public IdentityUserInactive(
        Guid userId,
        DateTimeOffset lastSignInTime,
        Guid? tenantId = null)
    {
        UserId = userId;
        TenantId = tenantId;
        LastSignInTime = lastSignInTime;
        Status = InactiveUserStatus.Notified;
    }

    public void MarkNotified(DateTimeOffset now)
    {
        LastNotificationTime = now;
        Status = InactiveUserStatus.Notified;
    }

    public void MarkDeactivated(DateTimeOffset now)
    {
        DeactivatedTime = now;
        Status = InactiveUserStatus.Deactivated;
    }
}
