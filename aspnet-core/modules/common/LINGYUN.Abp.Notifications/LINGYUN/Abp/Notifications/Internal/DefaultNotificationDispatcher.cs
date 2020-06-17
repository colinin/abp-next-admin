using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.EventBus.Distributed;

namespace LINGYUN.Abp.Notifications.Internal
{
    /// <summary>
    /// Implements <see cref="INotificationDispatcher"/>.
    /// </summary>
    internal class DefaultNotificationDispatcher : INotificationDispatcher
    {
        /// <summary>
        /// Reference to <see cref="ILogger<DefaultNotificationDispatcher>"/>.
        /// </summary>
        public ILogger<DefaultNotificationDispatcher> Logger { get; set; }
        /// <summary>
        /// Reference to <see cref="IDistributedEventBus"/>.
        /// </summary>
        public IDistributedEventBus DistributedEventBus { get; set; }
        /// <summary>
        /// Reference to <see cref="IBackgroundJobManager"/>.
        /// </summary>
        private readonly IBackgroundJobManager _backgroundJobManager;
        /// <summary>
        /// Reference to <see cref="INotificationStore"/>.
        /// </summary>
        private readonly INotificationStore _notificationStore;
        /// <summary>
        /// Reference to <see cref="INotificationDefinitionManager"/>.
        /// </summary>
        private readonly INotificationDefinitionManager _notificationDefinitionManager;
        /// <summary>
        /// Reference to <see cref="INotificationPublishProviderManager"/>.
        /// </summary>
        private readonly INotificationPublishProviderManager _notificationPublishProviderManager;
        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultNotificationDispatcher"/> class.
        /// </summary>
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

            DistributedEventBus = NullDistributedEventBus.Instance;
            Logger = NullLogger<DefaultNotificationDispatcher>.Instance;
        }
        /// <summary>
        /// 发送通知
        /// </summary>
        /// <param name="notificationName">通知名称</param>
        /// <param name="data">通知数据</param>
        /// <param name="tenantId">租户</param>
        /// <param name="notificationSeverity">级别</param>
        /// <returns></returns>
        public virtual async Task DispatchAsync(NotificationName notificationName, NotificationData data, Guid? tenantId = null, 
            NotificationSeverity notificationSeverity = NotificationSeverity.Info)
        {
            // 获取自定义的通知
            var defineNotification = _notificationDefinitionManager.Get(notificationName.CateGory);

            //// 没有定义的通知,应该也要能发布、订阅,
            //// 比如订单之类的,是以订单编号为通知名称,这是动态的,没法自定义
            //if(defineNotification == null)
            //{
            //    defineNotification = new NotificationDefinition(notificationName.CateGory);
            //}

            var notificationInfo = new NotificationInfo
            {
                CateGory = notificationName.CateGory,
                Name = notificationName.Name,
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
        /// <summary>
        /// 发送通知事件
        /// </summary>
        /// <param name="notificationName"></param>
        /// <param name="data"></param>
        /// <param name="tenantId"></param>
        /// <param name="notificationSeverity"></param>
        /// <returns></returns>
        public virtual async Task DispatchEventAsync(NotificationName notificationName, NotificationData data, Guid? tenantId = null,
            NotificationSeverity notificationSeverity = NotificationSeverity.Info)
        {
            // 获取自定义的通知
            var defineNotification = _notificationDefinitionManager.Get(notificationName.CateGory);

            var notificationEventData = new NotificationEventData
            {
                CateGory = notificationName.CateGory,
                Name = notificationName.Name,
                CreationTime = DateTime.Now,
                NotificationSeverity = notificationSeverity,
                NotificationType = defineNotification.NotificationType,
                TenantId = tenantId,
                Data = data
            };
            // 发布分布式通知事件,让消息中心统一处理
            await DistributedEventBus.PublishAsync(notificationEventData);
        }
        /// <summary>
        /// 发送通知
        /// </summary>
        /// <param name="notification">通知信息</param>
        /// <returns></returns>
        public virtual async Task DispatchAsync(NotificationInfo notification)
        {
            // 获取自定义的通知
            var defineNotification = _notificationDefinitionManager.GetOrNull(notification.Name);

            // 没有定义的通知,应该也要能发布、订阅,
            // 比如订单之类的,是以订单编号为通知名称,这是动态的,没法自定义
            if (defineNotification == null)
            {
                defineNotification = new NotificationDefinition(notification.Name);
            }

            var providers = Enumerable
                  .Reverse(_notificationPublishProviderManager.Providers);

            if (defineNotification.Providers.Any())
            {
                providers = providers.Where(p => defineNotification.Providers.Contains(p.Name));
            }

            await PublishFromProvidersAsync(providers, notification);
        }

        /// <summary>
        /// 指定提供者发布通知
        /// </summary>
        /// <param name="providers">提供者列表</param>
        /// <param name="notificationInfo">通知信息</param>
        /// <returns></returns>
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
        /// <summary>
        /// 发布通知
        /// </summary>
        /// <param name="provider">通知发布者</param>
        /// <param name="notificationInfo">通知信息</param>
        /// <param name="subscriptionUserIdentifiers">订阅用户列表</param>
        /// <returns></returns>
        protected async Task PublishAsync(INotificationPublishProvider provider, NotificationInfo notificationInfo, 
            IEnumerable<UserIdentifier> subscriptionUserIdentifiers)
        {
            try
            {
                Logger.LogDebug($"Sending notification with provider {provider.Name}");

                // 发布
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
