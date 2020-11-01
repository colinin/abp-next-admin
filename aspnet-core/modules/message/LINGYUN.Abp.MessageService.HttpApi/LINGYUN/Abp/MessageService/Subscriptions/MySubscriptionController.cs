using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;

namespace LINGYUN.Abp.MessageService.Subscriptions
{
    [RemoteService(Name = AbpMessageServiceConsts.RemoteServiceName)]
    [Route("api/my-subscribes")]
    public class MySubscriptionController : AbpController, IMySubscriptionAppService
    {
        private readonly IMySubscriptionAppService _subscriptionAppService;

        public MySubscriptionController(
            IMySubscriptionAppService subscriptionAppService)
        {
            _subscriptionAppService = subscriptionAppService;
        }

        [HttpGet]
        [Route("all")]
        public virtual async Task<ListResultDto<UserSubscreNotificationDto>> GetAllListAsync()
        {
            return await _subscriptionAppService.GetAllListAsync();
        }

        [HttpGet]
        public virtual async Task<PagedResultDto<UserSubscreNotificationDto>> GetListAsync(SubscriptionsGetByPagedDto input)
        {
            return await _subscriptionAppService.GetListAsync(input);
        }

        [HttpGet]
        [Route("is-subscribed/{Name}")]
        public virtual async Task<UserSubscriptionsResult> IsSubscribedAsync(SubscriptionsGetByNameDto input)
        {
            return await _subscriptionAppService.IsSubscribedAsync(input);
        }

        [HttpPost]
        public virtual async Task SubscribeAsync(SubscriptionsGetByNameDto input)
        {
            await _subscriptionAppService.SubscribeAsync(input);
        }

        [HttpDelete]
        public virtual async Task UnSubscribeAsync(SubscriptionsGetByNameDto input)
        {
            await _subscriptionAppService.UnSubscribeAsync(input);
        }
    }
}
