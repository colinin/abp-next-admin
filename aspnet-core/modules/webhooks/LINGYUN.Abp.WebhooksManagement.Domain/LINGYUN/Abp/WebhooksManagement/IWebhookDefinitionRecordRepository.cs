using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace LINGYUN.Abp.WebhooksManagement;

public interface IWebhookDefinitionRecordRepository : IBasicRepository<WebhookDefinitionRecord, Guid>
{
    Task<WebhookDefinitionRecord> FindByNameAsync(
        string name,
        CancellationToken cancellationToken = default);

    Task<List<WebhookDefinitionRecord>> GetAvailableListAsync(CancellationToken cancellationToken = default);
}
