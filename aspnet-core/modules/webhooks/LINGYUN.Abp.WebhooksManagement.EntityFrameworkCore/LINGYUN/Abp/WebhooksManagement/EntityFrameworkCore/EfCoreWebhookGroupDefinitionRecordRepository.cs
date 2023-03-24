using System;
using System.Threading.Tasks;
using System.Threading;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using System.Linq;
using Microsoft.EntityFrameworkCore;

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
}
