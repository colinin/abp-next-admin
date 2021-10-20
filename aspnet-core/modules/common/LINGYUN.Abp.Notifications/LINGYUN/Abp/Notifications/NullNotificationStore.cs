using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.Notifications
{
    [Dependency(TryRegister = true)]
    public class NullNotificationStore : INotificationStore, ISingletonDependency
    {
        public Task ChangeUserNotificationReadStateAsync(
            Guid? tenantId, 
            Guid userId, 
            long notificationId, 
            NotificationReadState readState,
            CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }

        public Task DeleteAllUserSubscriptionAsync(
            Guid? tenantId, 
            string notificationName,
            CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }

        public Task DeleteNotificationAsync(
            NotificationInfo notification,
            CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }

        public Task DeleteNotificationAsync(
            int batchCount,
            CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }

        public Task DeleteUserNotificationAsync(
            Guid? tenantId,
            Guid userId,
            long notificationId,
            CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }

        public Task DeleteUserSubscriptionAsync(
            Guid? tenantId,
            Guid userId,
            string notificationName,
            CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }

        public Task DeleteUserSubscriptionAsync(
            Guid? tenantId, 
            IEnumerable<UserIdentifier> identifiers, 
            string notificationName,
            CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }

        public Task<NotificationInfo> GetNotificationOrNullAsync(
            Guid? tenantId, 
            long notificationId,
            CancellationToken cancellationToken = default)
        {
            return Task.FromResult(new NotificationInfo());
        }

        public Task<List<NotificationSubscriptionInfo>> GetUserSubscriptionsAsync(
            Guid? tenantId, 
            string notificationName,
            IEnumerable<UserIdentifier> identifiers,
            CancellationToken cancellationToken = default)
        {
            return Task.FromResult(new List<NotificationSubscriptionInfo>());
        }

        public Task<List<NotificationInfo>> GetUserNotificationsAsync(
            Guid? tenantId, 
            Guid userId,
            NotificationReadState? readState = null,
            int maxResultCount = 10,
            CancellationToken cancellationToken = default)
        {
            return Task.FromResult(new List<NotificationInfo>());
        }

        public Task<int> GetUserNotificationsCountAsync(
            Guid? tenantId,
            Guid userId,
            string filter = "",
            NotificationReadState? readState = null,
            CancellationToken cancellationToken = default)
        {
            return Task.FromResult(0);
        }

        public Task<List<NotificationInfo>> GetUserNotificationsAsync(
            Guid? tenantId,
            Guid userId,
            string filter = "",
            string sorting = nameof(NotificationInfo.CreationTime),
            NotificationReadState? readState = null,
            int skipCount = 1,
            int maxResultCount = 10,
            CancellationToken cancellationToken = default)
        {
            return Task.FromResult(new List<NotificationInfo>());
        }

        public Task<List<NotificationSubscriptionInfo>> GetUserSubscriptionsAsync(
            Guid? tenantId, 
            Guid userId,
            CancellationToken cancellationToken = default)
        {
            return Task.FromResult(new List<NotificationSubscriptionInfo>());
        }

        public Task<List<NotificationSubscriptionInfo>> GetUserSubscriptionsAsync(
            Guid? tenantId,
            string userName,
            CancellationToken cancellationToken = default)
        {
            return Task.FromResult(new List<NotificationSubscriptionInfo>());
        }

        public Task InsertNotificationAsync(
            NotificationInfo notification,
            CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }

        public Task InsertUserNotificationAsync(
            NotificationInfo notification,
            Guid userId,
            CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }

        public Task InsertUserNotificationsAsync(
            NotificationInfo notification, 
            IEnumerable<Guid> userIds,
            CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }

        public Task InsertUserSubscriptionAsync(
            Guid? tenantId, 
            UserIdentifier identifier,
            string notificationName,
            CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }

        public Task InsertUserSubscriptionAsync(
            Guid? tenantId,
            IEnumerable<UserIdentifier> identifiers, 
            string notificationName,
            CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }

        public Task<bool> IsSubscribedAsync(
            Guid? tenantId, 
            Guid userId, 
            string notificationName,
            CancellationToken cancellationToken = default)
        {
            return Task.FromResult(false);
        }
    }
}
