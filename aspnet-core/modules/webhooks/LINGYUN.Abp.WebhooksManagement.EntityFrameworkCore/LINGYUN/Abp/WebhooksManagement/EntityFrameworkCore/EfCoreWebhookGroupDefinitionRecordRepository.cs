using System;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

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
}
