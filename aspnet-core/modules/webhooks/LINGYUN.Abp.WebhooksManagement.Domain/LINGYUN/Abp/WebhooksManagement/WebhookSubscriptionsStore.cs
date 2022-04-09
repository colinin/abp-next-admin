using LINGYUN.Abp.Webhooks;
using LINGYUN.Abp.WebhooksManagement.Extensions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Domain.Services;
using Volo.Abp.Uow;

namespace LINGYUN.Abp.WebhooksManagement;

public class WebhookSubscriptionsStore : DomainService, IWebhookSubscriptionsStore
{
    protected IWebhookSubscriptionRepository SubscriptionRepository { get; }

    public WebhookSubscriptionsStore(
        IWebhookSubscriptionRepository subscriptionRepository)
    {
        SubscriptionRepository = subscriptionRepository;
    }

    [UnitOfWork]
    public async virtual Task DeleteAsync(Guid id)
    {
        using (CurrentTenant.Change(null))
        {
            await SubscriptionRepository.DeleteAsync(id);
        }
    }

    [UnitOfWork]
    public async virtual Task<List<WebhookSubscriptionInfo>> GetAllSubscriptionsAsync(Guid? tenantId)
    {
        using (CurrentTenant.Change(null))
        {
            var queryable = await SubscriptionRepository.GetQueryableAsync();

            queryable = queryable.Where(x => x.TenantId == tenantId);

            var subscriptions = await AsyncExecuter.ToListAsync(queryable);

            return subscriptions.Select(subscription => subscription.ToWebhookSubscriptionInfo()).ToList();
        }
    }

    [UnitOfWork]
    public async virtual Task<List<WebhookSubscriptionInfo>> GetAllSubscriptionsAsync(Guid? tenantId, string webhookName)
    {
        using (CurrentTenant.Change(null))
        {
            var queryable = await SubscriptionRepository.GetQueryableAsync();

            queryable = queryable.Where(x =>
                    x.TenantId == tenantId &&
                    x.IsActive &&
                    x.Webhooks.Contains("\"" + webhookName + "\""));

            var subscriptions = await AsyncExecuter.ToListAsync(queryable);

            return subscriptions.Select(subscription => subscription.ToWebhookSubscriptionInfo()).ToList();
        }
    }

    [UnitOfWork]
    public async virtual Task<List<WebhookSubscriptionInfo>> GetAllSubscriptionsOfTenantsAsync(Guid?[] tenantIds)
    {
        using (CurrentTenant.Change(null))
        {
            var queryable = await SubscriptionRepository.GetQueryableAsync();

            var subscriptions = await AsyncExecuter.ToListAsync(queryable.Where(x => tenantIds.Contains(x.TenantId)));

            return subscriptions.Select(subscription => subscription.ToWebhookSubscriptionInfo()).ToList();
        }
    }

    [UnitOfWork]
    public async virtual Task<List<WebhookSubscriptionInfo>> GetAllSubscriptionsOfTenantsAsync(Guid?[] tenantIds, string webhookName)
    {
        using (CurrentTenant.Change(null))
        {
            var queryable = await SubscriptionRepository.GetQueryableAsync();

            queryable = queryable.Where(x =>
                    x.IsActive &&
                    tenantIds.Contains(x.TenantId) &&
                    x.Webhooks.Contains("\"" + webhookName + "\""));

            var subscriptions = await AsyncExecuter.ToListAsync(queryable);

            return subscriptions.Select(subscription => subscription.ToWebhookSubscriptionInfo()).ToList();
        }
    }

    [UnitOfWork]
    public async virtual Task<WebhookSubscriptionInfo> GetAsync(Guid id)
    {
        using (CurrentTenant.Change(null))
        {
            var subscription = await SubscriptionRepository.GetAsync(id);

            return subscription.ToWebhookSubscriptionInfo();
        }
    }

    [UnitOfWork]
    public async virtual Task InsertAsync(WebhookSubscriptionInfo webhookSubscription)
    {
        using (CurrentTenant.Change(null))
        {
            var subscription = new WebhookSubscription(
                  webhookSubscription.Id,
                  webhookSubscription.WebhookUri,
                  JsonConvert.SerializeObject(webhookSubscription.Webhooks),
                  JsonConvert.SerializeObject(webhookSubscription.Headers),
                  webhookSubscription.Secret,
                  webhookSubscription.TenantId);

            await SubscriptionRepository.InsertAsync(subscription);
        }
    }

    [UnitOfWork]
    public async virtual Task<bool> IsSubscribedAsync(Guid? tenantId, string webhookName)
    {
        using (CurrentTenant.Change(null))
        {
            var queryable = await SubscriptionRepository.GetQueryableAsync();

            queryable = queryable.Where(x =>
                    x.TenantId == tenantId &&
                    x.IsActive &&
                    x.Webhooks.Contains("\"" + webhookName + "\""));

            return await AsyncExecuter.AnyAsync(queryable);
        }
    }

    [UnitOfWork]
    public async virtual Task UpdateAsync(WebhookSubscriptionInfo webhookSubscription)
    {
        using (CurrentTenant.Change(webhookSubscription.TenantId))
        {
            var subscription = await SubscriptionRepository.GetAsync(webhookSubscription.Id);
            subscription.SetWebhookUri(webhookSubscription.WebhookUri);
            subscription.SetWebhooks(webhookSubscription.ToSubscribedWebhooksString());
            subscription.SetHeaders(webhookSubscription.ToWebhookHeadersString());

            await SubscriptionRepository.UpdateAsync(subscription);
        }
    }
}
