using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LINGYUN.Abp.Notifications.Internal
{
    internal class DefaultNotificationDispatcher : INotificationDispatcher
    {
        public ILogger<DefaultNotificationDispatcher> Logger { get; set; }

        private readonly INotificationStore _notificationStore;
        private readonly INotificationDefinitionManager _notificationDefinitionManager;
        private readonly INotificationPublishProviderManager _notificationPublishProviderManager;
        public DefaultNotificationDispatcher(
            INotificationStore notificationStore,
            INotificationDefinitionManager notificationDefinitionManager,
            INotificationPublishProviderManager notificationPublishProviderManager)
        {
            _notificationStore = notificationStore;
            _notificationDefinitionManager = notificationDefinitionManager;
            _notificationPublishProviderManager = notificationPublishProviderManager;

            Logger = NullLogger<DefaultNotificationDispatcher>.Instance;
        }

        public virtual async Task DispatchAsync(string notificationName, NotificationData data, Guid? tenantId = null, 
            NotificationSeverity notificationSeverity = NotificationSeverity.Info)
        {
            // 获取自定义的通知
            var defineNotification = _notificationDefinitionManager.Get(notificationName);

            var notificationInfo = new NotificationInfo
            {
                Name = defineNotification.Name,
                CreationTime = DateTime.Now,
                NotificationSeverity = notificationSeverity,
                NotificationType = defineNotification.NotificationType,
                TenantId = tenantId,
                Data = data
            };

            var providers = Enumerable
                  .Reverse(_notificationPublishProviderManager.Providers);

            if (defineNotification.Providers.Any())
            {
                providers = providers.Where(p => defineNotification.Providers.Contains(p.Name));
            }

            await PublishFromProvidersAsync(providers, notificationInfo);
        }

        public virtual async Task DispatchAsync(NotificationInfo notification)
        {
            // 获取自定义的通知
            var defineNotification = _notificationDefinitionManager.Get(notification.Name);

            notification.NotificationType = defineNotification.NotificationType;
            notification.Name = defineNotification.Name;

            var providers = Enumerable
                  .Reverse(_notificationPublishProviderManager.Providers);

            if (defineNotification.Providers.Any())
            {
                providers = providers.Where(p => defineNotification.Providers.Contains(p.Name));
            }

            await PublishFromProvidersAsync(providers, notification);
        }

        protected async Task PublishFromProvidersAsync(IEnumerable<INotificationPublishProvider> providers,
            NotificationInfo notificationInfo)
        {
            // 持久化通知
            await _notificationStore.InsertNotificationAsync(notificationInfo);

            // 获取用户订阅列表
            var userSubscriptions = await _notificationStore.GetSubscriptionsAsync(notificationInfo.TenantId, notificationInfo.Name);

            // 持久化用户通知
            var subscriptionUserIdentifiers = userSubscriptions.Select(us => new UserIdentifier(us.UserId, us.UserName));

            await _notificationStore.InsertUserNotificationsAsync(notificationInfo, 
                subscriptionUserIdentifiers.Select(u => u.UserId));

            // 发送通知
            foreach (var provider in providers)
            {
                try
                {
                    await provider.PublishAsync(notificationInfo, subscriptionUserIdentifiers);
                }
                catch(Exception ex)
                {
                    Logger.LogWarning("Send notification error with provider {0}", provider.Name);
                    Logger.LogWarning("Error message:{0}", ex.Message);

                    Logger.LogTrace(ex, "Send notification error with provider {0}", provider.Name);
                }
            }
        }
    }
}
