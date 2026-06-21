using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Specifications;

namespace LINGYUN.Abp.WebhooksManagement;

public interface IWebhookDefinitionRecordRepository : IBasicRepository<WebhookDefinitionRecord, Guid>
{
    Task<WebhookDefinitionRecord> FindByNameAsync(
        string name,
        CancellationToken cancellationToken = default);

    Task<List<WebhookDefinitionRecord>> GetAvailableListAsync(
        CancellationToken cancellationToken = default);

    Task<List<WebhookDefinitionRecord>> GetListAsync(
        ISpecification<WebhookDefinitionRecord> specification,
        CancellationToken cancellationToken = default);
}
