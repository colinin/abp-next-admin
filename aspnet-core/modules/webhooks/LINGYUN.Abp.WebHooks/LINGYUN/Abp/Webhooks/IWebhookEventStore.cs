using System;
using System.Threading.Tasks;

namespace LINGYUN.Abp.Webhooks
{
    public interface IWebhookEventStore
    {
        /// <summary>
        /// Inserts to persistent store
        /// </summary>
        Task<Guid> InsertAndGetIdAsync(WebhookEvent webhookEvent);

        /// <summary>
        /// Gets Webhook info by id
        /// </summary>
        Task<WebhookEvent> GetAsync(Guid? tenantId, Guid id);
    }
}
