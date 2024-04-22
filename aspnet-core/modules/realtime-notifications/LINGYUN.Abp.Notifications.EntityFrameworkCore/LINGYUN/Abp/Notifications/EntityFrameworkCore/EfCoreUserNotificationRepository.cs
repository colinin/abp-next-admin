using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace LINGYUN.Abp.Notifications.EntityFrameworkCore;

public class EfCoreUserNotificationRepository : EfCoreRepository<INotificationsDbContext, UserNotification, long>,
        IUserNotificationRepository, ITransientDependency
{
    public EfCoreUserNotificationRepository(
        IDbContextProvider<INotificationsDbContext> dbContextProvider)
        : base(dbContextProvider)
    {
    }

    public async virtual Task<bool> AnyAsync(
        Guid userId,
        long notificationId,
        CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .AnyAsync(x => x.NotificationId.Equals(notificationId) && x.UserId.Equals(userId),
                GetCancellationToken(cancellationToken));
    }

    public async virtual Task<UserNotificationInfo> GetByIdAsync(
        Guid userId,
        long notificationId,
        CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();
        var userNotifilerQuery = dbContext.Set<UserNotification>()
            .Where(x => x.UserId == userId);

        var notificationQuery = dbContext.Set<Notification>();

        var notifilerQuery = from un in userNotifilerQuery
                             join n in dbContext.Set<Notification>()
                                     on un.NotificationId equals n.NotificationId
                             where n.NotificationId.Equals(notificationId)
                             select new UserNotificationInfo
                             {
                                 Id = n.NotificationId,
                                 TenantId = n.TenantId,
                                 Name = n.NotificationName,
                                 ExtraProperties = n.ExtraProperties,
                                 CreationTime = n.CreationTime,
                                 NotificationTypeName = n.NotificationTypeName,
                                 Severity = n.Severity,
                                 State = un.ReadStatus,
                                 Type = n.Type,
                                 ContentType = n.ContentType
                             };

        return await notifilerQuery
            .FirstOrDefaultAsync(GetCancellationToken(cancellationToken));
    }

    public async virtual Task<List<UserNotification>> GetListAsync(
        Guid userId,
        IEnumerable<long> notificationIds,
        CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .Where(x => x.UserId.Equals(userId) && notificationIds.Contains(x.NotificationId))
            .ToListAsync(GetCancellationToken(cancellationToken));
    }

    public async virtual Task<List<UserNotificationInfo>> GetNotificationsAsync(
        Guid userId,
        NotificationReadState? readState = null,
        int maxResultCount = 10,
        CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();
        var userNotifilerQuery = dbContext.Set<UserNotification>()
                                          .Where(x => x.UserId == userId)
                                          .WhereIf(readState.HasValue, x => x.ReadStatus == readState.Value);

        var notifilerQuery = from un in userNotifilerQuery
                             join n in dbContext.Set<Notification>()
                                     on un.NotificationId equals n.NotificationId
                             select new UserNotificationInfo
                             {
                                 Id = n.NotificationId,
                                 TenantId = n.TenantId,
                                 Name = n.NotificationName,
                                 ExtraProperties = n.ExtraProperties,
                                 CreationTime = n.CreationTime,
                                 NotificationTypeName = n.NotificationTypeName,
                                 Severity = n.Severity,
                                 State = un.ReadStatus,
                                 Type = n.Type,
                                 ContentType = n.ContentType
                             };

        return await notifilerQuery
            .OrderBy(nameof(Notification.CreationTime) + " DESC")
            .Take(maxResultCount)
            .AsNoTracking()
            .ToListAsync(GetCancellationToken(cancellationToken));
    }

    public async virtual Task<int> GetCountAsync(
        Guid userId,
        string filter = "",
        NotificationReadState? readState = null,
        CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();
        var notifilerQuery = from un in dbContext.Set<UserNotification>()
                             join n in dbContext.Set<Notification>()
                                on un.NotificationId equals n.NotificationId
                             where un.UserId == userId
                             select new UserNotificationInfo
                             {
                                 Id = n.NotificationId,
                                 TenantId = n.TenantId,
                                 Name = n.NotificationName,
                                 ExtraProperties = n.ExtraProperties,
                                 CreationTime = n.CreationTime,
                                 NotificationTypeName = n.NotificationTypeName,
                                 Severity = n.Severity,
                                 State = un.ReadStatus,
                                 Type = n.Type,
                                 ContentType = n.ContentType
                             };

        return await notifilerQuery
            .WhereIf(readState.HasValue, x => x.State == readState.Value)
            .WhereIf(!filter.IsNullOrWhiteSpace(), nf =>
                nf.Name.Contains(filter) ||
                nf.NotificationTypeName.Contains(filter))
            .CountAsync(GetCancellationToken(cancellationToken));
    }

    public async virtual Task<List<UserNotificationInfo>> GetListAsync(
        Guid userId,
        string filter = "",
        string sorting = nameof(Notification.CreationTime),
        NotificationReadState? readState = null,
        int skipCount = 1,
        int maxResultCount = 10,
        CancellationToken cancellationToken = default)
    {
        if (sorting.IsNullOrWhiteSpace())
        {
            sorting = $"{nameof(Notification.CreationTime)} DESC";
        }
        var dbContext = await GetDbContextAsync();
        //var userNotifilerQuery = dbContext.Set<UserNotification>()
        //    .Where(x => x.UserId == userId)
        //    .WhereIf(readState.HasValue, x => x.ReadStatus == readState.Value);

        //var notificationQuery = dbContext.Set<Notification>()
        //    .WhereIf(!filter.IsNullOrWhiteSpace(), nf =>
        //        nf.NotificationName.Contains(filter) ||
        //        nf.NotificationTypeName.Contains(filter));

        var notifilerQuery = from un in dbContext.Set<UserNotification>()
                             join n in dbContext.Set<Notification>()
                                on un.NotificationId equals n.NotificationId
                             where un.UserId == userId
                             select new UserNotificationInfo
                             {
                                 Id = n.NotificationId,
                                 TenantId = n.TenantId,
                                 Name = n.NotificationName,
                                 ExtraProperties = n.ExtraProperties,
                                 CreationTime = n.CreationTime,
                                 NotificationTypeName = n.NotificationTypeName,
                                 Severity = n.Severity,
                                 State = un.ReadStatus,
                                 Type = n.Type,
                                 ContentType = n.ContentType,
                             };

        return await notifilerQuery
            .WhereIf(readState.HasValue, x => x.State == readState.Value)
            .WhereIf(!filter.IsNullOrWhiteSpace(), nf =>
                nf.Name.Contains(filter) ||
                nf.NotificationTypeName.Contains(filter))
            .OrderBy(sorting)
            .PageBy(skipCount, maxResultCount)
            .AsNoTracking()
            .ToListAsync(GetCancellationToken(cancellationToken));
    }
}
