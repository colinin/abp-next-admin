using System;
using Volo.Abp.Auditing;

namespace LINGYUN.Abp.Notifications;

public class UserSubscribe : Subscribe, IHasCreationTime
{
    public virtual Guid UserId { get; set; }
    public virtual string UserName { get; set; }
    protected UserSubscribe() { }
    public UserSubscribe(string notificationName, Guid userId, string userName, Guid? tenantId = null) 
        : base(notificationName, tenantId)
    {
        UserId = userId;
        UserName = userName;
    }
}
