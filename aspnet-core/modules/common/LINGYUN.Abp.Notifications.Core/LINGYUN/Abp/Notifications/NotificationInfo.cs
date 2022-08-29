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
        public NotificationLifetime Lifetime { get; set; }
        public NotificationType Type { get; set; }
        public NotificationSeverity Severity { get; set; }
        public NotificationInfo()
        {
            Data = new NotificationData();
            Lifetime = NotificationLifetime.Persistent;
            Type = NotificationType.Application;
            Severity = NotificationSeverity.Info;

            CreationTime = DateTime.Now;
        }

        public void SetId(long id)
        {
            if (Id.IsNullOrWhiteSpace())
            {
                Id = id.ToString();
            }
        }

        public long GetId()
        {
            return long.Parse(Id);
        }
    }
}
