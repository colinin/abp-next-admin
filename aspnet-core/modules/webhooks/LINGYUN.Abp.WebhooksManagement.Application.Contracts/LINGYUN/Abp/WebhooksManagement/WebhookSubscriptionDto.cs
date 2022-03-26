using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace LINGYUN.Abp.WebhooksManagement;

public class WebhookSubscriptionDto : CreationAuditedEntityDto<Guid>
{
    public Guid? TenantId { get; set; }
    public string WebhookUri { get; set; }
    public string Secret { get; set; }
    public bool IsActive { get; set; }
    public List<string> Webhooks { get; set; } = new List<string>();
    public IDictionary<string, string> Headers { get; set; } = new Dictionary<string, string>();
}
