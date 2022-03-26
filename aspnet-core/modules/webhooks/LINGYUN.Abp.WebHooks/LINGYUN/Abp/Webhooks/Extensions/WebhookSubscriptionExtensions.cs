using System.Collections.Generic;

namespace LINGYUN.Abp.Webhooks.Extensions
{
    public static class WebhookSubscriptionExtensions
    {
        /// <summary>
        /// checks if subscribed to given webhook
        /// </summary>
        /// <returns></returns>
        public static bool IsSubscribed(this WebhookSubscriptionInfo webhookSubscription, string webhookName)
        {
            if (webhookSubscription.Webhooks.IsNullOrEmpty())
            {
                return false;
            }

            return webhookSubscription.Webhooks.Contains(webhookName);
        }
    }
}
