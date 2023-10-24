using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace LINGYUN.Abp.Notifications
{
    /// <summary>
    /// 通知订阅管理器
    /// </summary>
    public interface INotificationSubscriptionManager
    {
        /// <summary>
        /// 是否已订阅
        /// </summary>
        /// <param name="tenantId">租户</param>
        /// <param name="userId">用户标识</param>
        /// <param name="notificationName">通知名称</param>
        /// <returns></returns>
        Task<bool> IsSubscribedAsync(
            Guid? tenantId, 
            Guid userId, 
            string notificationName,
            CancellationToken cancellationToken = default);
        /// <summary>
        /// 订阅通知
        /// </summary>
        /// <param name="tenantId">租户</param>
        /// <param name="identifier">用户标识</param>
        /// <param name="notificationName">通知名称</param>
        /// <returns></returns>
        Task SubscribeAsync(
            Guid? tenantId,
            UserIdentifier identifier, 
            string notificationName,
            CancellationToken cancellationToken = default);
        /// <summary>
        /// 订阅通知
        /// </summary>
        /// <param name="tenantId">租户</param>
        /// <param name="identifiers">用户标识列表</param>
        /// <param name="notificationName">通知名称</param>
        /// <returns></returns>
        Task SubscribeAsync(
            Guid? tenantId, 
            IEnumerable<UserIdentifier> identifiers,
            string notificationName,
            CancellationToken cancellationToken = default);
        /// <summary>
        /// 取消所有用户订阅
        /// </summary>
        /// <param name="tenantId">租户</param>
        /// <param name="notificationName">通知名称</param>
        /// <returns></returns>
        Task UnsubscribeAllAsync(
            Guid? tenantId, 
            string notificationName,
            CancellationToken cancellationToken = default);
        /// <summary>
        /// 取消订阅
        /// </summary>
        /// <param name="tenantId">租户</param>
        /// <param name="identifier">用户标识</param>
        /// <param name="notificationName">通知名称</param>
        /// <returns></returns>
        Task UnsubscribeAsync(
            Guid? tenantId, 
            UserIdentifier identifier, 
            string notificationName,
            CancellationToken cancellationToken = default);
        /// <summary>
        /// 取消订阅
        /// </summary>
        /// <param name="tenantId">租户</param>
        /// <param name="identifiers">用户标识列表</param>
        /// <param name="notificationName">通知名称</param>
        /// <returns></returns>
        Task UnsubscribeAsync(
            Guid? tenantId, 
            IEnumerable<UserIdentifier> identifiers,
            string notificationName,
            CancellationToken cancellationToken = default);
        /// <summary>
        /// 获取通知被订阅用户列表
        /// </summary>
        /// <param name="tenantId">租户</param>
        /// <param name="notificationName">通知名称</param>
        /// <param name="identifiers">需要检查的用户列表</param>
        /// <returns></returns>
        Task<List<NotificationSubscriptionInfo>> GetUsersSubscriptionsAsync(
            Guid? tenantId, 
            string notificationName, 
            IEnumerable<UserIdentifier> identifiers = null,
            CancellationToken cancellationToken = default);
        /// <summary>
        /// 获取用户订阅列表
        /// </summary>
        /// <param name="tenantId">租户</param>
        /// <param name="userId">用户标识</param>
        /// <returns></returns>
        Task<List<NotificationSubscriptionInfo>> GetUserSubscriptionsAsync(
            Guid? tenantId, 
            Guid userId,
            CancellationToken cancellationToken = default);
        /// <summary>
        /// 获取用户订阅列表
        /// </summary>
        /// <param name="tenantId">租户</param>
        /// <param name="userName">用户名</param>
        /// <returns></returns>
        Task<List<NotificationSubscriptionInfo>> GetUserSubscriptionsAsync(
            Guid? tenantId, 
            string userName,
            CancellationToken cancellationToken = default);
    }
}
