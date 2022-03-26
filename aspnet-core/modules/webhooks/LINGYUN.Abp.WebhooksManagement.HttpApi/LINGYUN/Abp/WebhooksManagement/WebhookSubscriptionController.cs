using LINGYUN.Abp.WebhooksManagement.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;

namespace LINGYUN.Abp.WebhooksManagement;

[RemoteService(Name = WebhooksManagementRemoteServiceConsts.RemoteServiceName)]
[Area(WebhooksManagementRemoteServiceConsts.ModuleName)]
[Authorize(WebhooksManagementPermissions.WebhookSubscription.Default)]
[Route("api/webhooks/subscriptions")]
public class WebhookSubscriptionController : WebhooksManagementControllerBase, IWebhookSubscriptionAppService
{
    protected IWebhookSubscriptionAppService SubscriptionAppService { get; }

    public WebhookSubscriptionController(IWebhookSubscriptionAppService subscriptionAppService)
    {
        SubscriptionAppService = subscriptionAppService;
    }

    [HttpPost]
    [Authorize(WebhooksManagementPermissions.WebhookSubscription.Create)]
    public Task<WebhookSubscriptionDto> CreateAsync(WebhookSubscriptionCreateInput input)
    {
        return SubscriptionAppService.CreateAsync(input);
    }

    [HttpDelete]
    [Route("{id}")]
    [Authorize(WebhooksManagementPermissions.WebhookSubscription.Delete)]
    public Task DeleteAsync(Guid id)
    {
        return SubscriptionAppService.DeleteAsync(id);
    }

    [HttpGet]
    [Route("{id}")]
    public Task<WebhookSubscriptionDto> GetAsync(Guid id)
    {
        return SubscriptionAppService.GetAsync(id);
    }

    [HttpGet]
    public Task<PagedResultDto<WebhookSubscriptionDto>> GetListAsync(WebhookSubscriptionGetListInput input)
    {
        return SubscriptionAppService.GetListAsync(input);
    }

    [HttpPut]
    [Route("{id}")]
    [Authorize(WebhooksManagementPermissions.WebhookSubscription.Update)]
    public Task<WebhookSubscriptionDto> UpdateAsync(Guid id, WebhookSubscriptionUpdateInput input)
    {
        return SubscriptionAppService.UpdateAsync(id, input);
    }
}
