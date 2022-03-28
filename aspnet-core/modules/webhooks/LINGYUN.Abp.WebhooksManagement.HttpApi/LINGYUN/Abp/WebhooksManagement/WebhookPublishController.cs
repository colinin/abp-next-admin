using LINGYUN.Abp.WebhooksManagement.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Volo.Abp;

namespace LINGYUN.Abp.WebhooksManagement;

[RemoteService(Name = WebhooksManagementRemoteServiceConsts.RemoteServiceName)]
[Area(WebhooksManagementRemoteServiceConsts.ModuleName)]
[Authorize(WebhooksManagementPermissions.Publish)]
[Route("api/webhooks/publish")]
public class WebhookPublishController : WebhooksManagementControllerBase, IWebhookPublishAppService
{
    protected IWebhookPublishAppService PublishAppService { get; }

    public WebhookPublishController(IWebhookPublishAppService publishAppService)
    {
        PublishAppService = publishAppService;
    }

    [HttpPost]
    public virtual Task PublishAsync(WebhookPublishInput input)
    {
        return PublishAppService.PublishAsync(input);
    }
}
