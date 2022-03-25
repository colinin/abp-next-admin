using LINGYUN.Abp.Webhooks;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Domain.Services;

namespace LINGYUN.Abp.WebhooksManagement;

public class WebhookSubscriptionsStore : DomainService, IWebhookSubscriptionsStore
{
    public Task DeleteAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<List<WebhookSubscriptionInfo>> GetAllSubscriptionsAsync(Guid? tenantId)
    {
        throw new NotImplementedException();
    }

    public Task<List<WebhookSubscriptionInfo>> GetAllSubscriptionsAsync(Guid? tenantId, string webhookName)
    {
        throw new NotImplementedException();
    }

    public Task<List<WebhookSubscriptionInfo>> GetAllSubscriptionsOfTenantsAsync(Guid?[] tenantIds)
    {
        throw new NotImplementedException();
    }

    public Task<List<WebhookSubscriptionInfo>> GetAllSubscriptionsOfTenantsAsync(Guid?[] tenantIds, string webhookName)
    {
        throw new NotImplementedException();
    }

    public Task<WebhookSubscriptionInfo> GetAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task InsertAsync(WebhookSubscriptionInfo webhookSubscription)
    {
        throw new NotImplementedException();
    }

    public Task<bool> IsSubscribedAsync(Guid? tenantId, string webhookName)
    {
        throw new NotImplementedException();
    }

    public Task UpdateAsync(WebhookSubscriptionInfo webhookSubscription)
    {
        throw new NotImplementedException();
    }
}
