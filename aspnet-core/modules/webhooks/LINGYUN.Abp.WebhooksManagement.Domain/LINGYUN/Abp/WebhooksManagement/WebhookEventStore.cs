using LINGYUN.Abp.Webhooks;
using System;
using System.Threading.Tasks;
using Volo.Abp.Domain.Services;
using Volo.Abp.ObjectMapping;
using Volo.Abp.Uow;

namespace LINGYUN.Abp.WebhooksManagement;

public class WebhookEventStore : DomainService, IWebhookEventStore
{
    protected IObjectMapper<WebhooksManagementDomainModule> ObjectMapper => LazyServiceProvider.LazyGetRequiredService<IObjectMapper<WebhooksManagementDomainModule>>();

    protected IUnitOfWorkManager UnitOfWorkManager { get; }
    protected IWebhookEventRecordRepository WebhookEventRepository { get; }

    public WebhookEventStore(
        IUnitOfWorkManager unitOfWorkManager,
        IWebhookEventRecordRepository webhookEventRepository)
    {
        UnitOfWorkManager = unitOfWorkManager;
        WebhookEventRepository = webhookEventRepository;
    }

    public async virtual Task<WebhookEvent> GetAsync(Guid? tenantId, Guid id)
    {
        using var uow = UnitOfWorkManager.Begin();
        using (CurrentTenant.Change(tenantId))
        {
            var record = await WebhookEventRepository.GetAsync(id);

            return ObjectMapper.Map<WebhookEventRecord, WebhookEvent>(record);
        }
    }

    public async virtual Task<Guid> InsertAndGetIdAsync(WebhookEvent webhookEvent)
    {
        using var uow = UnitOfWorkManager.Begin();
        using (CurrentTenant.Change(webhookEvent.TenantId))
        {
            var record = new WebhookEventRecord(
                GuidGenerator.Create(),
                webhookEvent.WebhookName,
                webhookEvent.Data,
                webhookEvent.TenantId)
            {
                CreationTime = Clock.Now,
            };

            await WebhookEventRepository.InsertAsync(record);

            await uow.SaveChangesAsync();

            return record.Id;
        }
    }
}
