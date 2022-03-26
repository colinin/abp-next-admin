using System;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Abp.WebhooksManagement;

[Serializable]
public class WebhookSubscriptionEto : IMultiTenant
{
    public Guid Id { get; set; }

    public Guid? TenantId { get; set; }

    public string WebhookUri { get; set; }
}
