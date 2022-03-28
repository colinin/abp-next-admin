using System;
using Volo.Abp.Application.Dtos;

namespace LINGYUN.Abp.WebhooksManagement;

public class WebhookEventRecordDto : EntityDto<Guid>
{
    public Guid? TenantId { get; set; }
    public string WebhookName { get; set; }
    public string Data { get; set; }
    public DateTime CreationTime { get; set; }
}
