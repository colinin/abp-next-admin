using LINGYUN.Abp.WebhooksManagement;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.Webhooks.ClientProxies;

[Dependency(ReplaceServices = true)]
public class ClientProxiesWebhookPublisher : IWebhookPublisher, ITransientDependency
{
    protected IWebhookPublishAppService PublishAppService { get; }

    public ClientProxiesWebhookPublisher(
        IWebhookPublishAppService publishAppService)
    {
        PublishAppService = publishAppService;
    }

    public async virtual Task PublishAsync(string webhookName, object data, bool sendExactSameData = false, WebhookHeader headers = null)
    {
        var input = new WebhookPublishInput
        {
            WebhookName = webhookName,
            Data = JsonConvert.SerializeObject(data),
            SendExactSameData = sendExactSameData,
        };
        if (headers != null)
        {
            input.Header = new WebhooksHeaderInput
            {
                UseOnlyGivenHeaders = headers.UseOnlyGivenHeaders,
                Headers = headers.Headers
            };
        }

        await PublishAsync(input);
    }

    public async virtual Task PublishAsync(string webhookName, object data, Guid? tenantId, bool sendExactSameData = false, WebhookHeader headers = null)
    {
        var input = new WebhookPublishInput
        {
            WebhookName = webhookName,
            Data = JsonConvert.SerializeObject(data),
            SendExactSameData = sendExactSameData,
            TenantIds = new List<Guid?>
            {
                tenantId
            },
        };
        if (headers != null)
        {
            input.Header = new WebhooksHeaderInput
            {
                UseOnlyGivenHeaders = headers.UseOnlyGivenHeaders,
                Headers = headers.Headers
            };
        }

        await PublishAsync(input);
    }

    public async virtual Task PublishAsync(Guid?[] tenantIds, string webhookName, object data, bool sendExactSameData = false, WebhookHeader headers = null)
    {
        var input = new WebhookPublishInput
        {
            WebhookName = webhookName,
            Data = JsonConvert.SerializeObject(data),
            SendExactSameData = sendExactSameData,
            TenantIds = tenantIds.ToList(),
        };
        if (headers != null)
        {
            input.Header = new WebhooksHeaderInput
            {
                UseOnlyGivenHeaders = headers.UseOnlyGivenHeaders,
                Headers = headers.Headers
            };
        }

        await PublishAsync(input);
    }

    protected virtual async Task PublishAsync(WebhookPublishInput input)
    {
        await PublishAppService.PublishAsync(input);
    }
}
