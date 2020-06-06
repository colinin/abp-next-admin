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
        private readonly INotificationDispatcher _notificationDispatcher;
        public NotificationController(
            INotificationDispatcher notificationDispatcher)
        {
            _notificationDispatcher = notificationDispatcher;
        }

        [HttpPost]
        [Route("Send")]
        public async Task SendNofitication([FromForm] SendNotification notification)
        {
            var notificationInfo = new NotificationInfo
            {
                TenantId = null,
                NotificationSeverity = notification.Severity,
                NotificationType = NotificationType.Application,
                Id = new Random().Next(int.MinValue, int.MaxValue),
                Name = "TestApplicationNotofication",
                CreationTime = Clock.Now
            };
            notificationInfo.Data.Properties["id"] = notificationInfo.Id.ToString();
            notificationInfo.Data.Properties["title"] = notification.Title;
            notificationInfo.Data.Properties["message"] = notification.Message;
            notificationInfo.Data.Properties["datetime"] = Clock.Now;
            notificationInfo.Data.Properties["severity"] = notification.Severity;

            await _notificationDispatcher.DispatcheAsync(notificationInfo);
        }
    }

    public class SendNotification
    {
        public Guid UserId { get; set; }
        public string Title { get; set; }
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
