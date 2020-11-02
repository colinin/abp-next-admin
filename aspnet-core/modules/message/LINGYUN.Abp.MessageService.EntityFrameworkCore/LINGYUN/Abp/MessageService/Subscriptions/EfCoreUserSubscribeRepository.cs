using LINGYUN.Abp.MessageService.EntityFrameworkCore;
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

namespace LINGYUN.Abp.MessageService.Subscriptions
{
    public class EfCoreUserSubscribeRepository : EfCoreRepository<IMessageServiceDbContext, UserSubscribe, long>,
        IUserSubscribeRepository, ITransientDependency
    {
        public EfCoreUserSubscribeRepository(
            IDbContextProvider<IMessageServiceDbContext> dbContextProvider) 
            : base(dbContextProvider)
        {
        }

        public async Task<List<UserSubscribe>> GetUserSubscribesAsync(
            string notificationName, 
            IEnumerable<Guid> userIds = null,
            CancellationToken cancellationToken = default)
        {
            var userSubscribes = await DbSet
                .Distinct()
                .Where(x => x.NotificationName.Equals(notificationName))
                .WhereIf(userIds != null, x => userIds.Contains(x.UserId))
                .AsNoTracking()
                .ToListAsync(GetCancellationToken(cancellationToken));

            return userSubscribes;
        }

        public async Task<UserSubscribe> GetUserSubscribeAsync(
            string notificationName, 
            Guid userId,
            CancellationToken cancellationToken = default)
        {
            var userSubscribe = await DbSet
                .Where(x => x.UserId.Equals(userId) && x.NotificationName.Equals(notificationName))
                .AsNoTracking()
                .FirstOrDefaultAsync(GetCancellationToken(cancellationToken));

            return userSubscribe;
        }

        public async Task<List<string>> GetUserSubscribesAsync(
            Guid userId,
            CancellationToken cancellationToken = default)
        {
            var userSubscribeNames = await DbSet
                .Distinct()
                .Where(x => x.UserId.Equals(userId))
                .Select(x => x.NotificationName)
                .ToListAsync(GetCancellationToken(cancellationToken));

            return userSubscribeNames;
        }

        public async Task<List<UserSubscribe>> GetUserSubscribesByNameAsync(
            string userName,
            CancellationToken cancellationToken = default)
        {
            var userSubscribeNames = await DbSet
                .Distinct()
                .Where(x => x.UserName.Equals(userName))
                .AsNoTracking()
                .ToListAsync(GetCancellationToken(cancellationToken));

            return userSubscribeNames;
        }

        public async Task<List<Guid>> GetUserSubscribesAsync(
            string notificationName,
            CancellationToken cancellationToken = default)
        {
            var subscribeUsers = await DbSet
                .Distinct()
                .Where(x => x.NotificationName.Equals(notificationName))
                .Select(x => x.UserId)
                .ToListAsync(GetCancellationToken(cancellationToken));

            return subscribeUsers;
        }

        public async Task InsertUserSubscriptionAsync(
            IEnumerable<UserSubscribe> userSubscribes,
            CancellationToken cancellationToken = default)
        {
            await DbSet.AddRangeAsync(userSubscribes, GetCancellationToken(cancellationToken));
        }

        public async Task DeleteUserSubscriptionAsync(
            string notificationName,
            CancellationToken cancellationToken = default)
        {
            var userSubscribes = await DbSet.Where(x => x.NotificationName.Equals(notificationName))
                .ToListAsync(GetCancellationToken(cancellationToken));
            DbSet.RemoveRange(userSubscribes);
        }

        public Task DeleteUserSubscriptionAsync(
            IEnumerable<UserSubscribe> userSubscribes,
            CancellationToken cancellationToken = default)
        {
            DbSet.RemoveRange(userSubscribes);
            return Task.CompletedTask;
        }

        public virtual async Task DeleteUserSubscriptionAsync(
            string notificationName, 
            IEnumerable<Guid> userIds,
            CancellationToken cancellationToken = default)
        {
            await DeleteAsync(usr => usr.NotificationName == notificationName && userIds.Contains(usr.UserId),
                false,
                GetCancellationToken(cancellationToken));
        }

        public async Task<bool> UserSubscribeExistsAysnc(
            string notificationName, 
            Guid userId,
            CancellationToken cancellationToken = default)
        {
            return await DbSet
                .AnyAsync(x => x.UserId.Equals(userId) && x.NotificationName.Equals(notificationName),
                    GetCancellationToken(cancellationToken));
        }

        public virtual async Task<List<UserSubscribe>> GetUserSubscribesAsync(
            Guid userId, 
            string sorting = "Id", 
            int skipCount = 1,
            int maxResultCount = 10,
            CancellationToken cancellationToken = default)
        {
            var userSubscribes = await DbSet
                 .Distinct()
                 .Where(x => x.UserId.Equals(userId))
                 .OrderBy(sorting ?? nameof(UserSubscribe.Id))
                 .Page(skipCount, maxResultCount)
                 .AsNoTracking()
                 .ToListAsync(GetCancellationToken(cancellationToken));

            return userSubscribes;
        }

        public virtual async Task<long> GetCountAsync(
            Guid userId,
            CancellationToken cancellationToken = default)
        {
            var userSubscribedCount = await DbSet
                 .Distinct()
                 .Where(x => x.UserId.Equals(userId))
                 .LongCountAsync(GetCancellationToken(cancellationToken));

            return userSubscribedCount;
        }
    }
}
