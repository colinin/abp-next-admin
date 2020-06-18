using System;

namespace LINGYUN.Abp.Notifications
{
    public class NotificationInfo
    {
        public Guid? TenantId { get; set; }
        public string Name { get; set; }
        public string CateGory { get; set; }
        public string Id { get; set; }
        public NotificationData Data { get; set; }
        public DateTime CreationTime { get; set; }
        public NotificationLifetime Lifetime { get; set; }
        public NotificationType NotificationType { get; set; }
        public NotificationSeverity NotificationSeverity { get; set; }
        public NotificationInfo()
        {
            Data = new NotificationData();
            Lifetime = NotificationLifetime.Persistent;
            NotificationType = NotificationType.Application;
            NotificationSeverity = NotificationSeverity.Info;

            CreationTime = DateTime.Now;
        }

        public long SetId(long id)
        {
            if (Id.IsNullOrWhiteSpace())
            {
                Id = id.ToString();
                return id;
            }

            return GetId();
        }

        public long GetId()
        {
            return long.Parse(Id);
        }

        public NotificationEventData ToNotificationEventData()
        {
            return new NotificationEventData
            {
                NotificationSeverity = NotificationSeverity,
                CreationTime = CreationTime,
                Data = Data,
                Id = Id,
                Name = Name,
                CateGory = CateGory,
                Lifetime = Lifetime,
                NotificationType = NotificationType,
                TenantId = TenantId
            };
        }
    }
}
