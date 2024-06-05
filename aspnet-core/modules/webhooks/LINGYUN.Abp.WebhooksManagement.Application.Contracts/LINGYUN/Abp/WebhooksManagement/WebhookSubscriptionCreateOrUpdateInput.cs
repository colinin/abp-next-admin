using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Validation;

namespace LINGYUN.Abp.WebhooksManagement;

public class WebhookSubscriptionCreateInput : WebhookSubscriptionCreateOrUpdateInput
{

}

public class WebhookSubscriptionUpdateInput : WebhookSubscriptionCreateOrUpdateInput, IHasConcurrencyStamp
{
    [StringLength(40)]
    public string ConcurrencyStamp { get; set; }
}

public abstract class WebhookSubscriptionCreateOrUpdateInput
{
    [Required]
    [DynamicStringLength(typeof(WebhookSubscriptionConsts), nameof(WebhookSubscriptionConsts.MaxWebhookUriLength))]
    public string WebhookUri { get; set; }

    [DynamicStringLength(typeof(WebhookSubscriptionConsts), nameof(WebhookSubscriptionConsts.MaxSecretLength))]
    public string Secret { get; set; }

    [DynamicStringLength(typeof(WebhookSubscriptionConsts), nameof(WebhookSubscriptionConsts.MaxDescriptionLength))]
    public string Description { get; set; }

    [DynamicRange(
        typeof(WebhookSubscriptionConsts), 
        typeof(int), 
        nameof(WebhookSubscriptionConsts.TimeoutDurationMinimum), 
        nameof(WebhookSubscriptionConsts.TimeoutDurationMaximum))]
    public int? TimeoutDuration { get; set; }

    public bool IsActive { get; set; }

    public Guid? TenantId { get; set; }

    public List<string> Webhooks { get; set; } = new List<string>();

    public Dictionary<string, string> Headers { get; set; } = new Dictionary<string, string>();
}
