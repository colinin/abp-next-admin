using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LINGYUN.Abp.Webhooks
{
    public class NullWebhookSendAttemptStore : IWebhookSendAttemptStore
    {
        public static NullWebhookSendAttemptStore Instance = new NullWebhookSendAttemptStore();

        public Task InsertAsync(WebhookSendAttempt webhookSendAttempt)
        {
            return Task.CompletedTask;
        }

        public Task UpdateAsync(WebhookSendAttempt webhookSendAttempt)
        {
            return Task.CompletedTask;
        }

        public Task<WebhookSendAttempt> GetAsync(Guid? tenantId, Guid id)
        {
            return Task.FromResult<WebhookSendAttempt>(default);
        }

        public Task<int> GetSendAttemptCountAsync(Guid? tenantId, Guid webhookId, Guid webhookSubscriptionId)
        {
            return Task.FromResult(int.MaxValue);
        }

        public Task<bool> HasXConsecutiveFailAsync(Guid? tenantId, Guid subscriptionId, int searchCount)
        {
            return default;
        }

        public Task<(int TotalCount, IReadOnlyCollection<WebhookSendAttempt> Webhooks)> GetAllSendAttemptsBySubscriptionAsPagedListAsync(Guid? tenantId, Guid subscriptionId, int maxResultCount,
            int skipCount)
        {
            return Task.FromResult(ValueTuple.Create(0, new List<WebhookSendAttempt>() as IReadOnlyCollection<WebhookSendAttempt>));
        }

        public Task<List<WebhookSendAttempt>> GetAllSendAttemptsByWebhookEventIdAsync(Guid? tenantId, Guid webhookEventId)
        {
            return Task.FromResult(new List<WebhookSendAttempt>());
        }
    }
}
