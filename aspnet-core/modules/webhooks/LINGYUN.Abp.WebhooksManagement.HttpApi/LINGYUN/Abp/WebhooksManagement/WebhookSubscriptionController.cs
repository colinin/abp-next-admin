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
    public virtual Task<WebhookSubscriptionDto> CreateAsync(WebhookSubscriptionCreateInput input)
    {
        return SubscriptionAppService.CreateAsync(input);
    }

    [HttpDelete]
    [Route("{id}")]
    [Authorize(WebhooksManagementPermissions.WebhookSubscription.Delete)]
    public virtual Task DeleteAsync(Guid id)
    {
        return SubscriptionAppService.DeleteAsync(id);
    }

    [HttpGet]
    [Route("{id}")]
    public virtual Task<WebhookSubscriptionDto> GetAsync(Guid id)
    {
        return SubscriptionAppService.GetAsync(id);
    }

    [HttpGet]
    public virtual Task<PagedResultDto<WebhookSubscriptionDto>> GetListAsync(WebhookSubscriptionGetListInput input)
    {
        return SubscriptionAppService.GetListAsync(input);
    }

    [HttpPut]
    [Route("{id}")]
    [Authorize(WebhooksManagementPermissions.WebhookSubscription.Update)]
    public virtual Task<WebhookSubscriptionDto> UpdateAsync(Guid id, WebhookSubscriptionUpdateInput input)
    {
        return SubscriptionAppService.UpdateAsync(id, input);
    }

    [HttpGet]
    [Route("availables")]
    public virtual Task<ListResultDto<WebhookAvailableGroupDto>> GetAllAvailableWebhooksAsync()
    {
        return SubscriptionAppService.GetAllAvailableWebhooksAsync();
    }
}
