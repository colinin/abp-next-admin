using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LINGYUN.Abp.Webhooks
{
    public interface IWebhookSendAttemptStore
    {
        Task<WebhookSendAttempt> GetAsync(Guid? tenantId, Guid id);

        /// <summary>
        /// Returns work item count by given web hook id and subscription id, (How many times publisher tried to send web hook)
        /// </summary>
        Task<int> GetSendAttemptCountAsync(Guid? tenantId, Guid webhookId, Guid webhookSubscriptionId);

        /// <summary>
        /// Checks is there any successful webhook attempt in last <paramref name="searchCount"/> items. Should return true if there are not X number items
        /// </summary>
        Task<bool> HasXConsecutiveFailAsync(Guid? tenantId, Guid subscriptionId, int searchCount);

        Task<(int TotalCount, IReadOnlyCollection<WebhookSendAttempt> Webhooks)> GetAllSendAttemptsBySubscriptionAsPagedListAsync(Guid? tenantId, Guid subscriptionId, int maxResultCount, int skipCount);

        Task<List<WebhookSendAttempt>> GetAllSendAttemptsByWebhookEventIdAsync(Guid? tenantId, Guid webhookEventId);
    }
}
