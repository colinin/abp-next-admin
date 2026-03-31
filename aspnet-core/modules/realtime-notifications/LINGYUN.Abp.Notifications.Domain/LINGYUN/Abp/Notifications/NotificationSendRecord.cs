using System;
using Volo.Abp;
using Volo.Abp.Domain.Entities;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Abp.Notifications;
public class NotificationSendRecord : Entity<long>, IMultiTenant
{
    public virtual Guid? TenantId { get; protected set; }
    public virtual string Provider { get; protected set; }
    public virtual DateTime SendTime { get; protected set; }
    public virtual Guid UserId { get; protected set; }
    public virtual string UserName { get; protected set; }
    public virtual long NotificationId { get; protected set; }
    public virtual string NotificationName { get; protected set; }
    public virtual NotificationSendState State { get; protected set; }
    public virtual string Reason { get; protected set; }
    protected NotificationSendRecord()
    {
    }

    public NotificationSendRecord(
        string provider,
        DateTime sendTime, 
        Guid userId, 
        string userName, 
        long notificationId, 
        string notificationName,
        NotificationSendState state,
        string reason = null,
        Guid? tenantId = null)
    {
        Provider = Check.NotNullOrWhiteSpace(provider, nameof(provider), NotificationSendRecordConsts.MaxProviderLength);
        SendTime = sendTime;
        UserId = userId;
        UserName = Check.Length(userName, nameof(userName), SubscribeConsts.MaxUserNameLength);
        NotificationId = notificationId;
        NotificationName = Check.NotNullOrWhiteSpace(notificationName, nameof(notificationName), NotificationConsts.MaxNameLength);
        State = state;
        Reason = reason;
        TenantId = tenantId;

        if (!Reason.IsNullOrWhiteSpace() && Reason.Length > NotificationSendRecordConsts.MaxReasonLength)
        {
            Reason = Reason.Substring(0, NotificationSendRecordConsts.MaxReasonLength);
        }
    }
}
