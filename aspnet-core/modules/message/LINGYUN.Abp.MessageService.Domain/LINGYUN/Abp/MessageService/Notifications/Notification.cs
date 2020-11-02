using LINGYUN.Abp.Notifications;
using System;
using Volo.Abp.Auditing;
using Volo.Abp.Domain.Entities;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Abp.MessageService.Notifications
{
    public class Notification : Entity<long>, IMultiTenant, IHasCreationTime
    {
        public virtual Guid? TenantId { get; protected set; }
        public virtual NotificationSeverity Severity { get; protected set; }
        public virtual NotificationType Type { get; set; }
        public virtual long NotificationId { get; protected set; }
        public virtual string NotificationName { get; protected set; }
        public virtual string NotificationData { get; protected set; }
        public virtual string NotificationTypeName { get; protected set; }
        public virtual DateTime? ExpirationTime { get; set; }
        public virtual DateTime CreationTime { get; set; }
        protected Notification(){}

        public Notification(long id)
        {
            Id = id;
        }

        public Notification(long id, string name, string dataType, string data, 
            NotificationSeverity severity = NotificationSeverity.Info,
            Guid? tenantId = null)
        {
            NotificationId = id;
            Severity = severity;
            NotificationName = name;
            NotificationData = data;
            NotificationTypeName = dataType;
            Type = NotificationType.Application;
            TenantId = tenantId;
        }
    }
}
