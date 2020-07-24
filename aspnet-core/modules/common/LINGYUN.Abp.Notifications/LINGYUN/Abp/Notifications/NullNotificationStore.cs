using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.Notifications
{
    [Dependency(Microsoft.Extensions.DependencyInjection.ServiceLifetime.Singleton, TryRegister = true)]
    [ExposeServices(typeof(INotificationStore))]
    public class NullNotificationStore : INotificationStore
    {
        public Task ChangeUserNotificationReadStateAsync(Guid? tenantId, Guid userId, long notificationId, NotificationReadState readState)
        {
            return Task.CompletedTask;
        }

        public Task DeleteAllUserSubscriptionAsync(Guid? tenantId, string notificationName)
        {
            return Task.CompletedTask;
        }

        public Task DeleteNotificationAsync(NotificationInfo notification)
        {
            return Task.CompletedTask;
        }

        public Task DeleteNotificationAsync(int batchCount)
        {
            return Task.CompletedTask;
        }

        public Task DeleteUserNotificationAsync(Guid? tenantId, Guid userId, long notificationId)
        {
            return Task.CompletedTask;
        }

        public Task DeleteUserSubscriptionAsync(Guid? tenantId, Guid userId, string notificationName)
        {
            return Task.CompletedTask;
        }

        public Task DeleteUserSubscriptionAsync(Guid? tenantId, IEnumerable<UserIdentifier> identifiers, string notificationName)
        {
            return Task.CompletedTask;
        }

        public Task<NotificationInfo> GetNotificationOrNullAsync(Guid? tenantId, long notificationId)
        {
            return Task.FromResult(new NotificationInfo());
        }

        public Task<List<NotificationSubscriptionInfo>> GetSubscriptionsAsync(Guid? tenantId, string notificationName)
        {
            return Task.FromResult(new List<NotificationSubscriptionInfo>());
        }

        public Task<List<NotificationInfo>> GetUserNotificationsAsync(Guid? tenantId, Guid userId, NotificationReadState readState = NotificationReadState.UnRead, int maxResultCount = 10)
        {
            return Task.FromResult(new List<NotificationInfo>());
        }

        public Task<List<NotificationSubscriptionInfo>> GetUserSubscriptionsAsync(Guid? tenantId, Guid userId)
        {
            return Task.FromResult(new List<NotificationSubscriptionInfo>());
        }

        public Task<List<NotificationSubscriptionInfo>> GetUserSubscriptionsAsync(Guid? tenantId, string userName)
        {
            return Task.FromResult(new List<NotificationSubscriptionInfo>());
        }

        public Task InsertNotificationAsync(NotificationInfo notification)
        {
            return Task.CompletedTask;
        }

        public Task InsertUserNotificationAsync(NotificationInfo notification, Guid userId)
        {
            return Task.CompletedTask;
        }

        public Task InsertUserNotificationsAsync(NotificationInfo notification, IEnumerable<Guid> userIds)
        {
            return Task.CompletedTask;
        }

        public Task InsertUserSubscriptionAsync(Guid? tenantId, UserIdentifier identifier, string notificationName)
        {
            return Task.CompletedTask;
        }

        public Task InsertUserSubscriptionAsync(Guid? tenantId, IEnumerable<UserIdentifier> identifiers, string notificationName)
        {
            return Task.CompletedTask;
        }

        public Task<bool> IsSubscribedAsync(Guid? tenantId, Guid userId, string notificationName)
        {
            return Task.FromResult(false);
        }
    }
}
