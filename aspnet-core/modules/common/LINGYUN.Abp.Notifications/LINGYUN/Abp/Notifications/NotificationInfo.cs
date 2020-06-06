using Newtonsoft.Json;
using System;

namespace LINGYUN.Abp.Notifications
{
    public class NotificationInfo
    {
        public Guid? TenantId { get; set; }
        public string Name { get; set; }
        public string Id { get; set; }
        public NotificationData Data { get; set; }
        public DateTime CreationTime { get; set; }
        public NotificationType NotificationType { get; set; }
        public NotificationSeverity NotificationSeverity { get; set; }
        public NotificationInfo()
        {
            Data = new NotificationData();
            NotificationType = NotificationType.Application;
            NotificationSeverity = NotificationSeverity.Info;

            CreationTime = DateTime.Now;
        }

        public long GetId()
        {
            return long.Parse(Id);
        }
    }
}
