using System;
using System.Collections.Generic;
using System.Threading;
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

        public virtual async Task<List<NotificationSubscriptionInfo>> GetUsersSubscriptionsAsync(
            Guid? tenantId,
            string notificationName, 
            IEnumerable<UserIdentifier> identifiers = null,
            CancellationToken cancellationToken = default)
        {
            return await _store.GetUserSubscriptionsAsync(tenantId, notificationName, identifiers, cancellationToken);
        }

        public virtual async Task<List<NotificationSubscriptionInfo>> GetUserSubscriptionsAsync(
            Guid? tenantId, 
            Guid userId,
            CancellationToken cancellationToken = default)
        {
            return await _store.GetUserSubscriptionsAsync(tenantId, userId, cancellationToken);
        }

        public virtual async Task<List<NotificationSubscriptionInfo>> GetUserSubscriptionsAsync(
            Guid? tenantId, 
            string userName,
            CancellationToken cancellationToken = default)
        {
            return await _store.GetUserSubscriptionsAsync(tenantId, userName, cancellationToken);
        }

        public virtual async Task<bool> IsSubscribedAsync(
            Guid? tenantId, 
            Guid userId, 
            string notificationName,
            CancellationToken cancellationToken = default)
        {
            return await _store.IsSubscribedAsync(tenantId, userId, notificationName, cancellationToken);
        }

        public virtual async Task SubscribeAsync(
            Guid? tenantId,
            UserIdentifier identifier,
            string notificationName,
            CancellationToken cancellationToken = default)
        {
            if (await IsSubscribedAsync(tenantId, identifier.UserId, notificationName, cancellationToken))
            {
                return;
            }
            await _store.InsertUserSubscriptionAsync(tenantId, identifier, notificationName, cancellationToken);
        }

        public virtual async Task SubscribeAsync(
            Guid? tenantId, 
            IEnumerable<UserIdentifier> identifiers, 
            string notificationName,
            CancellationToken cancellationToken = default)
        {
            foreach(var identifier in identifiers)
            {
                await SubscribeAsync(tenantId, identifier, notificationName, cancellationToken);
            }
        }

        public virtual async Task UnsubscribeAsync(
            Guid? tenantId, 
            UserIdentifier identifier, 
            string notificationName,
            CancellationToken cancellationToken = default)
        {
            await _store.DeleteUserSubscriptionAsync(tenantId, identifier.UserId, notificationName, cancellationToken);
        }

        public virtual async Task UnsubscribeAllAsync(
            Guid? tenantId, 
            string notificationName,
            CancellationToken cancellationToken = default)
        {
            await _store.DeleteAllUserSubscriptionAsync(tenantId, notificationName, cancellationToken);
        }

        public virtual async Task UnsubscribeAsync(
            Guid? tenantId,
            IEnumerable<UserIdentifier> identifiers,
            string notificationName,
            CancellationToken cancellationToken = default)
        {
            await _store.DeleteUserSubscriptionAsync(tenantId, identifiers, notificationName, cancellationToken);
        }
    }
}
