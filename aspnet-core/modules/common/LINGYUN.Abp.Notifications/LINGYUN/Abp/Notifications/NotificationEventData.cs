using System;

namespace LINGYUN.Abp.Notifications
{
    public class NotificationEventData
    {
        public Guid? TenantId { get; set; }
        public string CateGory { get; set; }
        public string Name { get; set; }
        public string Id { get; set; }
        public NotificationData Data { get; set; }
        public DateTime CreationTime { get; set; }
        public NotificationLifetime Lifetime { get; set; }
        public NotificationType NotificationType { get; set; }
        public NotificationSeverity NotificationSeverity { get; set; }

        public NotificationEventData()
        {

        }

        public NotificationInfo ToNotificationInfo()
        {
            return new NotificationInfo
            {
                NotificationSeverity = NotificationSeverity,
                CreationTime = CreationTime,
                Data = Data,
                Id = Id,
                Name = Name,
                CateGory = CateGory,
                NotificationType = NotificationType,
                Lifetime = Lifetime,
                TenantId = TenantId
            };
        }
    }
}
