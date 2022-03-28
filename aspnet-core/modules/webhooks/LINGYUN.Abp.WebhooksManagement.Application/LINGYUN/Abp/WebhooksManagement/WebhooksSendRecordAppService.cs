using LINGYUN.Abp.Webhooks;
using LINGYUN.Abp.WebhooksManagement.Authorization;
using LINGYUN.Abp.WebhooksManagement.Extensions;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.BackgroundJobs;

namespace LINGYUN.Abp.WebhooksManagement;

[Authorize(WebhooksManagementPermissions.WebhooksSendAttempts.Default)]
public class WebhooksSendRecordAppService : WebhooksManagementAppServiceBase, IWebhooksSendRecordAppService
{
    protected IBackgroundJobManager BackgroundJobManager => LazyServiceProvider.LazyGetRequiredService<IBackgroundJobManager>();
    protected IWebhookEventRecordRepository EventRepository => LazyServiceProvider.LazyGetRequiredService<IWebhookEventRecordRepository>();
    protected IWebhookSubscriptionRepository SubscriptionRepository => LazyServiceProvider.LazyGetRequiredService<IWebhookSubscriptionRepository>();


    protected IWebhookSendRecordRepository RecordRepository { get; }

    public WebhooksSendRecordAppService(
        IWebhookSendRecordRepository recordRepository)
    {
        RecordRepository = recordRepository;
    }

    public async virtual Task<WebhookSendRecordDto> GetAsync(Guid id)
    {
        var sendRecord = await RecordRepository.GetAsync(id);

        return ObjectMapper.Map<WebhookSendRecord, WebhookSendRecordDto>(sendRecord);
    }

    public async virtual Task<PagedResultDto<WebhookSendRecordDto>> GetListAsync(WebhookSendRecordGetListInput input)
    {
        var filter = new WebhookSendRecordFilter
        {
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
            ObjectMapper.Map<List<WebhookSendRecord>, List<WebhookSendRecordDto>>(sendRecords));
    }

    [Authorize(WebhooksManagementPermissions.WebhooksSendAttempts.Resend)]
    public async virtual Task ResendAsync(Guid id)
    {
        var sendRecord = await RecordRepository.GetAsync(id);
        var sendEvent = await EventRepository.GetAsync(sendRecord.WebhookEventId);
        var subscription = await SubscriptionRepository.GetAsync(sendRecord.WebhookSubscriptionId);

        await BackgroundJobManager.EnqueueAsync(new WebhookSenderArgs
        {
            TenantId = CurrentTenant.Id,
            WebhookSubscriptionId = sendRecord.WebhookSubscriptionId,
            WebhookEventId = sendRecord.WebhookEventId,
            WebhookName = sendEvent.WebhookName,
            WebhookUri = subscription.WebhookUri,
            Data = sendEvent.Data,
            Headers = subscription.GetWebhookHeaders(),
            Secret = subscription.Secret,
        });
    }
}
