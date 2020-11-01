using System;
using System.Collections.Generic;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Abp.Notifications
{
    public class NotificationEventData : IMultiTenant
    {
        public Guid? TenantId { get; set; }
        public string Name { get; set; }
        public NotificationData Data { get; set; }
        public DateTime CreationTime { get; set; }
        public NotificationSeverity Severity { get; set; }
        public List<UserIdentifier> Users { get; set; }
        public NotificationEventData()
        {
            Users = new List<UserIdentifier>();
        }
    }
}
