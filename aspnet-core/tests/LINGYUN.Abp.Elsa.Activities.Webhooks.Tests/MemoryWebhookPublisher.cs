using LINGYUN.Abp.Webhooks;

namespace LINGYUN.Abp.Elsa.Activities.Webhooks.Tests;

internal class MemoryWebhookPublisher : IWebhookPublisher
{
    public static readonly IDictionary<string, WebhookData> WebHooks = new Dictionary<string, WebhookData>();

    public Task PublishAsync(string webhookName, object data, bool sendExactSameData = false, WebhookHeader? headers = null)
    {
        if (!WebHooks.ContainsKey(webhookName))
        {
            WebHooks.Add(webhookName,
                new WebhookData(webhookName, data, sendExactSameData, headers));
        }

        return Task.CompletedTask;
    }

    public Task PublishAsync(string webhookName, object data, Guid? tenantId, bool sendExactSameData = false, WebhookHeader? headers = null)
    {
        if (!WebHooks.ContainsKey(webhookName))
        {
            Guid?[] tenantIds = new Guid?[] { tenantId };
            var webhook = new WebhookData(webhookName, data, sendExactSameData, headers, tenantIds);

            WebHooks.Add(webhookName, webhook);
        }

        return Task.CompletedTask;
    }

    public Task PublishAsync(Guid?[] tenantIds, string webhookName, object data, bool sendExactSameData = false, WebhookHeader? headers = null)
    {
        if (!WebHooks.ContainsKey(webhookName))
        {
            var webhook = new WebhookData(webhookName, data, sendExactSameData, headers, tenantIds);

            WebHooks.Add(webhookName, webhook);
        }

        return Task.CompletedTask;
    }
}

internal class WebhookData
{
    public string Name { get; set; }
    public object Data { get; set; }
    public bool SendExactSameData { get; set; }
    public WebhookHeader? Headers { get; set; }
    public Guid?[]? TenantIds { get; set; }
    public WebhookData(
        string name, 
        object data, 
        bool sendExactSameData = false, 
        WebhookHeader? headers = null,
        Guid?[]? tenantIds = null)
    {
        Name = name;
        Data = data;
        SendExactSameData = sendExactSameData;
        Headers = headers;
        TenantIds = tenantIds;
    }
}
