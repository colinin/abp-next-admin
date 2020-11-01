using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace LINGYUN.Abp.MessageService.Subscriptions
{
    public interface IUserSubscribeRepository : IBasicRepository<UserSubscribe, long>
    {
        Task<bool> UserSubscribeExistsAysnc(
            string notificationName, 
            Guid userId,
            CancellationToken cancellationToken = default);

        Task<UserSubscribe> GetUserSubscribeAsync(
            string notificationName,
            Guid userId,
            CancellationToken cancellationToken = default);

        Task<List<UserSubscribe>> GetUserSubscribesAsync(
            string notificationName,
            IEnumerable<Guid> userIds = null, 
            CancellationToken cancellationToken = default);

        Task<List<string>> GetUserSubscribesAsync(
            Guid userId,
            CancellationToken cancellationToken = default);

        Task<List<UserSubscribe>> GetUserSubscribesByNameAsync(
            string userName,
            CancellationToken cancellationToken = default);

        Task<List<Guid>> GetUserSubscribesAsync(
            string notificationName,
            CancellationToken cancellationToken = default);

        Task InsertUserSubscriptionAsync(
            IEnumerable<UserSubscribe> userSubscribes,
            CancellationToken cancellationToken = default);

        Task DeleteUserSubscriptionAsync(
            IEnumerable<UserSubscribe> userSubscribes,
            CancellationToken cancellationToken = default);

        Task DeleteUserSubscriptionAsync(
            string notificationName, 
            IEnumerable<Guid> userIds, 
            CancellationToken cancellationToken = default);

        Task DeleteUserSubscriptionAsync(
            string notificationName,
            CancellationToken cancellationToken = default);

        Task<List<UserSubscribe>> GetUserSubscribesAsync(
            Guid userId, 
            string sorting = nameof(UserSubscribe.Id),
            int skipCount = 1, 
            int maxResultCount = 10,
            CancellationToken cancellationToken = default);

        Task<long> GetCountAsync(
            Guid userId,
            CancellationToken cancellationToken = default);
    }
}
