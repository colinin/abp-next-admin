using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.Notifications.Internal
{
    internal class NotificationSubscriptionManager : INotificationSubscriptionManager, ITransientDependency
    {
        private readonly INotificationStore _store;

        public NotificationSubscriptionManager(
            INotificationStore store)
        {
            _store = store;
        }

        public virtual async Task<List<NotificationSubscriptionInfo>> GetSubscriptionsAsync(Guid? tenantId, string notificationName)
        {
            return await _store.GetSubscriptionsAsync(tenantId, notificationName);
        }

        public virtual async Task<List<NotificationSubscriptionInfo>> GetUserSubscriptionsAsync(Guid? tenantId, Guid userId)
        {
            return await _store.GetUserSubscriptionsAsync(tenantId, userId);
        }

        public virtual async Task<List<NotificationSubscriptionInfo>> GetUserSubscriptionsAsync(Guid? tenantId, string userName)
        {
            return await _store.GetUserSubscriptionsAsync(tenantId, userName);
        }

        public virtual async Task<bool> IsSubscribedAsync(Guid? tenantId, Guid userId, string notificationName)
        {
            return await _store.IsSubscribedAsync(tenantId, userId, notificationName);
        }

        public virtual async Task SubscribeAsync(Guid? tenantId, UserIdentifier identifier, string notificationName)
        {
            if (await IsSubscribedAsync(tenantId, identifier.UserId, notificationName))
            {
                return;
            }
            await _store.InsertUserSubscriptionAsync(tenantId, identifier, notificationName);
        }

        public virtual async Task SubscribeAsync(Guid? tenantId, IEnumerable<UserIdentifier> identifiers, string notificationName)
        {
            foreach(var identifier in identifiers)
            {
                await SubscribeAsync(tenantId, identifier, notificationName);
            }
        }

        public virtual async Task UnsubscribeAsync(Guid? tenantId, UserIdentifier identifier, string notificationName)
        {
            await _store.DeleteUserSubscriptionAsync(tenantId, identifier.UserId, notificationName);
        }

        public virtual async Task UnsubscribeAllAsync(Guid? tenantId, string notificationName)
        {
            await _store.DeleteAllUserSubscriptionAsync(tenantId, notificationName);
        }

        public virtual async Task UnsubscribeAsync(Guid? tenantId, IEnumerable<UserIdentifier> identifiers, string notificationName)
        {
            await _store.DeleteUserSubscriptionAsync(tenantId, identifiers, notificationName);
        }
    }
}
