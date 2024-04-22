﻿using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace LINGYUN.Abp.Notifications
{
    public interface INotificationRepository : IBasicRepository<Notification, long>
    {
        Task<Notification> GetByIdAsync(
            long notificationId,
            CancellationToken cancellationToken = default);

        Task<List<Notification>> GetExpritionAsync(
            int batchCount,
            CancellationToken cancellationToken = default);
    }
}
