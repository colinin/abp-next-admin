using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Authorization;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;
using Volo.Abp.Uow;

namespace LINGYUN.Abp.Webhooks
{
    public class WebhookSubscriptionManager : IWebhookSubscriptionManager, ITransientDependency
    {
        public IWebhookSubscriptionsStore WebhookSubscriptionsStore { get; set; }

        private readonly IGuidGenerator _guidGenerator;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly IWebhookDefinitionManager _webhookDefinitionManager;

        private const string WebhookSubscriptionSecretPrefix = "whs_";

        public WebhookSubscriptionManager(
            IGuidGenerator guidGenerator,
            IUnitOfWorkManager unitOfWorkManager,
            IWebhookDefinitionManager webhookDefinitionManager)
        {
            _guidGenerator = guidGenerator;
            _unitOfWorkManager = unitOfWorkManager;
            _webhookDefinitionManager = webhookDefinitionManager;

            WebhookSubscriptionsStore = NullWebhookSubscriptionsStore.Instance;
        }

        public virtual async Task<WebhookSubscriptionInfo> GetAsync(Guid id)
        {
            return await WebhookSubscriptionsStore.GetAsync(id);
        }

        public virtual async Task<List<WebhookSubscriptionInfo>> GetAllSubscriptionsAsync(Guid? tenantId)
        {
            return await WebhookSubscriptionsStore.GetAllSubscriptionsAsync(tenantId);
        }

        public virtual async Task<List<WebhookSubscriptionInfo>> GetAllSubscriptionsIfFeaturesGrantedAsync(Guid? tenantId, string webhookName)
        {
            if (!await _webhookDefinitionManager.IsAvailableAsync(tenantId, webhookName))
            {
                return new List<WebhookSubscriptionInfo>();
            }

            return (await WebhookSubscriptionsStore.GetAllSubscriptionsAsync(tenantId, webhookName)).ToList();
        }

        public virtual async Task<List<WebhookSubscriptionInfo>> GetAllSubscriptionsOfTenantsAsync(Guid?[] tenantIds)
        {
            return (await WebhookSubscriptionsStore.GetAllSubscriptionsOfTenantsAsync(tenantIds)).ToList();
        }

        public virtual async Task<List<WebhookSubscriptionInfo>> GetAllSubscriptionsOfTenantsIfFeaturesGrantedAsync(Guid?[] tenantIds, string webhookName)
        {
            var featureGrantedTenants = new List<Guid?>();
            foreach (var tenantId in tenantIds)
            {
                if (await _webhookDefinitionManager.IsAvailableAsync(tenantId, webhookName))
                {
                    featureGrantedTenants.Add(tenantId);
                }
            }

            return (await WebhookSubscriptionsStore.GetAllSubscriptionsOfTenantsAsync(featureGrantedTenants.ToArray(), webhookName)).ToList();
        }

        public virtual async Task<bool> IsSubscribedAsync(Guid? tenantId, string webhookName)
        {
            if (!await _webhookDefinitionManager.IsAvailableAsync(tenantId, webhookName))
            {
                return false;
            }

            return await WebhookSubscriptionsStore.IsSubscribedAsync(tenantId, webhookName);
        }
        
        public virtual async Task AddOrUpdateSubscriptionAsync(WebhookSubscriptionInfo webhookSubscription)
        {
            using (var uow = _unitOfWorkManager.Begin())
            {
                await CheckIfPermissionsGrantedAsync(webhookSubscription);

                if (webhookSubscription.Id == default)
                {
                    webhookSubscription.Id = _guidGenerator.Create();
                    webhookSubscription.Secret = WebhookSubscriptionSecretPrefix + Guid.NewGuid().ToString("N");
                    await WebhookSubscriptionsStore.InsertAsync(webhookSubscription);
                }
                else
                {
                    await WebhookSubscriptionsStore.UpdateAsync(webhookSubscription);
                }

                await uow.SaveChangesAsync();
            }
        }

        public virtual async Task ActivateWebhookSubscriptionAsync(Guid id, bool active)
        {
            using (var uow = _unitOfWorkManager.Begin())
            {
                var webhookSubscription = await WebhookSubscriptionsStore.GetAsync(id);
                webhookSubscription.IsActive = active;

                await uow.SaveChangesAsync();
            }
        }
        
        public virtual async Task DeleteSubscriptionAsync(Guid id)
        {
            using (var uow = _unitOfWorkManager.Begin())
            {
                await WebhookSubscriptionsStore.DeleteAsync(id);

                await uow.SaveChangesAsync();
            }
        }
        
        public virtual async Task AddWebhookAsync(WebhookSubscriptionInfo subscription, string webhookName)
        {
            using (var uow = _unitOfWorkManager.Begin())
            {
                await CheckPermissionsAsync(subscription.TenantId, webhookName);
                webhookName = webhookName.Trim();
                if (webhookName.IsNullOrWhiteSpace())
                {
                    throw new ArgumentNullException(nameof(webhookName), $"{nameof(webhookName)} can not be null, empty or whitespace!");
                }

                if (!subscription.Webhooks.Contains(webhookName))
                {
                    subscription.Webhooks.Add(webhookName);

                    await WebhookSubscriptionsStore.UpdateAsync(subscription);
                }

                await uow.SaveChangesAsync();
            }
        }

        #region PermissionCheck

        protected virtual async Task CheckIfPermissionsGrantedAsync(WebhookSubscriptionInfo webhookSubscription)
        {
            if (webhookSubscription.Webhooks.IsNullOrEmpty())
            {
                return;
            }

            foreach (var webhookDefinition in webhookSubscription.Webhooks)
            {
                await CheckPermissionsAsync(webhookSubscription.TenantId, webhookDefinition);
            }
        }

        protected virtual async Task CheckPermissionsAsync(Guid? tenantId, string webhookName)
        {
            if (!await _webhookDefinitionManager.IsAvailableAsync(tenantId, webhookName))
            {
                throw new AbpAuthorizationException($"Tenant \"{tenantId}\" must have necessary feature(s) to use webhook \"{webhookName}\"");
            }
        }

        #endregion
    }
}
