using LINGYUN.Abp.Notifications;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc;

namespace LINGYUN.Abp.MessageService.Controllers
{
    [Route("api/app/notifications")]
    public class NotificationController : AbpController
    {
        private readonly INotificationPublisher _notificationPublisher;
        public NotificationController(
            INotificationPublisher notificationPublisher)
        {
            _notificationPublisher = notificationPublisher;
        }

        [HttpPost]
        [Route("Send")]
        public async Task SendNofitication([FromForm] SendNotification notification)
        {
            var notificationInfo = new NotificationInfo
            {
                TenantId = null,
                NotificationSeverity = NotificationSeverity.Success,
                NotificationType = NotificationType.Application,
                Id = 164589598456654164,
                Name = "TestApplicationNotofication"
            };
            notificationInfo.Data.Properties["Title"] = notification.Title;
            notificationInfo.Data.Properties["Message"] = notification.Message;
            notificationInfo.Data.Properties["DateTime"] = notification.DateTime;
            notificationInfo.Data.Properties["Severity"] = notification.Severity;

            await _notificationPublisher.PublishAsync(notificationInfo, new List<Guid>() { notification.UserId });
        }
    }

    public class SendNotification
    {
        public Guid UserId { get; set; }
        public string Title { get; set; }
        public DateTime DateTime { get; set; } = DateTime.Now;
        public string Message { get; set; }
        public NotificationSeverity Severity { get; set; } = NotificationSeverity.Success;
    }

    public class TestApplicationNotificationData : NotificationData
    {
        public object Message
        {
            get { return this[nameof(Message)]; }
            set
            {
                Properties[nameof(Message)] = value;
            }
        }
    }
}
