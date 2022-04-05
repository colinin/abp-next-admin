using System;
using System.Net;

namespace LINGYUN.Abp.WebhooksManagement;

public class WebhookSendRecordFilter
{
    public string Filter { get; set; }

    public Guid? TenantId { get; set; }

    public Guid? WebhookEventId { get; set; }

    public Guid? SubscriptionId { get; set; }

    public HttpStatusCode? ResponseStatusCode { get; set; }

    public DateTime? BeginCreationTime { get; set; }

    public DateTime? EndCreationTime { get; set; }
}
