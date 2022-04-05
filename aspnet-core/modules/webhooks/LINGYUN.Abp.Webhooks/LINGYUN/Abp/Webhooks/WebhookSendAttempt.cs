using System;
using System.Net;

namespace LINGYUN.Abp.Webhooks
{
    /// <summary>
    /// Table for store webhook work items. Each item stores web hook send attempt of <see cref="WebhookEvent"/> to subscribed tenants
    /// </summary>
    public class WebhookSendAttempt
    {
        public Guid Id { get; set; }

        /// <summary>
        /// <see cref="WebhookEvent"/> foreign id 
        /// </summary>
        public Guid WebhookEventId { get; set; }

        /// <summary>
        /// <see cref="WebhookSubscription"/> foreign id 
        /// </summary>
        public Guid WebhookSubscriptionId { get; set; }

        /// <summary>
        /// Webhook response content that webhook endpoint send back
        /// </summary>
        public string Response { get; set; }

        /// <summary>
        /// Webhook response status code that webhook endpoint send back
        /// </summary>
        public HttpStatusCode? ResponseStatusCode { get; set; }

        public DateTime CreationTime { get; set; }

        public DateTime? LastModificationTime { get; set; }

        public Guid? TenantId { get; set; }
    }
}
