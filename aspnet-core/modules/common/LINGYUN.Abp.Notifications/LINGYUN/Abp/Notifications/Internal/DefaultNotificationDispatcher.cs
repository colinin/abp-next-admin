using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.BackgroundJobs;

namespace LINGYUN.Abp.Notifications.Internal
{
    internal class DefaultNotificationDispatcher : INotificationDispatcher
    {
        public ILogger<DefaultNotificationDispatcher> Logger { get; set; }

        private readonly IBackgroundJobManager _backgroundJobManager;

        private readonly INotificationStore _notificationStore;
        private readonly INotificationDefinitionManager _notificationDefinitionManager;
        private readonly INotificationPublishProviderManager _notificationPublishProviderManager;
        public DefaultNotificationDispatcher(
            IBackgroundJobManager backgroundJobManager,

            INotificationStore notificationStore,
            INotificationDefinitionManager notificationDefinitionManager,
            INotificationPublishProviderManager notificationPublishProviderManager)
        {
            _backgroundJobManager = backgroundJobManager;

            _notificationStore = notificationStore;
            _notificationDefinitionManager = notificationDefinitionManager;
            _notificationPublishProviderManager = notificationPublishProviderManager;

            Logger = NullLogger<DefaultNotificationDispatcher>.Instance;
        }

        public virtual async Task DispatchAsync(string notificationName, NotificationData data, Guid? tenantId = null, 
            NotificationSeverity notificationSeverity = NotificationSeverity.Info)
        {
            // 获取自定义的通知
            var defineNotification = _notificationDefinitionManager.GetOrNull(notificationName);

            // 没有定义的通知,应该也要能发布、订阅,
            // 比如订单之类的,是以订单编号为通知名称,这是动态的,没法自定义
            if(defineNotification == null)
            {
                defineNotification = new NotificationDefinition(notificationName);
            }

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
            Logger.LogDebug($"Persistent notification {notificationInfo.Name}");
            // 持久化通知
            await _notificationStore.InsertNotificationAsync(notificationInfo);

            Logger.LogDebug($"Gets a list of user subscriptions {notificationInfo.Name}");
            // 获取用户订阅列表
            var userSubscriptions = await _notificationStore.GetSubscriptionsAsync(notificationInfo.TenantId, notificationInfo.Name);

            Logger.LogDebug($"Persistent user notifications {notificationInfo.Name}");
            // 持久化用户通知
            var subscriptionUserIdentifiers = userSubscriptions.Select(us => new UserIdentifier(us.UserId, us.UserName));

            await _notificationStore.InsertUserNotificationsAsync(notificationInfo, 
                subscriptionUserIdentifiers.Select(u => u.UserId));

            // 发布通知
            foreach (var provider in providers)
            {
                await PublishAsync(provider, notificationInfo, subscriptionUserIdentifiers);
            }

            // TODO: 需要计算队列大小,根据情况是否需要并行发布消息
            //Parallel.ForEach(providers, async (provider) =>
            //{
            //    await PublishAsync(provider, notificationInfo, subscriptionUserIdentifiers);
            //});
        }

        protected async Task PublishAsync(INotificationPublishProvider provider, NotificationInfo notificationInfo, 
            IEnumerable<UserIdentifier> subscriptionUserIdentifiers)
        {
            try
            {
                Logger.LogDebug($"Sending notification with provider {provider.Name}");

                await provider.PublishAsync(notificationInfo, subscriptionUserIdentifiers);

                Logger.LogDebug($"Send notification {notificationInfo.Name} with provider {provider.Name} was successful");
            }
            catch (Exception ex)
            {
                Logger.LogWarning($"Send notification error with provider {provider.Name}");
                Logger.LogWarning($"Error message:{ex.Message}");

                Logger.LogTrace(ex, $"Send notification error with provider { provider.Name}");

                Logger.LogDebug($"Send notification error, notification {notificationInfo.Name} entry queue");
                // 发送失败的消息进入后台队列
                await _backgroundJobManager.EnqueueAsync(
                    new NotificationPublishJobArgs(notificationInfo.GetId(), 
                    provider.GetType().AssemblyQualifiedName,
                    subscriptionUserIdentifiers.ToList(),
                    notificationInfo.TenantId));
            }
        }
    }
}
