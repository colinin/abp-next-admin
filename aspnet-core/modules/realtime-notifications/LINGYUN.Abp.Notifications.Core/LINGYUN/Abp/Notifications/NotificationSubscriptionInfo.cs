using System;

namespace LINGYUN.Abp.Notifications;

public class NotificationSubscriptionInfo
{
    public Guid? TenantId { get; set; }
    public Guid UserId { get; set; }
    public string UserName { get; set; }
    public string NotificationName { get; set; }
}
