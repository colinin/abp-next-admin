using System.Linq;
using System.Threading.Tasks;

namespace LINGYUN.Abp.Notifications.Internal
{
    internal class DefaultNotificationDispatcher : INotificationDispatcher
    {
        private readonly INotificationStore _notificationStore;
        private readonly INotificationPublisher _notificationPublisher;
        public DefaultNotificationDispatcher(
            INotificationStore notificationStore,
            INotificationPublisher notificationPublisher)
        {
            _notificationStore = notificationStore;
            _notificationPublisher = notificationPublisher;
        }

        public async Task DispatcheAsync(NotificationInfo notification)
        {
            // 持久化通知
            await _notificationStore.InsertNotificationAsync(notification);

            // 获取用户订阅列表
            var userSubscriptions = await _notificationStore.GetSubscriptionsAsync(notification.TenantId, notification.Name);

            // 持久化用户通知
            var subscriptionUserIds = userSubscriptions.Select(us => us.UserId);
            await _notificationStore.InsertUserNotificationsAsync(notification, subscriptionUserIds);

            // 发布用户通知
            await _notificationPublisher.PublishAsync(notification, subscriptionUserIds);
        }
    }
}
