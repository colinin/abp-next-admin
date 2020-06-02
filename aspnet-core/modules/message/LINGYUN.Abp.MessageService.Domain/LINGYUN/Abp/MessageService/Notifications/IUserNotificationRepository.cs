using LINGYUN.Abp.Notifications;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace LINGYUN.Abp.MessageService.Notifications
{
    public interface IUserNotificationRepository : IBasicRepository<UserNotification, long>
    {
        Task InsertUserNotificationsAsync(IEnumerable<UserNotification> userNotifications);

        Task<UserNotification> GetByIdAsync(Guid userId, long notificationId);

        Task<List<Notification>> GetNotificationsAsync(Guid userId, NotificationReadState readState = NotificationReadState.UnRead, int maxResultCount = 10);

        Task ChangeUserNotificationReadStateAsync(Guid userId, long notificationId, NotificationReadState readState);
    }
}
