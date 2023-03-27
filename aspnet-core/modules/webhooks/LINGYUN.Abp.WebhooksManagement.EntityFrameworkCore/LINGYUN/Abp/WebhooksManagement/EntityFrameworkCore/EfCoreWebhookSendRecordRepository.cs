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

public class EfCoreWebhookSendRecordRepository :
    EfCoreRepository<IWebhooksManagementDbContext, WebhookSendRecord, Guid>,
    IWebhookSendRecordRepository
{
    public EfCoreWebhookSendRecordRepository(
        IDbContextProvider<IWebhooksManagementDbContext> dbContextProvider)
        : base(dbContextProvider)
    {
    }

    public async virtual Task<int> GetCountAsync(
        ISpecification<WebhookSendRecord> specification, 
        CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .Where(specification.ToExpression())
            .CountAsync(GetCancellationToken(cancellationToken));

    }

    public async virtual Task<List<WebhookSendRecord>> GetListAsync(
        ISpecification<WebhookSendRecord> specification, 
        string sorting = $"{nameof(WebhookSendRecord.CreationTime)} DESC", 
        int maxResultCount = 10, 
        int skipCount = 10,
        bool includeDetails = false,
        CancellationToken cancellationToken = default)
    {
        if (sorting.IsNullOrWhiteSpace())
        {
            sorting = $"{nameof(WebhookSendRecord.CreationTime)} DESC";
        }
        return await (await GetDbSetAsync())
            .IncludeDetails(includeDetails)
            .Where(specification.ToExpression())
            .OrderBy(sorting)
            .PageBy(skipCount, maxResultCount)
            .ToListAsync(GetCancellationToken(cancellationToken));
    }

    public async override Task<IQueryable<WebhookSendRecord>> WithDetailsAsync()
    {
        return (await base.WithDetailsAsync()).IncludeDetails();
    }

    protected virtual IQueryable<WebhookSendRecord> ApplyFilter(
        IQueryable<WebhookSendRecord> queryable,
        WebhookSendRecordFilter filter)
    {
        return queryable
            .WhereIf(filter.TenantId.HasValue, x => x.TenantId == filter.TenantId)
            .WhereIf(filter.WebhookEventId.HasValue, x => x.WebhookEventId == filter.WebhookEventId)
            .WhereIf(filter.SubscriptionId.HasValue, x => x.WebhookSubscriptionId == filter.SubscriptionId)
            .WhereIf(filter.ResponseStatusCode.HasValue, x => x.ResponseStatusCode == filter.ResponseStatusCode)
            .WhereIf(filter.BeginCreationTime.HasValue, x => x.CreationTime.CompareTo(filter.BeginCreationTime) >= 0)
            .WhereIf(filter.EndCreationTime.HasValue, x => x.CreationTime.CompareTo(filter.EndCreationTime) <= 0)
            .WhereIf(!filter.Filter.IsNullOrWhiteSpace(), x => x.Response.Contains(filter.Filter));
    }
}
