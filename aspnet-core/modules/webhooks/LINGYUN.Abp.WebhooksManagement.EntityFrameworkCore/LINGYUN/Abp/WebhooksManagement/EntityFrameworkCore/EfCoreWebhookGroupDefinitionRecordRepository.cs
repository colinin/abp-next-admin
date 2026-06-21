using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Specifications;

namespace LINGYUN.Abp.WebhooksManagement.EntityFrameworkCore;

public class EfCoreWebhookGroupDefinitionRecordRepository :
    EfCoreRepository<IWebhooksManagementDbContext, WebhookGroupDefinitionRecord, Guid>,
    IWebhookGroupDefinitionRecordRepository
{
    public EfCoreWebhookGroupDefinitionRecordRepository(
        IDbContextProvider<IWebhooksManagementDbContext> dbContextProvider)
        : base(dbContextProvider)
    {
    }

    public async Task<WebhookGroupDefinitionRecord> FindByNameAsync(
        string name,
        CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .OrderBy(x => x.Id)
            .FirstOrDefaultAsync(r => r.Name == name, cancellationToken);
    }

    public async virtual Task<List<WebhookGroupDefinitionRecord>> GetListAsync(
        ISpecification<WebhookGroupDefinitionRecord> specification,
        CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .Where(specification.ToExpression())
            .ToListAsync(GetCancellationToken(cancellationToken));
    }
}
