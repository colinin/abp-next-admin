using LINGYUN.Abp.Webhooks;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;
using Volo.Abp.ObjectMapping;
using Volo.Abp.Uow;

namespace LINGYUN.Abp.WebhooksManagement;

public class WebhookSendAttemptStore : DomainService, IWebhookSendAttemptStore
{
    private class WebhookSendRecordSpecification : Volo.Abp.Specifications.Specification<WebhookSendRecord>
    {
        protected Guid SubscriptionId { get; }

        public WebhookSendRecordSpecification(
            Guid subscriptionId)
        {
            SubscriptionId = subscriptionId;
        }

        public override Expression<Func<WebhookSendRecord, bool>> ToExpression()
        {
            Expression<Func<WebhookSendRecord, bool>> expression = _ => true;

            return expression.And(x => x.WebhookSubscriptionId == SubscriptionId);
        }
    }

    protected IObjectMapper<WebhooksManagementDomainModule> ObjectMapper => LazyServiceProvider.LazyGetRequiredService<IObjectMapper<WebhooksManagementDomainModule>>();

    protected IWebhookSendRecordRepository WebhookSendAttemptRepository { get; }

    public WebhookSendAttemptStore(
        IWebhookSendRecordRepository webhookSendAttemptRepository)
    {
        WebhookSendAttemptRepository = webhookSendAttemptRepository;
    }

    [UnitOfWork]
    public async virtual Task<(int TotalCount, IReadOnlyCollection<WebhookSendAttempt> Webhooks)> GetAllSendAttemptsBySubscriptionAsPagedListAsync(
        Guid? tenantId,
        Guid subscriptionId, 
        int maxResultCount,
        int skipCount)
    {
        using (CurrentTenant.Change(tenantId))
        {
            var specification = new WebhookSendRecordSpecification(subscriptionId);
            var totalCount = await WebhookSendAttemptRepository.GetCountAsync(specification);

            var list = await WebhookSendAttemptRepository.GetListAsync(
                specification,
                maxResultCount: maxResultCount,
                skipCount: skipCount);

            var webHooks = ObjectMapper.Map<List<WebhookSendRecord>, List<WebhookSendAttempt>>(list);

            return ValueTuple.Create(totalCount, webHooks.ToImmutableList());
        }
    }

    [UnitOfWork]
    public async virtual Task<List<WebhookSendAttempt>> GetAllSendAttemptsByWebhookEventIdAsync(
        Guid? tenantId,
        Guid webhookEventId)
    {
        using (CurrentTenant.Change(tenantId))
        {
            var queryable = await WebhookSendAttemptRepository.GetQueryableAsync();

            var list = await AsyncExecuter.ToListAsync(queryable
                    .Where(attempt => attempt.WebhookEventId == webhookEventId)
                    .OrderByDescending(attempt => attempt.CreationTime)
            );

            return ObjectMapper.Map<List<WebhookSendRecord>, List<WebhookSendAttempt>>(list);
        }
    }

    [UnitOfWork]
    public async virtual Task<WebhookSendAttempt> GetAsync(
        Guid? tenantId, 
        Guid id)
    {
        using (CurrentTenant.Change(tenantId))
        {
            var sendAttempt = await WebhookSendAttemptRepository.GetAsync(id);

            return ObjectMapper.Map<WebhookSendRecord, WebhookSendAttempt>(sendAttempt);
        }
    }

    [UnitOfWork]
    public async virtual Task<int> GetSendAttemptCountAsync(
        Guid? tenantId, 
        Guid webhookEventId,
        Guid webhookSubscriptionId)
    {
        using (CurrentTenant.Change(tenantId))
        {
            return await WebhookSendAttemptRepository.CountAsync(attempt =>
                    attempt.WebhookEventId == webhookEventId &&
                    attempt.WebhookSubscriptionId == webhookSubscriptionId);
        }
    }

    [UnitOfWork]
    public async virtual Task<bool> HasXConsecutiveFailAsync(
        Guid? tenantId,
        Guid subscriptionId,
        int failCount)
    {
        using (CurrentTenant.Change(tenantId))
        {
            if (await WebhookSendAttemptRepository.CountAsync(x => x.WebhookSubscriptionId == subscriptionId) < failCount)
            {
                return false;
            }
            else
            {
                var queryable = await WebhookSendAttemptRepository.GetQueryableAsync();

               return !await AsyncExecuter.AnyAsync(queryable
                        .OrderByDescending(attempt => attempt.CreationTime)
                        .Take(failCount)
                        .Where(attempt => attempt.ResponseStatusCode == HttpStatusCode.OK)
                );
            }
        }
    }
}
