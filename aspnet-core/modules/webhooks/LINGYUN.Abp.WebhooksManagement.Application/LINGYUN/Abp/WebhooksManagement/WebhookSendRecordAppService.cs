using LINGYUN.Abp.Webhooks;
using LINGYUN.Abp.WebhooksManagement.Authorization;
using LINGYUN.Abp.WebhooksManagement.Extensions;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.BackgroundJobs;

namespace LINGYUN.Abp.WebhooksManagement;

[Authorize(WebhooksManagementPermissions.WebhooksSendAttempts.Default)]
public class WebhookSendRecordAppService : WebhooksManagementAppServiceBase, IWebhookSendRecordAppService
{
    protected IBackgroundJobManager BackgroundJobManager => LazyServiceProvider.LazyGetRequiredService<IBackgroundJobManager>();
    protected IWebhookEventRecordRepository EventRepository => LazyServiceProvider.LazyGetRequiredService<IWebhookEventRecordRepository>();
    protected IWebhookSubscriptionRepository SubscriptionRepository => LazyServiceProvider.LazyGetRequiredService<IWebhookSubscriptionRepository>();


    protected IWebhookSendRecordRepository RecordRepository { get; }

    public WebhookSendRecordAppService(
        IWebhookSendRecordRepository recordRepository)
    {
        RecordRepository = recordRepository;
    }

    public async virtual Task<WebhookSendRecordDto> GetAsync(Guid id)
    {
        var sendRecord = await RecordRepository.GetAsync(id);

        return sendRecord.ToWebhookSendRecordDto();
    }

    [Authorize(WebhooksManagementPermissions.WebhooksSendAttempts.Delete)]
    public async virtual Task DeleteAsync(Guid id)
    {
        var sendRecord = await RecordRepository.GetAsync(id);

        await RecordRepository.DeleteAsync(sendRecord);
    }

    public async virtual Task<PagedResultDto<WebhookSendRecordDto>> GetListAsync(WebhookSendRecordGetListInput input)
    {
        var filter = new WebhookSendRecordFilter
        {
            TenantId = input.TenantId,
            SubscriptionId = input.SubscriptionId,
            ResponseStatusCode = input.ResponseStatusCode,
            BeginCreationTime = input.BeginCreationTime,
            EndCreationTime = input.EndCreationTime,
            WebhookEventId = input.WebhookEventId,
            Filter = input.Filter
        };
        var totalCount = await RecordRepository.GetCountAsync(filter);
        var sendRecords = await RecordRepository.GetListAsync(filter,
            input.Sorting, input.MaxResultCount, input.SkipCount);

        return new PagedResultDto<WebhookSendRecordDto>(totalCount,
            sendRecords.Select(sendRecord => sendRecord.ToWebhookSendRecordDto()).ToList());
    }

    [Authorize(WebhooksManagementPermissions.WebhooksSendAttempts.Resend)]
    public async virtual Task ResendAsync(Guid id)
    {
        var sendRecord = await RecordRepository.GetAsync(id);
        var sendEvent = await EventRepository.GetAsync(sendRecord.WebhookEventId);
        var subscription = await SubscriptionRepository.GetAsync(sendRecord.WebhookSubscriptionId);

        var headersToSend = new Dictionary<string, string>();
        if (!sendRecord.RequestHeaders.IsNullOrWhiteSpace())
        {
            headersToSend = JsonConvert.DeserializeObject<Dictionary<string, string>>(sendRecord.RequestHeaders);
        }

        using (CurrentTenant.Change(sendRecord.TenantId))
        {
            await BackgroundJobManager.EnqueueAsync(new WebhookSenderArgs
            {
                TenantId = CurrentTenant.Id,
                WebhookSubscriptionId = sendRecord.WebhookSubscriptionId,
                WebhookEventId = sendRecord.WebhookEventId,
                WebhookName = sendEvent.WebhookName,
                WebhookUri = subscription.WebhookUri,
                Data = sendEvent.Data,
                Headers = headersToSend,
                Secret = subscription.Secret,
                TryOnce = true,
                SendExactSameData = sendRecord.SendExactSameData
            });
        }
    }
}
