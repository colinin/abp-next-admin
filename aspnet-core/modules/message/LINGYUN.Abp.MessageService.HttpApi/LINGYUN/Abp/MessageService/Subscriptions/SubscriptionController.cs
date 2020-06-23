using LINGYUN.Abp.Notifications;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;

namespace LINGYUN.Abp.MessageService.Subscriptions
{
    [RemoteService(Name = AbpMessageServiceConsts.RemoteServiceName)]
    [Route("api/subscribes")]
    public class SubscriptionController : AbpController, ISubscriptionAppService
    {
        private readonly ISubscriptionAppService _subscriptionAppService;

        public SubscriptionController(
            ISubscriptionAppService subscriptionAppService)
        {
            _subscriptionAppService = subscriptionAppService;
        }

        [HttpGet]
        public virtual async Task<PagedResultDto<NotificationSubscriptionInfo>> GetSubscribedsAsync(SubscriptionsGetByPagedDto subscriptionsGetByPaged)
        {
            return await _subscriptionAppService.GetSubscribedsAsync(subscriptionsGetByPaged);
        }

        [HttpGet]
        [Route("IsSubscribed/{NotificationName}")]
        public virtual async Task<bool> IsSubscribedAsync(SubscriptionsGetByNameDto subscriptionsGetByName)
        {
            return await _subscriptionAppService.IsSubscribedAsync(subscriptionsGetByName);
        }

        [HttpPost]
        public virtual async Task SubscribeAsync(SubscriptionsGetByNameDto subscriptionsGetByName)
        {
            await _subscriptionAppService.SubscribeAsync(subscriptionsGetByName);
        }

        [HttpDelete]
        public virtual async Task UnSubscribeAsync(SubscriptionsGetByNameDto subscriptionsGetByName)
        {
            await _subscriptionAppService.UnSubscribeAsync(subscriptionsGetByName);
        }
    }
}
