using System;
using Volo.Abp.Auditing;
using Volo.Abp.Domain.Entities;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Abp.Notifications;

public abstract class Subscribe : Entity<long>, IMultiTenant, IHasCreationTime
{
    public virtual Guid? TenantId { get; protected set; }
    public virtual DateTime CreationTime { get; set; }
    public virtual string NotificationName { get; protected set; }

    protected Subscribe() { }

    protected Subscribe(string notificationName, Guid? tenantId = null)
    {
        NotificationName = notificationName;
        TenantId = tenantId;
    }
}
