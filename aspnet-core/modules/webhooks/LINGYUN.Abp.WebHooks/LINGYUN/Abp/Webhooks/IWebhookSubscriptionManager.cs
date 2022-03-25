using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LINGYUN.Abp.Webhooks
{
    public interface IWebhookSubscriptionManager
    {
        /// <summary>
        /// Returns subscription for given id. 
        /// </summary>
        /// <param name="id">Unique identifier of <see cref="WebhookSubscriptionInfo"/></param>
        Task<WebhookSubscriptionInfo> GetAsync(Guid id);

        /// <summary>
        /// Returns all subscriptions of tenant
        /// </summary>
        /// <param name="tenantId">
        /// Target tenant id.
        /// </param>
        Task<List<WebhookSubscriptionInfo>> GetAllSubscriptionsAsync(Guid? tenantId);

        /// <summary>
        /// Returns all subscriptions for given webhook.
        /// </summary>
        /// <param name="webhookName"><see cref="WebhookDefinition.Name"/></param>
        /// <param name="tenantId">
        /// Target tenant id.
        /// </param>
        Task<List<WebhookSubscriptionInfo>> GetAllSubscriptionsIfFeaturesGrantedAsync(Guid? tenantId, string webhookName);

        /// <summary>
        /// Returns all subscriptions of tenant
        /// </summary>
        /// <returns></returns>
        Task<List<WebhookSubscriptionInfo>> GetAllSubscriptionsOfTenantsAsync(Guid?[] tenantIds);

        /// <summary>
        /// Returns all subscriptions for given webhook.
        /// </summary>
        /// <param name="webhookName"><see cref="WebhookDefinition.Name"/></param>
        /// <param name="tenantIds">
        /// Target tenant id(s).
        /// </param>
        Task<List<WebhookSubscriptionInfo>> GetAllSubscriptionsOfTenantsIfFeaturesGrantedAsync(Guid?[] tenantIds, string webhookName);

        /// <summary>
        /// Checks if tenant subscribed for a webhook. (Checks if webhook features are granted)
        /// </summary>
        /// <param name="tenantId">
        /// Target tenant id(s).
        /// </param>
        /// <param name="webhookName"><see cref="WebhookDefinition.Name"/></param>
        Task<bool> IsSubscribedAsync(Guid? tenantId, string webhookName);

        /// <summary>
        /// If id is the default(Guid) adds new subscription, else updates current one. (Checks if webhook features are granted)
        /// </summary>
        Task AddOrUpdateSubscriptionAsync(WebhookSubscriptionInfo webhookSubscription);

        /// <summary>
        /// Activates/Deactivates given webhook subscription
        /// </summary>
        /// <param name="id">unique identifier of <see cref="WebhookSubscriptionInfo"/></param>
        /// <param name="active">IsActive</param>
        Task ActivateWebhookSubscriptionAsync(Guid id, bool active);

        /// <summary>
        /// Delete given webhook subscription.
        /// </summary>
        /// <param name="id">unique identifier of <see cref="WebhookSubscriptionInfo"/></param>
        Task DeleteSubscriptionAsync(Guid id);
    }
}
