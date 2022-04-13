namespace LINGYUN.Abp.WebhooksManagement.Extensions;
public static class WebhookEventRecordExtensions
{
    public static WebhookEventRecordDto ToWebhookEventRecordDto(this WebhookEventRecord eventRecord)
    {
        return new WebhookEventRecordDto
        {
            Id = eventRecord.Id,
            CreationTime = eventRecord.CreationTime,
            Data = eventRecord.Data,
            TenantId = eventRecord.TenantId,
            WebhookName = eventRecord.WebhookName
        };
    }
}
