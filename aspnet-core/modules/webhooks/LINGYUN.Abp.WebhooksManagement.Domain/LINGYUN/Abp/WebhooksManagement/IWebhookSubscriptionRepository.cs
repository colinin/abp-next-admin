using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace LINGYUN.Abp.WebhooksManagement;

public interface IWebhookSubscriptionRepository : IRepository<WebhookSubscription, Guid>
{
    Task<bool> IsSubscribedAsync(
        Guid? tenantId,
        string webhookUri,
        string webhookName,
        CancellationToken cancellationToken = default);

    Task<int> GetCountAsync(
        WebhookSubscriptionFilter filter,
        CancellationToken cancellationToken = default);

    Task<List<WebhookSubscription>> GetListAsync(
        WebhookSubscriptionFilter filter,
        string sorting = $"{nameof(WebhookSubscription.CreationTime)} DESC",
        int maxResultCount = 10,
        int skipCount = 0,
        CancellationToken cancellationToken = default);
}
