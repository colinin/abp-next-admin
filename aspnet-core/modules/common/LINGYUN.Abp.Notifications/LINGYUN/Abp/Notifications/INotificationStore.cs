using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LINGYUN.Abp.Notifications
{
    public interface INotificationStore
    {
        Task InsertUserSubscriptionAsync(Guid? tenantId, Guid userId, string notificationName);

        Task InsertUserSubscriptionAsync(Guid? tenantId, IEnumerable<Guid> userIds, string notificationName);

        Task DeleteUserSubscriptionAsync(Guid? tenantId, Guid userId, string notificationName);

        Task<List<NotificationSubscriptionInfo>> GetSubscriptionsAsync(Guid? tenantId, string notificationName);

        Task<List<NotificationSubscriptionInfo>> GetUserSubscriptionsAsync(Guid? tenantId, Guid userId);

        Task<bool> IsSubscribedAsync(Guid? tenantId, Guid userId, string notificationName);

        Task InsertNotificationAsync(NotificationInfo notification);

        Task DeleteNotificationAsync(NotificationInfo notification);

        Task InsertUserNotificationAsync(NotificationInfo notification, Guid userId);

        Task InsertUserNotificationsAsync(NotificationInfo notification, IEnumerable<Guid> userIds);

        Task DeleteUserNotificationAsync(Guid? tenantId, Guid userId, long notificationId);

        Task<NotificationInfo> GetNotificationOrNullAsync(Guid? tenantId, long notificationId);

        Task<List<NotificationInfo>> GetUserNotificationsAsync(Guid? tenantId, Guid userId, NotificationReadState readState = NotificationReadState.UnRead, int maxResultCount = 10);

        Task ChangeUserNotificationReadStateAsync(Guid? tenantId, Guid userId, long notificationId, NotificationReadState readState);
    }
}
