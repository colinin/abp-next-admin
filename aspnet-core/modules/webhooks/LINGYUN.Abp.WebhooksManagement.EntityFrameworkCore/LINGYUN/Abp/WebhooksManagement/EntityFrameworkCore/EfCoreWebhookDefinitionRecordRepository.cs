using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace LINGYUN.Abp.WebhooksManagement.EntityFrameworkCore;

public class EfCoreWebhookDefinitionRecordRepository :
    EfCoreRepository<IWebhooksManagementDbContext, WebhookDefinitionRecord, Guid>,
    IWebhookDefinitionRecordRepository
{
    public EfCoreWebhookDefinitionRecordRepository(
        IDbContextProvider<IWebhooksManagementDbContext> dbContextProvider)
        : base(dbContextProvider)
    {
    }

    public async virtual Task<WebhookDefinitionRecord> FindByNameAsync(
        string name,
        CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .OrderBy(x => x.Id)
            .FirstOrDefaultAsync(r => r.Name == name, GetCancellationToken(cancellationToken));
    }

    public async virtual Task<List<WebhookDefinitionRecord>> GetAvailableListAsync(CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .Where(x => x.IsEnabled == true)
            .ToListAsync(GetCancellationToken(cancellationToken));
    }
}
