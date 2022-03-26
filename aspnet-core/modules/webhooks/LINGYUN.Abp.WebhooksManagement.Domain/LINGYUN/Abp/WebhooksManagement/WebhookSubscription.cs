using System;
using Volo.Abp;
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
        Secret = Check.NotNullOrWhiteSpace(secret, nameof(secret), WebhookSubscriptionConsts.MaxSecretLength);
        SetWebhookUri(webhookUri);
        SetWebhooks(webhooks);
        SetHeaders(headers);
        TenantId = tenantId;

        IsActive = true;
    }

    public void SetWebhookUri(string webhookUri)
    {
        WebhookUri = Check.NotNullOrWhiteSpace(webhookUri, nameof(webhookUri), WebhookSubscriptionConsts.MaxWebhookUriLength);
    }

    public void SetWebhooks(string webhooks)
    {
        Webhooks = Check.Length(webhooks, nameof(webhooks), WebhookSubscriptionConsts.MaxWebhooksLength);
    }

    public void SetHeaders(string headers)
    {
        Headers = Check.Length(headers, nameof(headers), WebhookSubscriptionConsts.MaxHeadersLength);
    }
}
