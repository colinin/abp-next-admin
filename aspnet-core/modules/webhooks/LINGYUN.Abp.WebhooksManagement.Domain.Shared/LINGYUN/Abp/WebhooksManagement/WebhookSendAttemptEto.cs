using System;
using Volo.Abp.EventBus;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Abp.WebhooksManagement;

[Serializable]
[EventName("abp.webhooks.send-attempt")]
public class WebhookSendAttemptEto : IMultiTenant
{
    public Guid Id { get; set; }

    public Guid? TenantId { get; set; }

    public Guid WebhookEventId { get; set; }

    public Guid WebhookSubscriptionId { get; set; }
}
