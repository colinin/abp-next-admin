using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Specifications;

namespace LINGYUN.Abp.WebhooksManagement;

public interface IWebhookSendRecordRepository : IRepository<WebhookSendRecord, Guid>
{
    Task<int> GetCountAsync(
        ISpecification<WebhookSendRecord> specification, 
        CancellationToken cancellationToken = default);

    Task<List<WebhookSendRecord>> GetListAsync(
        ISpecification<WebhookSendRecord> specification, 
        string sorting = $"{nameof(WebhookSendRecord.CreationTime)} DESC",
        int maxResultCount = 10,
        int skipCount = 10,
        bool includeDetails = false,
        CancellationToken cancellationToken = default);
}
