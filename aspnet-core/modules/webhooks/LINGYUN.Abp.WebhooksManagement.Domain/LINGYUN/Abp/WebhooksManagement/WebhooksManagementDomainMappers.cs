using LINGYUN.Abp.Webhooks;
using Riok.Mapperly.Abstractions;
using Volo.Abp.Mapperly;

namespace LINGYUN.Abp.WebhooksManagement;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class WebhookEventRecordToWebhookEventEtoMapper : MapperBase<WebhookEventRecord, WebhookEventEto>
{
    public override partial WebhookEventEto Map(WebhookEventRecord source);
    public override partial void Map(WebhookEventRecord source, WebhookEventEto destination);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class WebhookSendRecordToWebhookSendAttemptEtoMapper : MapperBase<WebhookSendRecord, WebhookSendAttemptEto>
{
    public override partial WebhookSendAttemptEto Map(WebhookSendRecord source);
    public override partial void Map(WebhookSendRecord source, WebhookSendAttemptEto destination);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class WebhookSubscriptionToWebhookSubscriptionEtoMapper : MapperBase<WebhookSubscription, WebhookSubscriptionEto>
{
    public override partial WebhookSubscriptionEto Map(WebhookSubscription source);
    public override partial void Map(WebhookSubscription source, WebhookSubscriptionEto destination);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class WebhookEventRecordToWebhookEventMapper : MapperBase<WebhookEventRecord, WebhookEvent>
{
    public override partial WebhookEvent Map(WebhookEventRecord source);
    public override partial void Map(WebhookEventRecord source, WebhookEvent destination);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class WebhookSendRecordToWebhookSendAttemptMapper : MapperBase<WebhookSendRecord, WebhookSendAttempt>
{
    public override partial WebhookSendAttempt Map(WebhookSendRecord source);
    public override partial void Map(WebhookSendRecord source, WebhookSendAttempt destination);
}
