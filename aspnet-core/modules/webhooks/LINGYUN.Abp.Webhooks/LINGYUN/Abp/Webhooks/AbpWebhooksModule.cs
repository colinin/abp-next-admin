using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.Guids;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.Webhooks;

[DependsOn(typeof(AbpWebhooksCoreModule))]
[DependsOn(typeof(AbpBackgroundJobsAbstractionsModule))]
[DependsOn(typeof(AbpGuidsModule))]
[DependsOn(typeof(AbpHttpClientModule))]
public class AbpWebhooksModule : AbpModule
{
    internal const string WebhooksClient = "__Abp_Webhooks_HttpClient";

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var options = context.Services.ExecutePreConfiguredActions<AbpWebhooksOptions>();

        context.Services.AddHttpClient(WebhooksClient, client =>
        {
            client.Timeout = options.TimeoutDuration;
        });
    }
}
