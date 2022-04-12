using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Validation;

namespace LINGYUN.Abp.WebhooksManagement;

public class WebhookSubscriptionCreateInput : WebhookSubscriptionCreateOrUpdateInput
{

}

public class WebhookSubscriptionUpdateInput : WebhookSubscriptionCreateOrUpdateInput
{

}

public abstract class WebhookSubscriptionCreateOrUpdateInput
{
    [Required]
    [DynamicStringLength(typeof(WebhookSubscriptionConsts), nameof(WebhookSubscriptionConsts.MaxWebhookUriLength))]
    public string WebhookUri { get; set; }

    [DynamicStringLength(typeof(WebhookSubscriptionConsts), nameof(WebhookSubscriptionConsts.MaxSecretLength))]
    public string Secret { get; set; }

    public bool IsActive { get; set; }

    public Guid? TenantId { get; set; }

    public List<string> Webhooks { get; set; } = new List<string>();

    public Dictionary<string, string> Headers { get; set; } = new Dictionary<string, string>();
}
