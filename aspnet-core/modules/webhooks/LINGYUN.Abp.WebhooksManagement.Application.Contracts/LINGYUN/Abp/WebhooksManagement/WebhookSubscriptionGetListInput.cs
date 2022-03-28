using System;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Validation;

namespace LINGYUN.Abp.WebhooksManagement;

public class WebhookSubscriptionGetListInput : PagedAndSortedResultRequestDto
{
    public string Filter { get; set; }

    public Guid? TenantId { get; set; }

    [DynamicStringLength(typeof(WebhookSubscriptionConsts), nameof(WebhookSubscriptionConsts.MaxWebhookUriLength))]
    public string WebhookUri { get; set; }

    [DynamicStringLength(typeof(WebhookSubscriptionConsts), nameof(WebhookSubscriptionConsts.MaxSecretLength))]
    public string Secret { get; set; }

    public bool? IsActive { get; set; }

    public string Webhooks { get; set; }

    public DateTime? BeginCreationTime { get; set; }

    public DateTime? EndCreationTime { get; set; }
}
