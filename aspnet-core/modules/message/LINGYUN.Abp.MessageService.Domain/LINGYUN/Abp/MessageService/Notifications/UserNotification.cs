using System;
using Volo.Abp.Domain.Entities;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Abp.MessageService.Notifications
{
    public class UserNotification : Entity<long>, IMultiTenant
    {
        public virtual Guid? TenantId { get; protected set; }
        public virtual Guid UserId { get; protected set; }
        public virtual long NotificationId { get; protected set; }
        public virtual ReadStatus ReadStatus { get; protected set; }
        protected UserNotification() { }
        public UserNotification(long notificationId, Guid userId)
        {
            UserId = userId;
            NotificationId = notificationId;
        }
    }
}
