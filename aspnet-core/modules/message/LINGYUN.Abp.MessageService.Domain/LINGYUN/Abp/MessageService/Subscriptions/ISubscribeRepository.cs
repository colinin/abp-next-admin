using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace LINGYUN.Abp.MessageService.Subscriptions
{
    public interface ISubscribeRepository : IBasicRepository<Subscribe, long>
    {
        Task<bool> UserSubscribeExistsAysnc(string notificationName, Guid userId);

        Task<UserSubscribe> GetUserSubscribeAsync(string notificationName, Guid userId);

        Task<List<string>> GetUserSubscribesAsync(Guid userId);

        Task<List<Guid>> GetUserSubscribesAsync(string notificationName);
    }
}
