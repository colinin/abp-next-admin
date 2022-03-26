using System;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace LINGYUN.Abp.WebhooksManagement.EntityFrameworkCore;

public class EfCoreWebhookSubscriptionRepository :
    EfCoreRepository<IWebhooksManagementDbContext, WebhookSubscription, Guid>,
    IWebhookSubscriptionRepository
{
    public EfCoreWebhookSubscriptionRepository(
        IDbContextProvider<IWebhooksManagementDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
