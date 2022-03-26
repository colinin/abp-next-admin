using System;
using Volo.Abp.Application.Services;

namespace LINGYUN.Abp.WebhooksManagement;

public interface IWebhookSubscriptionAppService :
    ICrudAppService<
        WebhookSubscriptionDto,
        Guid,
        WebhookSubscriptionGetListInput,
        WebhookSubscriptionCreateInput,
        WebhookSubscriptionUpdateInput>
{
}
