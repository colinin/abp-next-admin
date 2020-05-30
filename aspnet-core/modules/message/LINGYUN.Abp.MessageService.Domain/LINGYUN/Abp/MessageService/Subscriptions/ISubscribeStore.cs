using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LINGYUN.Abp.MessageService.Subscriptions
{
    public interface ISubscribeStore
    {
        Task<List<Guid>> GetUserSubscribesAsync(string notificationName);
        Task UserSubscribeAsync(string notificationName, Guid userId);
        Task UserUnSubscribeAsync(string notificationName, Guid userId);
    }
}
