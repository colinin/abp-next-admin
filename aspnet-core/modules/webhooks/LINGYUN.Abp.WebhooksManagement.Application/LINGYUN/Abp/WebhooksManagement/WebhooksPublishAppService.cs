using LINGYUN.Abp.Webhooks;
using LINGYUN.Abp.WebhooksManagement.Authorization;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;
using System.Linq;
using System.Threading.Tasks;

namespace LINGYUN.Abp.WebhooksManagement;

[Authorize(WebhooksManagementPermissions.Publish)]
public class WebhooksPublishAppService : WebhooksManagementAppServiceBase, IWebhooksPublishAppService
{
    protected IWebhookPublisher InnerPublisher { get; }

    public WebhooksPublishAppService(IWebhookPublisher innerPublisher)
    {
        InnerPublisher = innerPublisher;
    }

    public async virtual Task PublishAsync(WebhooksPublishInput input)
    {
        var webhookHeader = new WebhookHeader
        {
            UseOnlyGivenHeaders = input.Header.UseOnlyGivenHeaders,
            Headers = input.Header.Headers,
        };
        var inputData = JsonConvert.DeserializeObject(input.Data);

        if (input.TenantIds.Any())
        {
            await InnerPublisher.PublishAsync(
                input.TenantIds.ToArray(),
                input.WebhookName,
                inputData,
                input.SendExactSameData,
                webhookHeader);
            return;
        }
        await InnerPublisher.PublishAsync(
            input.WebhookName,
            inputData,
            input.SendExactSameData,
            webhookHeader);
    }
}
