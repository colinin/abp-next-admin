using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.MessageService.Subscriptions
{
    public class SubscribeStore : ISubscribeStore, ITransientDependency
    {
        protected ISubscribeRepository SubscribeRepository { get; }

        public SubscribeStore(ISubscribeRepository subscribeRepository)
        {
            SubscribeRepository = subscribeRepository;
        }

        public virtual async Task<List<Guid>> GetUserSubscribesAsync(string notificationName)
        {
            return await SubscribeRepository.GetUserSubscribesAsync(notificationName);
        }

        public virtual async Task UserSubscribeAsync(string notificationName, Guid userId)
        {
            var userSubscribeExists = await SubscribeRepository.UserSubscribeExistsAysnc(notificationName, userId);
            if (!userSubscribeExists)
            {
                var userSbuscribe = new UserSubscribe(notificationName, userId);
                await SubscribeRepository.InsertAsync(userSbuscribe);
            }
        }

        public virtual async Task UserUnSubscribeAsync(string notificationName, Guid userId)
        {
            var userSubscribe = await SubscribeRepository.GetUserSubscribeAsync(notificationName, userId);
            if (userSubscribe != null)
            {
                await SubscribeRepository.DeleteAsync(userSubscribe);
            }
        }
    }
}
