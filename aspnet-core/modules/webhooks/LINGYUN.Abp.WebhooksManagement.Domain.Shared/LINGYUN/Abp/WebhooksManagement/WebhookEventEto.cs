using System;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Abp.WebhooksManagement;

[Serializable]
public class WebhookEventEto : IMultiTenant
{
    public Guid Id { get; set; }
    public Guid? TenantId { get; set; }
    public string WebhookName { get; set; }
}
