using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace LINGYUN.Abp.WebhooksManagement.EntityFrameworkCore;

public class EfCoreWebhookSubscriptionRepository :
    EfCoreRepository<IWebhooksManagementDbContext, WebhookSubscription, Guid>,
    IWebhookSubscriptionRepository
{
    public EfCoreWebhookSubscriptionRepository(
        IDbContextProvider<IWebhooksManagementDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }

    public async virtual Task<bool> IsSubscribedAsync(
        Guid? tenantId,
        string webhookUri,
        string webhookName,
        CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .AnyAsync(x => x.TenantId == tenantId &&
                x.WebhookUri == webhookUri &&
                x.Webhooks.Contains("\"" + webhookName + "\""),
                GetCancellationToken(cancellationToken));
    }

    public async virtual Task<int> GetCountAsync(
        WebhookSubscriptionFilter filter,
        CancellationToken cancellationToken = default)
    {
        return await ApplyFilter(await GetDbSetAsync(), filter)
            .CountAsync(GetCancellationToken(cancellationToken));
    }

    public async virtual Task<List<WebhookSubscription>> GetListAsync(
        WebhookSubscriptionFilter filter,
        string sorting = $"{nameof(WebhookSubscription.CreationTime)} DESC",
        int maxResultCount = 10,
        int skipCount = 0,
        CancellationToken cancellationToken = default)
    {
        return await ApplyFilter(await GetDbSetAsync(), filter)
            .OrderBy(sorting ?? $"{nameof(WebhookSubscription.CreationTime)} DESC")
            .PageBy(skipCount, maxResultCount)
            .ToListAsync(GetCancellationToken(cancellationToken));
    }

    protected virtual IQueryable<WebhookSubscription> ApplyFilter(
        IQueryable<WebhookSubscription> queryable,
        WebhookSubscriptionFilter filter)
    {
        return queryable
            .WhereIf(filter.TenantId.HasValue, x => x.TenantId == filter.TenantId)
            .WhereIf(filter.IsActive.HasValue, x => x.IsActive == filter.IsActive)
            .WhereIf(!filter.WebhookUri.IsNullOrWhiteSpace(), x => x.WebhookUri == filter.WebhookUri)
            .WhereIf(!filter.Secret.IsNullOrWhiteSpace(), x => x.Secret == filter.Secret)
            .WhereIf(!filter.Webhooks.IsNullOrWhiteSpace(), x => x.Webhooks.Contains("\"" + filter.Webhooks + "\""))
            .WhereIf(filter.BeginCreationTime.HasValue, x => x.CreationTime.CompareTo(filter.BeginCreationTime) >= 0)
            .WhereIf(filter.EndCreationTime.HasValue, x => x.CreationTime.CompareTo(filter.EndCreationTime) <= 0)
            .WhereIf(!filter.Filter.IsNullOrWhiteSpace(), x => x.WebhookUri.Contains(filter.Filter) || 
                x.Secret.Contains(filter.Filter) || x.Webhooks.Contains(filter.Filter));
    }
}
