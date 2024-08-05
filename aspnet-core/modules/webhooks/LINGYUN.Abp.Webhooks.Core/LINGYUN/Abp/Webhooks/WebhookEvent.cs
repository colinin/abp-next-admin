using System;

namespace LINGYUN.Abp.Webhooks;

public class WebhookEvent
{
    public Guid Id { get; set; }

    /// <summary>
    /// Webhook unique name <see cref="WebhookDefinition.Name"/>
    /// </summary>
    public string WebhookName { get; set; }

    /// <summary>
    /// Webhook data as JSON string.
    /// </summary>
    public string Data { get; set; }

    public DateTime CreationTime { get; set; }

    public Guid? TenantId { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime? DeletionTime { get; set; }
}
