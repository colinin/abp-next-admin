using LINGYUN.Abp.MessageService.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace LINGYUN.Abp.MessageService.Subscriptions
{
    public class EfCoreUserSubscribeRepository : EfCoreRepository<MessageServiceDbContext, UserSubscribe, long>,
        IUserSubscribeRepository, ITransientDependency
    {
        public EfCoreUserSubscribeRepository(
            IDbContextProvider<MessageServiceDbContext> dbContextProvider) 
            : base(dbContextProvider)
        {
        }

        public async Task<List<UserSubscribe>> GetSubscribesAsync(string notificationName)
        {
            var userSubscribes = await DbSet
                .Distinct()
                .Where(x => x.NotificationName.Equals(notificationName))
                .ToListAsync();

            return userSubscribes;
        }

        public async Task<UserSubscribe> GetUserSubscribeAsync(string notificationName, Guid userId)
        {
            var userSubscribe = await DbSet
                .Where(x => x.UserId.Equals(userId) && x.NotificationName.Equals(notificationName))
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

        public async Task<bool> UserSubscribeExistsAysnc(string notificationName, Guid userId)
        {
            return await DbSet.AnyAsync(x => x.UserId.Equals(userId) && x.NotificationName.Equals(notificationName));
        }
    }
}
