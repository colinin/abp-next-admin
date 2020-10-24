using LINGYUN.Abp.MessageService.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
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

        public async Task<List<UserSubscribe>> GetSubscribesAsync(string notificationName)
        {
            var userSubscribes = await DbSet
                .Distinct()
                .Where(x => x.NotificationName.Equals(notificationName))
                .AsNoTracking()
                .ToListAsync();

            return userSubscribes;
        }

        public async Task<UserSubscribe> GetUserSubscribeAsync(string notificationName, Guid userId)
        {
            var userSubscribe = await DbSet
                .Where(x => x.UserId.Equals(userId) && x.NotificationName.Equals(notificationName))
                .AsNoTracking()
                .FirstOrDefaultAsync();

            return userSubscribe;
        }

        public async Task<List<string>> GetUserSubscribesAsync(Guid userId)
        {
            var userSubscribeNames = await DbSet
                .Distinct()
                .Where(x => x.UserId.Equals(userId))
                .Select(x => x.NotificationName)
                .ToListAsync();

            return userSubscribeNames;
        }

        public async Task<List<UserSubscribe>> GetUserSubscribesByNameAsync(string userName)
        {
            var userSubscribeNames = await DbSet
                .Distinct()
                .Where(x => x.UserName.Equals(userName))
                .AsNoTracking()
                .ToListAsync();

            return userSubscribeNames;
        }

        public async Task<List<Guid>> GetUserSubscribesAsync(string notificationName)
        {
            var subscribeUsers = await DbSet
                .Distinct()
                .Where(x => x.NotificationName.Equals(notificationName))
                .Select(x => x.UserId)
                .ToListAsync();

            return subscribeUsers;
        }

        public async Task InsertUserSubscriptionAsync(IEnumerable<UserSubscribe> userSubscribes)
        {
            await DbSet.AddRangeAsync(userSubscribes);
        }

        public async Task DeleteUserSubscriptionAsync(string notificationName)
        {
            var userSubscribes = await DbSet.Where(x => x.NotificationName.Equals(notificationName)).ToListAsync();
            DbSet.RemoveRange(userSubscribes);
        }

        public Task DeleteUserSubscriptionAsync(IEnumerable<UserSubscribe> userSubscribes)
        {
            DbSet.RemoveRange(userSubscribes);
            return Task.CompletedTask;
        }

        public async Task<bool> UserSubscribeExistsAysnc(string notificationName, Guid userId)
        {
            return await DbSet.AnyAsync(x => x.UserId.Equals(userId) && x.NotificationName.Equals(notificationName));
        }

        public virtual async Task<List<UserSubscribe>> GetUserSubscribesAsync(Guid userId, string sorting = "Id", int skipCount = 1, int maxResultCount = 10)
        {
            var userSubscribes = await DbSet
                 .Distinct()
                 .Where(x => x.UserId.Equals(userId))
                 .OrderBy(sorting ?? nameof(UserSubscribe.Id))
                 .Page(skipCount, maxResultCount)
                 .AsNoTracking()
                 .ToListAsync();

            return userSubscribes;
        }

        public virtual async Task<long> GetCountAsync(Guid userId)
        {
            var userSubscribedCount = await DbSet
                 .Distinct()
                 .Where(x => x.UserId.Equals(userId))
                 .LongCountAsync();

            return userSubscribedCount;
        }
    }
}
