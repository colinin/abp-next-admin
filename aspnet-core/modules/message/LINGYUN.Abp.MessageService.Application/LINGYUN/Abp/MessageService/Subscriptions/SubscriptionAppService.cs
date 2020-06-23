using LINGYUN.Abp.Notifications;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Users;

namespace LINGYUN.Abp.MessageService.Subscriptions
{
    [Authorize]
    public class SubscriptionAppService : ApplicationService, ISubscriptionAppService
    {
        private readonly IUserSubscribeRepository _userSubscribeRepository;

        public SubscriptionAppService(
            IUserSubscribeRepository userSubscribeRepository)
        {
            _userSubscribeRepository = userSubscribeRepository;
        }

        public virtual async Task<PagedResultDto<NotificationSubscriptionInfo>> GetSubscribedsAsync(SubscriptionsGetByPagedDto subscriptionsGetByPaged)
        {
            var userSubscribedCount = await _userSubscribeRepository.GetCountAsync(CurrentUser.GetId());

            var userSubscribes = await _userSubscribeRepository
                .GetUserSubscribesAsync(CurrentUser.GetId(), subscriptionsGetByPaged.Sorting,
                        subscriptionsGetByPaged.SkipCount, subscriptionsGetByPaged.MaxResultCount);

            return new PagedResultDto<NotificationSubscriptionInfo>(userSubscribedCount,
                ObjectMapper.Map<List<UserSubscribe>, List<NotificationSubscriptionInfo>>(userSubscribes));
        }

        public virtual async Task<bool> IsSubscribedAsync(SubscriptionsGetByNameDto subscriptionsGetByName)
        {
            var isSubscribed = await _userSubscribeRepository
                .UserSubscribeExistsAysnc(subscriptionsGetByName.NotificationName, CurrentUser.GetId());

            return isSubscribed;
        }

        public virtual async Task SubscribeAsync(SubscriptionsGetByNameDto subscriptionsGetByName)
        {
            var isSubscribed = await _userSubscribeRepository
                .UserSubscribeExistsAysnc(subscriptionsGetByName.NotificationName, CurrentUser.GetId());
            if (!isSubscribed)
            {
                UserSubscribe userSubscribe = new UserSubscribe(
                    subscriptionsGetByName.NotificationName, 
                    CurrentUser.GetId(), CurrentUser.UserName, CurrentTenant.Id);

                await _userSubscribeRepository.InsertAsync(userSubscribe, true);
            }
        }

        public virtual async Task UnSubscribeAsync(SubscriptionsGetByNameDto subscriptionsGetByName)
        {
            var userSubscribe = await _userSubscribeRepository.GetUserSubscribeAsync(subscriptionsGetByName.NotificationName, CurrentUser.GetId());
            if (userSubscribe != null)
            {
                await _userSubscribeRepository.DeleteAsync(userSubscribe.Id, true);
            }
        }
    }
}
