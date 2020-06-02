using LINGYUN.Abp.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.MessageService.Notifications
{
    public class NotificationDispatcher : INotificationDispatcher, ITransientDependency
    {
        protected INotificationStore NotificationStore { get; }
        protected INotificationPublisher NotificationPublisher { get; }

        public NotificationDispatcher(
            INotificationStore notificationStore,
            INotificationPublisher notificationPublisher)
        {
            NotificationStore = notificationStore;
            NotificationPublisher = notificationPublisher;
        }

        public virtual async Task DispatcheAsync(NotificationInfo notification)
        {
            var subscribes = await NotificationStore.GetSubscriptionsAsync(notification.TenantId, notification.Name);
            foreach (var subscribe in subscribes)
            {
                await NotificationStore.InsertUserNotificationAsync(notification, subscribe.UserId);
            }

            var subscribeUsers = subscribes.Select(s => s.UserId);
            await NotifyAsync(notification, subscribeUsers);
        }

        protected virtual async Task NotifyAsync(NotificationInfo notification, IEnumerable<Guid> userIds)
        {
            await NotificationPublisher.PublishAsync(notification, userIds);
        }
    }
}
