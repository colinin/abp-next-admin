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

public class EfCoreUserSubscribeRepository : EfCoreRepository<INotificationsDbContext, UserSubscribe, long>,
        IUserSubscribeRepository, ITransientDependency
{
    public EfCoreUserSubscribeRepository(
        IDbContextProvider<INotificationsDbContext> dbContextProvider)
        : base(dbContextProvider)
    {
    }

    public async virtual Task<List<UserSubscribe>> GetUserSubscribesAsync(
        string notificationName,
        IEnumerable<Guid> userIds = null,
        CancellationToken cancellationToken = default)
    {
        var userSubscribes = await (await GetDbSetAsync())
            .Distinct()
            .Where(x => x.NotificationName.Equals(notificationName))
            .WhereIf(userIds?.Any() == true, x => userIds.Contains(x.UserId))
            .AsNoTracking()
            .ToListAsync(GetCancellationToken(cancellationToken));

        return userSubscribes;
    }

    public async virtual Task<UserSubscribe> GetUserSubscribeAsync(
        string notificationName,
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        var userSubscribe = await (await GetDbSetAsync())
            .Where(x => x.UserId.Equals(userId) && x.NotificationName.Equals(notificationName))
            .AsNoTracking()
            .FirstOrDefaultAsync(GetCancellationToken(cancellationToken));

        return userSubscribe;
    }

    public async virtual Task<List<string>> GetUserSubscribesAsync(
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        var userSubscribeNames = await (await GetDbSetAsync())
            .Distinct()
            .Where(x => x.UserId.Equals(userId))
            .Select(x => x.NotificationName)
            .ToListAsync(GetCancellationToken(cancellationToken));

        return userSubscribeNames;
    }

    public async virtual Task<List<UserSubscribe>> GetUserSubscribesByNameAsync(
        string userName,
        CancellationToken cancellationToken = default)
    {
        var userSubscribeNames = await (await GetDbSetAsync())
            .Distinct()
            .Where(x => x.UserName.Equals(userName))
            .AsNoTracking()
            .ToListAsync(GetCancellationToken(cancellationToken));

        return userSubscribeNames;
    }

    public async virtual Task<List<Guid>> GetUserSubscribesAsync(
        string notificationName,
        CancellationToken cancellationToken = default)
    {
        var subscribeUsers = await (await GetDbSetAsync())
            .Distinct()
            .Where(x => x.NotificationName.Equals(notificationName))
            .Select(x => x.UserId)
            .ToListAsync(GetCancellationToken(cancellationToken));

        return subscribeUsers;
    }

    public async virtual Task InsertUserSubscriptionAsync(
        IEnumerable<UserSubscribe> userSubscribes,
        CancellationToken cancellationToken = default)
    {
        await (await GetDbSetAsync()).AddRangeAsync(userSubscribes, GetCancellationToken(cancellationToken));
    }

    public async virtual Task DeleteUserSubscriptionAsync(
        string notificationName,
        CancellationToken cancellationToken = default)
    {
        var userSubscribes = await (await GetDbSetAsync()).Where(x => x.NotificationName.Equals(notificationName))
            .ToListAsync(GetCancellationToken(cancellationToken));
        (await GetDbSetAsync()).RemoveRange(userSubscribes);
    }

    public async virtual Task DeleteUserSubscriptionAsync(
        IEnumerable<UserSubscribe> userSubscribes,
        CancellationToken cancellationToken = default)
    {
        await DeleteManyAsync(userSubscribes);
    }

    public async virtual Task DeleteUserSubscriptionAsync(
        string notificationName,
        IEnumerable<Guid> userIds,
        CancellationToken cancellationToken = default)
    {
        await DeleteAsync(usr => usr.NotificationName == notificationName && userIds.Contains(usr.UserId),
            false,
            GetCancellationToken(cancellationToken));
    }

    public async virtual Task<bool> UserSubscribeExistsAysnc(
        string notificationName,
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .AnyAsync(x => x.UserId.Equals(userId) && x.NotificationName.Equals(notificationName),
                GetCancellationToken(cancellationToken));
    }

    public async virtual Task<List<UserSubscribe>> GetUserSubscribesAsync(
        Guid userId,
        string sorting = "Id",
        int skipCount = 1,
        int maxResultCount = 10,
        CancellationToken cancellationToken = default)
    {
        if (sorting.IsNullOrWhiteSpace())
        {
            sorting = nameof(UserSubscribe.Id);
        }
        var userSubscribes = await (await GetDbSetAsync())
             .Distinct()
             .Where(x => x.UserId.Equals(userId))
             .OrderBy(sorting)
             .Page(skipCount, maxResultCount)
             .AsNoTracking()
             .ToListAsync(GetCancellationToken(cancellationToken));

        return userSubscribes;
    }

    public async virtual Task<long> GetCountAsync(
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        var userSubscribedCount = await (await GetDbSetAsync())
             .Distinct()
             .Where(x => x.UserId.Equals(userId))
             .LongCountAsync(GetCancellationToken(cancellationToken));

        return userSubscribedCount;
    }
}
