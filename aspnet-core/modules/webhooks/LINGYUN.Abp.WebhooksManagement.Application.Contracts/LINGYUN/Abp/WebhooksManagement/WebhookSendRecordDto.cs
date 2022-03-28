using System;
using System.Net;
using Volo.Abp.Application.Dtos;

namespace LINGYUN.Abp.WebhooksManagement;

public class WebhookSendRecordDto : EntityDto<Guid>
{
    public Guid? TenantId { get; set; }

    public Guid WebhookEventId { get; set; }

    public Guid WebhookSubscriptionId { get; set; }

    public string Response { get; set; }

    public HttpStatusCode? ResponseStatusCode { get; set; }

    public DateTime CreationTime { get; set; }

    public DateTime? LastModificationTime { get; set; }

    public WebhookEventRecordDto WebhookEvent { get; set; } = new WebhookEventRecordDto();
}
