using LINGYUN.Abp.MessageService.EntityFrameworkCore;
using LINGYUN.Abp.Notifications;
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

namespace LINGYUN.Abp.MessageService.Notifications
{
    public class EfCoreUserNotificationRepository : EfCoreRepository<IMessageServiceDbContext, UserNotification, long>,
        IUserNotificationRepository, ITransientDependency
    {
        public EfCoreUserNotificationRepository(
            IDbContextProvider<IMessageServiceDbContext> dbContextProvider) 
            : base(dbContextProvider)
        {
        }

        public virtual async Task<bool> AnyAsync(
            Guid userId, 
            long notificationId,
            CancellationToken cancellationToken = default)
        {
            return await DbSet
                .AnyAsync(x => x.NotificationId.Equals(notificationId) && x.UserId.Equals(userId),
                    GetCancellationToken(cancellationToken));
        }

        public virtual async Task InsertUserNotificationsAsync(
            IEnumerable<UserNotification> userNotifications,
            CancellationToken cancellationToken = default)
        {
            await DbSet.AddRangeAsync(userNotifications, GetCancellationToken(cancellationToken));
        }

        public virtual async Task ChangeUserNotificationReadStateAsync(
            Guid userId,
            long notificationId, 
            NotificationReadState readState,
            CancellationToken cancellationToken = default)
        {
            var userNofitication = await GetByIdAsync(userId, notificationId, cancellationToken);
            userNofitication.ChangeReadState(readState);

            DbSet.Update(userNofitication);
        }

        public virtual async Task<UserNotification> GetByIdAsync(
            Guid userId,
            long notificationId,
            CancellationToken cancellationToken = default)
        {
            var userNofitication = await DbSet
                .Where(x => x.NotificationId.Equals(notificationId) && x.UserId.Equals(userId))
                .FirstOrDefaultAsync(GetCancellationToken(cancellationToken));

            return userNofitication;
        }

        public virtual async Task<List<Notification>> GetNotificationsAsync(
            Guid userId,
            NotificationReadState readState = NotificationReadState.UnRead,
            int maxResultCount = 10,
            CancellationToken cancellationToken = default)
        {
            var userNotifilerQuery = from un in DbContext.Set<UserNotification>()
                                     join n in DbContext.Set<Notification>()
                                         on un.NotificationId equals n.NotificationId
                                     where un.UserId.Equals(userId) && un.ReadStatus.Equals(readState)
                                     select n;

            return await userNotifilerQuery
                .OrderBy(nameof(Notification.CreationTime) + " DESC")
                .Take(maxResultCount)
                .AsNoTracking()
                .ToListAsync(GetCancellationToken(cancellationToken));
        }

        public virtual async Task<int> GetCountAsync(
            Guid userId, 
            string filter = "", 
            NotificationReadState readState = NotificationReadState.UnRead,
            CancellationToken cancellationToken = default)
        {
            var userNotifilerQuery = from un in DbContext.Set<UserNotification>()
                                     join n in DbContext.Set<Notification>()
                                         on un.NotificationId equals n.NotificationId
                                     where un.UserId.Equals(userId) && un.ReadStatus.Equals(readState)
                                     select n;

            return await userNotifilerQuery
                .WhereIf(!filter.IsNullOrWhiteSpace(), nf =>
                    nf.NotificationName.Contains(filter) ||
                    nf.NotificationTypeName.Contains(filter))
                .CountAsync(GetCancellationToken(cancellationToken));
        }

        public virtual async Task<List<Notification>> GetListAsync(
            Guid userId, 
            string filter = "",
            string sorting = nameof(Notification.CreationTime),
            bool reverse = true,
            NotificationReadState readState = NotificationReadState.UnRead, 
            int skipCount = 1,
            int maxResultCount = 10,
            CancellationToken cancellationToken = default)
        {
            sorting ??= nameof(Notification.CreationTime);
            sorting = reverse ? sorting + " DESC" : sorting;

            var userNotifilerQuery = from un in DbContext.Set<UserNotification>()
                                     join n in DbContext.Set<Notification>()
                                         on un.NotificationId equals n.NotificationId
                                     where un.UserId.Equals(userId) && un.ReadStatus.Equals(readState)
                                     select n;

            return await userNotifilerQuery
                .WhereIf(!filter.IsNullOrWhiteSpace(), nf =>
                    nf.NotificationName.Contains(filter) ||
                    nf.NotificationTypeName.Contains(filter))
                .OrderBy(sorting)
                .PageBy(skipCount, maxResultCount)
                .AsNoTracking()
                .ToListAsync(GetCancellationToken(cancellationToken));
        }
    }
}
