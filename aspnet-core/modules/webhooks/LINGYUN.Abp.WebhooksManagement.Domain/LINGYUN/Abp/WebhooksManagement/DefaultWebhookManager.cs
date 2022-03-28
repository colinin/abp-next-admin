using LINGYUN.Abp.Webhooks;
using System;
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
                webhookSenderArgs.TenantId);

            await WebhookSendAttemptRepository.InsertAsync(record);

            return record.Id;
        }
    }

    [UnitOfWork]
    public async override Task StoreResponseOnWebhookSendAttemptAsync(Guid webhookSendAttemptId, Guid? tenantId, HttpStatusCode? statusCode, string content)
    {
        using (CurrentTenant.Change(tenantId))
        {
            var record = await WebhookSendAttemptRepository.GetAsync(webhookSendAttemptId);
            record.SetResponse(content, statusCode);

            await WebhookSendAttemptRepository.UpdateAsync(record);
        }
    }
}
