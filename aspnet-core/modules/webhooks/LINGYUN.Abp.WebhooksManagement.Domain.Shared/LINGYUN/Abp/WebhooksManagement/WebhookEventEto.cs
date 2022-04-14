using System;
using Volo.Abp.EventBus;

namespace LINGYUN.Abp.WebhooksManagement;

[Serializable]
[EventName("abp.webhooks.event")]
public class WebhookEventEto
{
    public Guid Id { get; set; }
    public Guid? TenantId { get; set; }
    public string WebhookName { get; set; }
}
