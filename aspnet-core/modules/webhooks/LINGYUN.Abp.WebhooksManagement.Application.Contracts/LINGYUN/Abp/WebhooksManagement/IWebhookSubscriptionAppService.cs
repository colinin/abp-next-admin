using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
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
    Task<ListResultDto<WebhookAvailableGroupDto>> GetAllAvailableWebhooksAsync();
}
