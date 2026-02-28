using Newtonsoft.Json;
using Riok.Mapperly.Abstractions;
using System;
using System.Collections.Generic;
using Volo.Abp.Mapperly;

namespace LINGYUN.Abp.WebhooksManagement;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class WebhookEventRecordToWebhookEventRecordDtoMapper : MapperBase<WebhookEventRecord, WebhookEventRecordDto>
{
    public override partial WebhookEventRecordDto Map(WebhookEventRecord source);
    public override partial void Map(WebhookEventRecord source, WebhookEventRecordDto destination);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class WebhookSendRecordToWebhookSendRecordDtoMapper : MapperBase<WebhookSendRecord, WebhookSendRecordDto>
{
    [MapperIgnoreTarget(nameof(WebhookSendRecord.RequestHeaders))]
    [MapperIgnoreTarget(nameof(WebhookSendRecord.ResponseHeaders))]
    public override partial WebhookSendRecordDto Map(WebhookSendRecord source);

    [MapperIgnoreTarget(nameof(WebhookSendRecord.RequestHeaders))]
    [MapperIgnoreTarget(nameof(WebhookSendRecord.ResponseHeaders))]
    public override partial void Map(WebhookSendRecord source, WebhookSendRecordDto destination);

    public override void AfterMap(WebhookSendRecord source, WebhookSendRecordDto destination)
    {
        destination.RequestHeaders = TryGetRequestHeaders(source.RequestHeaders);
        destination.ResponseHeaders = TryGetResponseHeaders(source.ResponseHeaders);
    }

    [UserMapping(Ignore = true)]
    private static IDictionary<string, string> TryGetRequestHeaders(string requestHeaders)
    {
        var result = new Dictionary<string, string>();

        if (!requestHeaders.IsNullOrWhiteSpace())
        {
            try
            {
                result = JsonConvert.DeserializeObject<Dictionary<string, string>>(requestHeaders);
            }
            catch { }
        }

        return result;
    }

    [UserMapping(Ignore = true)]
    private static IDictionary<string, string> TryGetResponseHeaders(string responseHeaders)
    {
        var result = new Dictionary<string, string>();

        if (!responseHeaders.IsNullOrWhiteSpace())
        {
            try
            {
                result = JsonConvert.DeserializeObject<Dictionary<string, string>>(responseHeaders);
            }
            catch { }
        }

        return result;
    }
}
