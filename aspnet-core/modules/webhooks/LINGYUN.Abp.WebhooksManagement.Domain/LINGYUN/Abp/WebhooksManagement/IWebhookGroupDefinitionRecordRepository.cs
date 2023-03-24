using System;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace LINGYUN.Abp.WebhooksManagement;
public interface IWebhookGroupDefinitionRecordRepository : IBasicRepository<WebhookGroupDefinitionRecord, Guid>
{
    Task<WebhookGroupDefinitionRecord> FindByNameAsync(string name, CancellationToken cancellationToken = default);
}
