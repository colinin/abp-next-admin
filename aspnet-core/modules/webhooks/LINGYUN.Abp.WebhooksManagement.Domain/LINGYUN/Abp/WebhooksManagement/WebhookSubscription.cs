using System;
using Volo.Abp.Domain.Entities.Auditing;

namespace LINGYUN.Abp.WebhooksManagement;

public class WebhookSubscription : CreationAuditedEntity<Guid>
{
    public virtual Guid? TenantId { get; protected set; }
    public virtual string WebhookUri { get; protected set; }
    public virtual string Secret { get; protected set; }
    public virtual bool IsActive { get; set; }
    public virtual string Webhooks { get; protected set; }
    public virtual string Headers { get; protected set; }
    protected WebhookSubscription()
    {
    }
    public WebhookSubscription(
        Guid id,
        string webhookUri,
        string secret,
        string webhooks,
        string headers,
        Guid? tenantId = null) : base(id)
    {
        WebhookUri = webhookUri;
        Secret = secret;
        Webhooks = webhooks;
        Headers = headers;
        TenantId = tenantId;

        IsActive = true;
    }
}
