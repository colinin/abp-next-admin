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
public class WebhooksPublishController : WebhooksManagementControllerBase, IWebhooksPublishAppService
{
    protected IWebhooksPublishAppService PublishAppService { get; }

    public WebhooksPublishController(IWebhooksPublishAppService publishAppService)
    {
        PublishAppService = publishAppService;
    }

    [HttpPost]
    public virtual Task PublishAsync(WebhooksPublishInput input)
    {
        return PublishAppService.PublishAsync(input);
    }
}
