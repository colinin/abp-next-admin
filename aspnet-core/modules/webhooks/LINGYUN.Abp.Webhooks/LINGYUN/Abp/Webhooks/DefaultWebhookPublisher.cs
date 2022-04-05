using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.Guids;
using Volo.Abp.MultiTenancy;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.Webhooks
{
    public class DefaultWebhookPublisher : IWebhookPublisher, ITransientDependency
    {
        public IWebhookEventStore WebhookEventStore { get; set; }

        private readonly ICurrentTenant _currentTenant;
        private readonly IBackgroundJobManager _backgroundJobManager;
        private readonly IWebhookSubscriptionManager _webhookSubscriptionManager;

        public DefaultWebhookPublisher(
            IWebhookSubscriptionManager webhookSubscriptionManager,
            ICurrentTenant currentTenant,
            IBackgroundJobManager backgroundJobManager)
        {
            _currentTenant = currentTenant;
            _backgroundJobManager = backgroundJobManager;
            _webhookSubscriptionManager = webhookSubscriptionManager;

            WebhookEventStore = NullWebhookEventStore.Instance;
        }
        
        #region Async Publish Methods

        public virtual async Task PublishAsync(
            string webhookName,
            object data, 
            bool sendExactSameData = false, 
            WebhookHeader headers = null)
        {
            var subscriptions = await _webhookSubscriptionManager.GetAllSubscriptionsIfFeaturesGrantedAsync(_currentTenant.Id, webhookName);
            await PublishAsync(webhookName, data, subscriptions, sendExactSameData, headers);
        }

        public virtual async Task PublishAsync(
            string webhookName,
            object data,
            Guid? tenantId,
            bool sendExactSameData = false, 
            WebhookHeader headers = null)
        {
            var subscriptions = await _webhookSubscriptionManager.GetAllSubscriptionsIfFeaturesGrantedAsync(tenantId, webhookName);
            await PublishAsync(webhookName, data, subscriptions, sendExactSameData, headers);
        }

        public virtual async Task PublishAsync(
            Guid?[] tenantIds, 
            string webhookName,
            object data,
            bool sendExactSameData = false, 
            WebhookHeader headers = null)
        {
            var subscriptions = await _webhookSubscriptionManager.GetAllSubscriptionsOfTenantsIfFeaturesGrantedAsync(tenantIds, webhookName);
            await PublishAsync(webhookName, data, subscriptions, sendExactSameData, headers);
        }

        protected virtual async Task PublishAsync(
            string webhookName, 
            object data, 
            List<WebhookSubscriptionInfo> webhookSubscriptions,
            bool sendExactSameData = false, 
            WebhookHeader headers = null)
        {
            if (webhookSubscriptions.IsNullOrEmpty())
            {
                return;
            }

            var subscriptionsGroupedByTenant = webhookSubscriptions.GroupBy(x => x.TenantId);

            foreach (var subscriptionGroupedByTenant in subscriptionsGroupedByTenant)
            {
                var webhookInfo = await SaveAndGetWebhookAsync(subscriptionGroupedByTenant.Key, webhookName, data);

                foreach (var webhookSubscription in subscriptionGroupedByTenant)
                {
                    var headersToSend = webhookSubscription.Headers;
                    if (headers != null)
                    {
                        if (headers.UseOnlyGivenHeaders)//do not use the headers defined in subscription
                        {
                            headersToSend = headers.Headers;
                        }
                        else
                        {
                            //use the headers defined in subscription. If additional headers has same header, use additional headers value.
                            foreach (var additionalHeader in headers.Headers)
                            {
                                headersToSend[additionalHeader.Key] = additionalHeader.Value;
                            }
                        }
                    }

                    await _backgroundJobManager.EnqueueAsync(new WebhookSenderArgs
                    {
                        TenantId = webhookSubscription.TenantId,
                        WebhookEventId = webhookInfo.Id,
                        Data = webhookInfo.Data,
                        WebhookName = webhookInfo.WebhookName,
                        WebhookSubscriptionId = webhookSubscription.Id,
                        Headers = headersToSend,
                        Secret = webhookSubscription.Secret,
                        WebhookUri = webhookSubscription.WebhookUri,
                        SendExactSameData = sendExactSameData
                    });
                }
            }
        }
        
        #endregion

        protected virtual async Task<WebhookEvent> SaveAndGetWebhookAsync(
            Guid? tenantId,
            string webhookName,
            object data)
        {
            var webhookInfo = new WebhookEvent
            {
                WebhookName = webhookName,
                Data = JsonConvert.SerializeObject(data),
                TenantId = tenantId
            };

            var webhookId = await WebhookEventStore.InsertAndGetIdAsync(webhookInfo);
            webhookInfo.Id = webhookId;

            return webhookInfo;
        }
    }
}