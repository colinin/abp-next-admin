using LINGYUN.Abp.Webhooks;
using LINGYUN.Abp.WebhooksManagement.Authorization;
using LINGYUN.Abp.WebhooksManagement.Extensions;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
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

    [Authorize(WebhooksManagementPermissions.WebhooksSendAttempts.Delete)]
    public async virtual Task DeleteManyAsync(WebhookSendRecordDeleteManyInput input)
    {
        var sendRecords = await RecordRepository.GetListAsync(x => input.RecordIds.Contains(x.Id));

        await RecordRepository.DeleteManyAsync(sendRecords);
    }

    public async virtual Task<PagedResultDto<WebhookSendRecordDto>> GetListAsync(WebhookSendRecordGetListInput input)
    {
        var specification = new WebhookSendRecordGetListSpecification(input);
        var totalCount = await RecordRepository.GetCountAsync(specification);
        var sendRecords = await RecordRepository.GetListAsync(specification,
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

    [Authorize(WebhooksManagementPermissions.WebhooksSendAttempts.Resend)]
    public async virtual Task ResendManyAsync(WebhookSendRecordResendManyInput input)
    {
        foreach (var recordId in input.RecordIds)
        {
            await ResendAsync(recordId);
        }
    }

    private class WebhookSendRecordGetListSpecification : Volo.Abp.Specifications.Specification<WebhookSendRecord>
    {
        protected WebhookSendRecordGetListInput Filter { get; }

        public WebhookSendRecordGetListSpecification(WebhookSendRecordGetListInput filter)
        {
            Filter = filter;
        }

        public override Expression<Func<WebhookSendRecord, bool>> ToExpression()
        {
            Expression<Func<WebhookSendRecord, bool>> expression = _ => true;

            return expression
                .AndIf(Filter.TenantId.HasValue, x => x.TenantId == Filter.TenantId)
                .AndIf(Filter.State == true, x => x.ResponseStatusCode > HttpStatusCode.Continue && x.ResponseStatusCode < HttpStatusCode.BadRequest)
                .AndIf(Filter.State == false, x => x.ResponseStatusCode >= HttpStatusCode.BadRequest && x.ResponseStatusCode <= HttpStatusCode.NetworkAuthenticationRequired)
                .AndIf(Filter.WebhookEventId.HasValue, x => x.WebhookEventId == Filter.WebhookEventId)
                .AndIf(Filter.SubscriptionId.HasValue, x => x.WebhookSubscriptionId == Filter.SubscriptionId)
                .AndIf(Filter.ResponseStatusCode.HasValue, x => x.ResponseStatusCode == Filter.ResponseStatusCode)
                .AndIf(Filter.BeginCreationTime.HasValue, x => x.CreationTime >= Filter.BeginCreationTime)
                .AndIf(Filter.EndCreationTime.HasValue, x => x.CreationTime <= Filter.EndCreationTime)
                .AndIf(!Filter.Filter.IsNullOrWhiteSpace(), x => x.Response.Contains(Filter.Filter));
        }
    }
}
