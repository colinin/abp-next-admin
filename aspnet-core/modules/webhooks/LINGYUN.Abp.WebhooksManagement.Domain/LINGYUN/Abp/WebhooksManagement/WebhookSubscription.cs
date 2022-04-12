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
        string webhooks,
        string headers,
        string secret = null,
        Guid? tenantId = null) : base(id)
    {
        SetWebhookUri(webhookUri);
        SetWebhooks(webhooks);
        SetHeaders(headers);
        SetSecret(secret);
        TenantId = tenantId;

        IsActive = true;
    }

    public void SetTenantId(Guid? tenantId = null)
    {
        TenantId = tenantId;
    }

    public void SetSecret(string secret)
    {
        Secret = Check.Length(secret, nameof(secret), WebhookSubscriptionConsts.MaxSecretLength);
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
