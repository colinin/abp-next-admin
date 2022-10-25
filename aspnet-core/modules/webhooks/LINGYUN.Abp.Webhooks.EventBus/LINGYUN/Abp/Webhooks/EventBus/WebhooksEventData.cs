using System;
using Volo.Abp.EventBus;

namespace LINGYUN.Abp.Webhooks.EventBus;

[Serializable]
[EventName("abp.webhooks.events")]
public class WebhooksEventData
{
    public Guid?[] TenantIds { get; set; }

    public string WebhookName { get; set; }

    public string Data { get; set; }

    public bool SendExactSameData { get; set; }

    public WebhookHeader Headers { get; set; }

    public WebhooksEventData()
    {
        Headers = new WebhookHeader();
        TenantIds = new Guid?[0];
    }

    public WebhooksEventData(
        string webhookName,
        string data, 
        bool sendExactSameData = false, 
        WebhookHeader headers = null,
        Guid?[] tenantIds = null)
    {
        WebhookName = webhookName;
        Data = data;
        SendExactSameData = sendExactSameData;
        Headers = headers ?? new WebhookHeader();
        TenantIds = tenantIds ?? new Guid?[0];
    }
}
