using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Volo.Abp;

namespace LINGYUN.Abp.WebhooksManagement.Integration;

[Area(WebhooksManagementRemoteServiceConsts.ModuleName)]
[ControllerName("WebhookPublishIntegration")]
[RemoteService(Name = WebhooksManagementRemoteServiceConsts.RemoteServiceName)]
[Route($"integration-api/{WebhooksManagementRemoteServiceConsts.ModuleName}/webhooks/publish")]
public class WebhookPublishIntegrationController : WebhooksManagementControllerBase, IWebhookPublishIntegrationService
{
    protected IWebhookPublishIntegrationService PublishAppService { get; }

    public WebhookPublishIntegrationController(IWebhookPublishIntegrationService publishAppService)
    {
        PublishAppService = publishAppService;
    }

    [HttpPost]
    public virtual Task PublishAsync(WebhookPublishInput input)
    {
        return PublishAppService.PublishAsync(input);
    }
}
