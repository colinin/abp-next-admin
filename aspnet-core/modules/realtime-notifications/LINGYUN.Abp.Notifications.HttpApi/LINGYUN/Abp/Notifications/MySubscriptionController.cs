using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;

namespace LINGYUN.Abp.Notifications;

[Authorize]
[Controller]
[RemoteService(Name = AbpNotificationsRemoteServiceConsts.RemoteServiceName)]
[Area(AbpNotificationsRemoteServiceConsts.ModuleName)]
[Route("api/notifications/my-subscribes")]
public class MySubscriptionController : AbpControllerBase, IMySubscriptionAppService
{
    private readonly IMySubscriptionAppService _subscriptionAppService;

    public MySubscriptionController(
        IMySubscriptionAppService subscriptionAppService)
    {
        _subscriptionAppService = subscriptionAppService;
    }

    [HttpGet]
    [Route("all")]
    public async virtual Task<ListResultDto<UserSubscreNotificationDto>> GetAllListAsync()
    {
        return await _subscriptionAppService.GetAllListAsync();
    }

    [HttpGet]
    public async virtual Task<PagedResultDto<UserSubscreNotificationDto>> GetListAsync(SubscriptionsGetByPagedDto input)
    {
        return await _subscriptionAppService.GetListAsync(input);
    }

    [HttpGet]
    [Route("is-subscribed/{Name}")]
    public async virtual Task<UserSubscriptionsResult> IsSubscribedAsync(SubscriptionsGetByNameDto input)
    {
        return await _subscriptionAppService.IsSubscribedAsync(input);
    }

    [HttpPost]
    public async virtual Task SubscribeAsync(SubscriptionsGetByNameDto input)
    {
        await _subscriptionAppService.SubscribeAsync(input);
    }

    [HttpDelete]
    public async virtual Task UnSubscribeAsync(SubscriptionsGetByNameDto input)
    {
        await _subscriptionAppService.UnSubscribeAsync(input);
    }
}
