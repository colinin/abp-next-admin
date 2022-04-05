using System;
using System.Collections.Generic;

namespace LINGYUN.Abp.Webhooks
{
    public class WebhookSubscriptionInfo
    {
        public Guid Id { get; set; }
        /// <summary>
        /// Subscribed Tenant's id .
        /// </summary>
        public Guid? TenantId { get; set; }

        /// <summary>
        /// Subscription webhook endpoint
        /// </summary>
        public string WebhookUri { get; set; }

        /// <summary>
        /// Webhook secret
        /// </summary>
        public string Secret { get; set; }

        /// <summary>
        /// Is subscription active
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Subscribed webhook definitions unique names.It contains webhook definitions list as json
        /// <para>
        /// Do not change it manually.
        /// Use <see cref=" WebhookSubscriptionInfoExtensions.GetSubscribedWebhooks"/>, 
        /// <see cref=" WebhookSubscriptionInfoExtensions.SubscribeWebhook"/>, 
        /// <see cref="WebhookSubscriptionInfoExtensions.UnsubscribeWebhook"/> and 
        /// <see cref="WebhookSubscriptionInfoExtensions.RemoveAllSubscribedWebhooks"/> to change it.
        /// </para> 
        /// </summary>
        public List<string> Webhooks { get; set; }

        /// <summary>
        /// Gets a set of additional HTTP headers.That headers will be sent with the webhook. It contains webhook header dictionary as json
        /// <para>
        /// Do not change it manually.
        /// Use <see cref=" WebhookSubscriptionInfoExtensions.GetWebhookHeaders"/>, 
        /// <see cref="WebhookSubscriptionInfoExtensions.AddWebhookHeader"/>, 
        /// <see cref="WebhookSubscriptionInfoExtensions.RemoveWebhookHeader"/>, 
        /// <see cref="WebhookSubscriptionInfoExtensions.RemoveAllWebhookHeaders"/> to change it.
        /// </para> 
        /// </summary>
        public IDictionary<string, string> Headers { get; set; }

        public WebhookSubscriptionInfo()
        {
            IsActive = true;
            Headers = new Dictionary<string, string>();
            Webhooks = new List<string>();
        }
    }
}