using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace LINGYUN.Abp.Notifications
{
    public interface INotificationStore
    {
        Task InsertUserSubscriptionAsync(
            Guid? tenantId, 
            UserIdentifier identifier, 
            string notificationName,
            CancellationToken cancellationToken = default);

        Task InsertUserSubscriptionAsync(
            Guid? tenantId,
            IEnumerable<UserIdentifier> identifiers,
            string notificationName,
            CancellationToken cancellationToken = default);

        Task DeleteUserSubscriptionAsync(
            Guid? tenantId,
            Guid userId, 
            string notificationName,
            CancellationToken cancellationToken = default);

        Task DeleteAllUserSubscriptionAsync(
            Guid? tenantId, 
            string notificationName,
            CancellationToken cancellationToken = default);

        Task DeleteUserSubscriptionAsync(
            Guid? tenantId, 
            IEnumerable<UserIdentifier> identifiers, 
            string notificationName,
            CancellationToken cancellationToken = default);

        Task<List<NotificationSubscriptionInfo>> GetUserSubscriptionsAsync(
            Guid? tenantId, 
            string notificationName, 
            IEnumerable<UserIdentifier> identifiers = null,
            CancellationToken cancellationToken = default);

        Task<List<NotificationSubscriptionInfo>> GetUserSubscriptionsAsync(
            Guid? tenantId, 
            Guid userId,
            CancellationToken cancellationToken = default);

        Task<List<NotificationSubscriptionInfo>> GetUserSubscriptionsAsync(
            Guid? tenantId, 
            string userName,
            CancellationToken cancellationToken = default);

        Task<bool> IsSubscribedAsync(
            Guid? tenantId, 
            Guid userId, 
            string notificationName,
            CancellationToken cancellationToken = default);

        Task InsertNotificationAsync(
            NotificationInfo notification,
            CancellationToken cancellationToken = default);

        Task DeleteNotificationAsync(
            NotificationInfo notification,
            CancellationToken cancellationToken = default);

        Task DeleteNotificationAsync(
            int batchCount,
            CancellationToken cancellationToken = default);

        Task InsertUserNotificationAsync(
            NotificationInfo notification, 
            Guid userId,
            CancellationToken cancellationToken = default);

        Task InsertUserNotificationsAsync(
            NotificationInfo notification, 
            IEnumerable<Guid> userIds,
            CancellationToken cancellationToken = default);

        Task DeleteUserNotificationAsync(
            Guid? tenantId, 
            Guid userId, 
            long notificationId,
            CancellationToken cancellationToken = default);

        Task<NotificationInfo> GetNotificationOrNullAsync(
            Guid? tenantId, 
            long notificationId,
            CancellationToken cancellationToken = default);

        Task<List<NotificationInfo>> GetUserNotificationsAsync(
            Guid? tenantId, 
            Guid userId, 
            NotificationReadState? readState = null, 
            int maxResultCount = 10,
            CancellationToken cancellationToken = default);

        Task<int> GetUserNotificationsCountAsync(
            Guid? tenantId,
            Guid userId,
            string filter = "",
            NotificationReadState? readState = null,
            CancellationToken cancellationToken = default);

        Task<List<NotificationInfo>> GetUserNotificationsAsync(
            Guid? tenantId,
            Guid userId,
            string filter = "",
            string sorting = nameof(NotificationInfo.CreationTime),
            NotificationReadState? readState = null,
            int skipCount = 1,
            int maxResultCount = 10,
            CancellationToken cancellationToken = default);

        Task ChangeUserNotificationReadStateAsync(
            Guid? tenantId, 
            Guid userId, 
            long notificationId, 
            NotificationReadState readState,
            CancellationToken cancellationToken = default);
    }
}
