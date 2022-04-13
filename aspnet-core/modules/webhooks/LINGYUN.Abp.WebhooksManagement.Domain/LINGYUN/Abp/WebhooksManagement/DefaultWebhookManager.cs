using LINGYUN.Abp.Webhooks;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Uow;

namespace LINGYUN.Abp.WebhooksManagement;

public class DefaultWebhookManager : WebhookManager, ITransientDependency
{
    protected ICurrentTenant CurrentTenant { get; }
    protected IGuidGenerator GuidGenerator { get; }
    protected IUnitOfWorkManager UnitOfWorkManager { get; }
    protected IWebhookSendRecordRepository WebhookSendAttemptRepository { get; }
    public DefaultWebhookManager(
        ICurrentTenant currentTenant,
        IGuidGenerator guidGenerator,
        IWebhookSendAttemptStore webhookSendAttemptStore,
        IUnitOfWorkManager unitOfWorkManager,
        IWebhookSendRecordRepository webhookSendAttemptRepository) 
        : base(webhookSendAttemptStore)
    {
        CurrentTenant = currentTenant;
        GuidGenerator = guidGenerator;
        UnitOfWorkManager = unitOfWorkManager;
        WebhookSendAttemptRepository = webhookSendAttemptRepository;
    }

    [UnitOfWork]
    public async override Task<Guid> InsertAndGetIdWebhookSendAttemptAsync(WebhookSenderArgs webhookSenderArgs)
    {
        using (CurrentTenant.Change(webhookSenderArgs.TenantId))
        {
            var record = new WebhookSendRecord(
                GuidGenerator.Create(),
                webhookSenderArgs.WebhookEventId,
                webhookSenderArgs.WebhookSubscriptionId,
                webhookSenderArgs.TenantId)
            {
                SendExactSameData = webhookSenderArgs.SendExactSameData
            };

            await WebhookSendAttemptRepository.InsertAsync(record);

            return record.Id;
        }
    }

    [UnitOfWork]
    public async override Task StoreResponseOnWebhookSendAttemptAsync(
        Guid webhookSendAttemptId,
        Guid? tenantId,
        HttpStatusCode? statusCode, 
        string content,
        IDictionary<string, string> requestHeaders = null,
        IDictionary<string, string> responseHeaders = null)
    {
        using (CurrentTenant.Change(tenantId))
        {
            var reqHeaders = "{}";
            var resHeaders = "{}";
            if (requestHeaders != null)
            {
                reqHeaders = JsonConvert.SerializeObject(requestHeaders);
            }
            if (responseHeaders != null)
            {
                resHeaders = JsonConvert.SerializeObject(responseHeaders);
            }

            var record = await WebhookSendAttemptRepository.GetAsync(webhookSendAttemptId);
            record.SetResponse(content, statusCode, resHeaders);
            // 加入标头信息，便于维护人员调试
            record.SetRequestHeaders(reqHeaders);

            await WebhookSendAttemptRepository.UpdateAsync(record);
        }
    }
}
