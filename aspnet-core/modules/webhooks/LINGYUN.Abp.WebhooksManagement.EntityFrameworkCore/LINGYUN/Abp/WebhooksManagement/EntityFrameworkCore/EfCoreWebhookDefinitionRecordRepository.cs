using Microsoft.EntityFrameworkCore;
using System;
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

    public async Task<WebhookDefinitionRecord> FindByNameAsync(
        string name,
        CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .OrderBy(x => x.Id)
            .FirstOrDefaultAsync(r => r.Name == name, cancellationToken);
    }
}
