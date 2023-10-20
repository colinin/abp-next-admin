using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace LINGYUN.Abp.Notifications.EntityFrameworkCore;

public class EfCoreNotificationRepository : EfCoreRepository<INotificationsDbContext, Notification, long>,
        INotificationRepository, ITransientDependency
{
    public EfCoreNotificationRepository(
        IDbContextProvider<INotificationsDbContext> dbContextProvider)
        : base(dbContextProvider)
    {
    }

    public async Task<List<Notification>> GetExpritionAsync(
        int batchCount,
        CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .Where(x => x.ExpirationTime < DateTime.Now)
            .OrderBy(x => x.ExpirationTime)
            .Take(batchCount)
            .ToListAsync(GetCancellationToken(cancellationToken));
    }

    public async Task<Notification> GetByIdAsync(
        long notificationId,
        CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync()).Where(x => x.NotificationId.Equals(notificationId))
            .FirstOrDefaultAsync(GetCancellationToken(cancellationToken));
    }
}
