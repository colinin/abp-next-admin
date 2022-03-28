using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace LINGYUN.Abp.WebhooksManagement;

public interface IWebhookSendRecordRepository : IRepository<WebhookSendRecord, Guid>
{
    Task<int> GetCountAsync(
        WebhookSendRecordFilter filter, 
        CancellationToken cancellationToken = default);

    Task<List<WebhookSendRecord>> GetListAsync(
        WebhookSendRecordFilter filter, 
        string sorting = nameof(WebhookSendRecord.CreationTime),
        int maxResultCount = 10,
        int skipCount = 10,
        bool includeDetails = false,
        CancellationToken cancellationToken = default);
}
