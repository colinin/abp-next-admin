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
            var notificationData = new NotificationData();
            notificationData.Properties["title"] = notification.Title;
            notificationData.Properties["message"] = notification.Message;
            notificationData.Properties["datetime"] = Clock.Now;
            notificationData.Properties["severity"] = notification.Severity;

            notificationData.Properties.AddIfNotContains(notification.Data);

            await _notificationDispatcher.DispatchAsync("TestApplicationNotofication", notificationData,
                notificationSeverity: notification.Severity);

            // await _notificationDispatcher.DispatcheAsync(notificationInfo);
        }
    }

    public class SendNotification
    {
        public Guid UserId { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public Dictionary<string, object> Data { get; set; } = new Dictionary<string, object>();
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
