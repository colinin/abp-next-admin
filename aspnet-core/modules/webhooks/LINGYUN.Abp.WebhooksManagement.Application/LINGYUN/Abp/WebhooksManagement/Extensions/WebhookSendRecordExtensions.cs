using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace LINGYUN.Abp.WebhooksManagement.Extensions;
public static class WebhookSendRecordExtensions 
{
    public static WebhookSendRecordDto ToWebhookSendRecordDto(this WebhookSendRecord sendRecord)
    {
        var dto = new WebhookSendRecordDto
        {
            Id = sendRecord.Id,
            TenantId = sendRecord.TenantId,
            CreationTime = sendRecord.CreationTime,
            ResponseStatusCode = sendRecord.ResponseStatusCode,
            SendExactSameData = sendRecord.SendExactSameData,
            WebhookEventId = sendRecord.WebhookEventId,
            WebhookSubscriptionId = sendRecord.WebhookSubscriptionId,
            LastModificationTime = sendRecord.LastModificationTime,
            RequestHeaders = sendRecord.GetRequestHeaders(),
            ResponseHeaders = sendRecord.GetResponseHeaders(),
            Response = sendRecord.Response,
        };

        if (sendRecord.WebhookEvent != null)
        {
            dto.WebhookEvent = sendRecord.WebhookEvent.ToWebhookEventRecordDto();
        }

        return dto;
    }

    public static IDictionary<string, string> GetRequestHeaders(this WebhookSendRecord sendRecord)
    {
        if (sendRecord.RequestHeaders.IsNullOrWhiteSpace())
        {
            return new Dictionary<string, string>();
        }

        return JsonConvert.DeserializeObject<Dictionary<string, string>>(sendRecord.RequestHeaders);
    }

    public static IDictionary<string, string> GetResponseHeaders(this WebhookSendRecord sendRecord)
    {
        if (sendRecord.ResponseHeaders.IsNullOrWhiteSpace())
        {
            return new Dictionary<string, string>();
        }

        return JsonConvert.DeserializeObject<Dictionary<string, string>>(sendRecord.ResponseHeaders);
    }
}
