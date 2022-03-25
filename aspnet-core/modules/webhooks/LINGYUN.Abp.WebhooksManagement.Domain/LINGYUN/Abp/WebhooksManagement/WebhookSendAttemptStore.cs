using LINGYUN.Abp.Webhooks;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;
using Volo.Abp.Linq;
using Volo.Abp.ObjectMapping;
using Volo.Abp.Uow;

namespace LINGYUN.Abp.WebhooksManagement;

public class WebhookSendAttemptStore : DomainService, IWebhookSendAttemptStore
{
    protected IObjectMapper<WebhooksManagementDomainModule> ObjectMapper => LazyServiceProvider.LazyGetRequiredService<IObjectMapper<WebhooksManagementDomainModule>>();
    protected IAsyncQueryableExecuter AsyncQueryableExecuter => LazyServiceProvider.LazyGetRequiredService<IAsyncQueryableExecuter>();

    protected IUnitOfWorkManager UnitOfWorkManager { get; }
    protected IWebhookSendRecordRepository WebhookSendAttemptRepository { get; }

    public WebhookSendAttemptStore(
        IUnitOfWorkManager unitOfWorkManager,
        IWebhookSendRecordRepository webhookSendAttemptRepository)
    {
        UnitOfWorkManager = unitOfWorkManager;
        WebhookSendAttemptRepository = webhookSendAttemptRepository;
    }

    public async virtual Task<(int TotalCount, IReadOnlyCollection<WebhookSendAttempt> Webhooks)> GetAllSendAttemptsBySubscriptionAsPagedListAsync(
        Guid? tenantId,
        Guid subscriptionId, 
        int maxResultCount,
        int skipCount)
    {
        (int TotalCount, IReadOnlyCollection<WebhookSendAttempt> Webhooks) sendAttempts;

        using (var uow = UnitOfWorkManager.Begin())
        {
            using (CurrentTenant.Change(tenantId))
            {
                var filter = new WebhookSendRecordFilter
                {
                    SubscriptionId = subscriptionId,
                };
                var totalCount = await WebhookSendAttemptRepository.GetCountAsync(filter);

                var list = await WebhookSendAttemptRepository.GetListAsync(
                    filter,
                    maxResultCount: maxResultCount,
                    skipCount: skipCount);

                var webHooks = ObjectMapper.Map<List<WebhookSendRecord>, List<WebhookSendAttempt>>(list);

                sendAttempts = ValueTuple.Create(totalCount, webHooks.ToImmutableList());
            }

            await uow.CompleteAsync();
        }

        return sendAttempts;
    }

    public async virtual Task<List<WebhookSendAttempt>> GetAllSendAttemptsByWebhookEventIdAsync(
        Guid? tenantId,
        Guid webhookEventId)
    {
        List<WebhookSendAttempt> sendAttempts;

        using (var uow = UnitOfWorkManager.Begin())
        {
            using (CurrentTenant.Change(tenantId))
            {
                var queryable = await WebhookSendAttemptRepository.GetQueryableAsync();

                var list = await AsyncQueryableExecuter.ToListAsync(queryable
                        .Where(attempt => attempt.WebhookEventId == webhookEventId)
                        .OrderByDescending(attempt => attempt.CreationTime)
                );

                sendAttempts = ObjectMapper.Map<List<WebhookSendRecord>, List<WebhookSendAttempt>>(list);
            }

            await uow.CompleteAsync();
        }

        return sendAttempts;
    }

    public async virtual Task<WebhookSendAttempt> GetAsync(
        Guid? tenantId, 
        Guid id)
    {
        WebhookSendRecord sendAttempt;

        using (var uow = UnitOfWorkManager.Begin())
        {
            using (CurrentTenant.Change(tenantId))
            {
                sendAttempt = await WebhookSendAttemptRepository.GetAsync(id);
            }

            await uow.CompleteAsync();
        }

        return ObjectMapper.Map<WebhookSendRecord, WebhookSendAttempt>(sendAttempt);
    }

    public async virtual Task<int> GetSendAttemptCountAsync(
        Guid? tenantId, 
        Guid webhookEventId,
        Guid webhookSubscriptionId)
    {
        int sendAttemptCount;

        using (var uow = UnitOfWorkManager.Begin())
        {
            using (CurrentTenant.Change(tenantId))
            {
                sendAttemptCount = await WebhookSendAttemptRepository.CountAsync(attempt =>
                        attempt.WebhookEventId == webhookEventId &&
                        attempt.WebhookSubscriptionId == webhookSubscriptionId);
            }

            await uow.CompleteAsync();
        }

        return sendAttemptCount;
    }

    public async virtual Task<bool> HasXConsecutiveFailAsync(
        Guid? tenantId,
        Guid subscriptionId,
        int failCount)
    {
        bool result;

        using (var uow = UnitOfWorkManager.Begin())
        {
            using (CurrentTenant.Change(tenantId))
            {
                if (await WebhookSendAttemptRepository.CountAsync(x => x.WebhookSubscriptionId == subscriptionId) < failCount)
                {
                    result = false;
                }
                else
                {
                    var queryable = await WebhookSendAttemptRepository.GetQueryableAsync();

                    result = !await AsyncQueryableExecuter.AnyAsync(queryable
                            .OrderByDescending(attempt => attempt.CreationTime)
                            .Take(failCount)
                            .Where(attempt => attempt.ResponseStatusCode == HttpStatusCode.OK)
                    );
                }
            }

            await uow.CompleteAsync();
        }

        return result;
    }
}
