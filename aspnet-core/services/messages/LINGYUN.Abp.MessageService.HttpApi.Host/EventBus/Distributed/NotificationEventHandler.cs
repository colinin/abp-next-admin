using LINGYUN.Abp.Notifications;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.Uow;

namespace LINGYUN.Abp.MessageService.EventBus.Distributed
{
    /// <summary>
    /// 订阅通知发布事件,统一发布消息
    /// </summary>
    /// <remarks>
    /// 作用在于SignalR客户端只会与一台服务器建立连接,
    /// 只有启用了SignlR服务端的才能真正将消息发布到客户端
    /// </remarks>
    public class NotificationEventHandler : IDistributedEventHandler<NotificationEventData>, ITransientDependency
    {
        /// <summary>
        /// Reference to <see cref="ILogger<DefaultNotificationDispatcher>"/>.
        /// </summary>
        public ILogger<NotificationEventHandler> Logger { get; set; }
        /// <summary>
        /// Reference to <see cref="IBackgroundJobManager"/>.
        /// </summary>
        protected IBackgroundJobManager BackgroundJobManager;
        /// <summary>
        /// Reference to <see cref="INotificationStore"/>.
        /// </summary>
        protected INotificationStore NotificationStore { get; }
        /// <summary>
        /// Reference to <see cref="INotificationPublishProviderManager"/>.
        /// </summary>
        protected INotificationPublishProviderManager NotificationPublishProviderManager { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="NotificationEventHandler"/> class.
        /// </summary>
        public NotificationEventHandler(
            IBackgroundJobManager backgroundJobManager,

            INotificationStore notificationStore,
            INotificationPublishProviderManager notificationPublishProviderManager)
        {
            BackgroundJobManager = backgroundJobManager;

            NotificationStore = notificationStore;
            NotificationPublishProviderManager = notificationPublishProviderManager;

            Logger = NullLogger<NotificationEventHandler>.Instance;
        }

        [UnitOfWork]
        public virtual async Task HandleEventAsync(NotificationEventData eventData)
        {
            var notificationInfo = eventData.ToNotificationInfo();

            var providers = Enumerable
                 .Reverse(NotificationPublishProviderManager.Providers);

            await PublishFromProvidersAsync(providers, notificationInfo);
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
            await NotificationStore.InsertNotificationAsync(notificationInfo);

            Logger.LogDebug($"Gets a list of user subscriptions {notificationInfo.Name}");
            // 获取用户订阅列表
            var userSubscriptions = await NotificationStore.GetSubscriptionsAsync(notificationInfo.TenantId, notificationInfo.Name);

            Logger.LogDebug($"Persistent user notifications {notificationInfo.Name}");
            // 持久化用户通知
            var subscriptionUserIdentifiers = userSubscriptions.Select(us => new UserIdentifier(us.UserId, us.UserName));

            await NotificationStore.InsertUserNotificationsAsync(notificationInfo,
                subscriptionUserIdentifiers.Select(u => u.UserId));

            // 发布通知
            foreach (var provider in providers)
            {
                await PublishAsync(provider, notificationInfo, subscriptionUserIdentifiers);
            }

            if (notificationInfo.Lifetime == NotificationLifetime.OnlyOne)
            {
                // 一次性通知在发送完成后就取消用户订阅
                await NotificationStore.DeleteAllUserSubscriptionAsync(notificationInfo.TenantId,
                    notificationInfo.Name);
            }
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
                await BackgroundJobManager.EnqueueAsync(
                    new NotificationPublishJobArgs(notificationInfo.GetId(),
                    provider.GetType().AssemblyQualifiedName,
                    subscriptionUserIdentifiers.ToList(),
                    notificationInfo.TenantId));
            }
        }
    }
}
