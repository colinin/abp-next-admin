using System;
using Volo.Abp.Domain.Entities;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Abp.Notifications;

public class UserNotification : Entity<long>, IMultiTenant
{
    public virtual Guid? TenantId { get; protected set; }
    public virtual Guid UserId { get; protected set; }
    public virtual long NotificationId { get; protected set; }
    public virtual NotificationReadState ReadStatus { get; protected set; }
    protected UserNotification() { }
    public UserNotification(long notificationId, Guid userId, Guid? tenantId = null)
    {
        UserId = userId;
        NotificationId = notificationId;
        ReadStatus = NotificationReadState.UnRead;
        TenantId = tenantId;
    }

    public void ChangeReadState(NotificationReadState readStatus)
    {
        ReadStatus = readStatus;
    }
}
