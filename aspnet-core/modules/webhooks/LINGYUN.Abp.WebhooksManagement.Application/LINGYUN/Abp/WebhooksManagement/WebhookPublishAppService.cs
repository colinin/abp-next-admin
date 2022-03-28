using LINGYUN.Abp.Webhooks;
using LINGYUN.Abp.WebhooksManagement.Authorization;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;
using System.Linq;
using System.Threading.Tasks;

namespace LINGYUN.Abp.WebhooksManagement;

[Authorize(WebhooksManagementPermissions.Publish)]
public class WebhookPublishAppService : WebhooksManagementAppServiceBase, IWebhookPublishAppService
{
    protected IWebhookPublisher InnerPublisher { get; }

    public WebhookPublishAppService(IWebhookPublisher innerPublisher)
    {
        InnerPublisher = innerPublisher;
    }

    public async virtual Task PublishAsync(WebhookPublishInput input)
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
