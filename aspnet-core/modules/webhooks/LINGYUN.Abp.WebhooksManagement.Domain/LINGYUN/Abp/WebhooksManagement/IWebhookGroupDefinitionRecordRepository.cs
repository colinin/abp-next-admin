using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Specifications;

namespace LINGYUN.Abp.WebhooksManagement;
public interface IWebhookGroupDefinitionRecordRepository : IBasicRepository<WebhookGroupDefinitionRecord, Guid>
{
    Task<WebhookGroupDefinitionRecord> FindByNameAsync(
        string name, 
        CancellationToken cancellationToken = default);

    Task<List<WebhookGroupDefinitionRecord>> GetListAsync(
        ISpecification<WebhookGroupDefinitionRecord> specification,
        CancellationToken cancellationToken = default);
}
