using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Specifications;

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
        ISpecification<WebhookSubscription> specification,
        CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .Where(specification.ToExpression())
            .CountAsync(GetCancellationToken(cancellationToken));
    }

    public async virtual Task<List<WebhookSubscription>> GetListAsync(
        ISpecification<WebhookSubscription> specification,
        string sorting = $"{nameof(WebhookSubscription.CreationTime)} DESC",
        int maxResultCount = 10,
        int skipCount = 0,
        CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .Where(specification.ToExpression())
            .OrderBy(sorting ?? $"{nameof(WebhookSubscription.CreationTime)} DESC")
            .PageBy(skipCount, maxResultCount)
            .ToListAsync(GetCancellationToken(cancellationToken));
    }
}
