using System;

namespace LINGYUN.Abp.WebhooksManagement;

public class WebhookSubscriptionFilter
{
    public string Filter { get; set; }

    public Guid? TenantId { get; set; }

    public string WebhookUri { get; set; }

    public string Secret { get; set; }

    public bool? IsActive { get; set; }

    public string Webhooks { get; set; }

    public DateTime? BeginCreationTime { get; set; }

    public DateTime? EndCreationTime { get; set; }
}
