using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.WebhooksManagement.EntityFrameworkCore;

[DependsOn(
    typeof(WebhooksManagementDomainModule),
    typeof(AbpEntityFrameworkCoreModule))]
public class WebhooksManagementEntityFrameworkCoreModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAbpDbContext<WebhooksManagementDbContext>(options =>
        {
            options.AddRepository<WebhookSendRecord, EfCoreWebhookSendRecordRepository>();
            options.AddRepository<WebhookEventRecord, EfCoreWebhookEventRecordRepository>();
            options.AddRepository<WebhookSubscription, EfCoreWebhookSubscriptionRepository>();

            options.AddDefaultRepositories<IWebhooksManagementDbContext>();
        });
    }
}
