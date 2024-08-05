using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace LINGYUN.Abp.Notifications;

public interface IUserNotificationRepository : IBasicRepository<UserNotification, long>
{
    Task<bool> AnyAsync(
        Guid userId, 
        long notificationId,
        CancellationToken cancellationToken = default);

    Task<UserNotificationInfo> GetByIdAsync(
        Guid userId,
        long notificationId,
        CancellationToken cancellationToken = default);

    Task<List<UserNotification>> GetListAsync(
        Guid userId,
        IEnumerable<long> notificationIds,
        CancellationToken cancellationToken = default);

    Task<List<UserNotificationInfo>> GetNotificationsAsync(
        Guid userId, 
        NotificationReadState? readState = null, 
        int maxResultCount = 10,
        CancellationToken cancellationToken = default);

    Task<int> GetCountAsync(
        Guid userId,
        string filter = "",
        NotificationReadState? readState = null,
        CancellationToken cancellationToken = default);

    Task<List<UserNotificationInfo>> GetListAsync(
        Guid userId, 
        string filter = "", 
        string sorting = nameof(Notification.CreationTime),
        NotificationReadState? readState = null,
        int skipCount = 0, 
        int maxResultCount = 10,
        CancellationToken cancellationToken = default);
}
