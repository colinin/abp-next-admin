using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.Features;
using Volo.Abp.Guids;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.Webhooks;

[DependsOn(typeof(AbpBackgroundJobsAbstractionsModule))]
[DependsOn(typeof(AbpFeaturesModule))]
[DependsOn(typeof(AbpGuidsModule))]
[DependsOn(typeof(AbpHttpClientModule))]
public class AbpWebhooksModule : AbpModule
{
    internal const string WebhooksClient = "__Abp_Webhooks_HttpClient";

    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        AutoAddDefinitionProviders(context.Services);
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var options = context.Services.ExecutePreConfiguredActions<AbpWebhooksOptions>();

        context.Services.AddHttpClient(WebhooksClient, client =>
        {
            client.Timeout = options.TimeoutDuration;
        });
    }

    private static void AutoAddDefinitionProviders(IServiceCollection services)
    {
        var definitionProviders = new List<Type>();

        services.OnRegistred(context =>
        {
            if (typeof(WebhookDefinitionProvider).IsAssignableFrom(context.ImplementationType))
            {
                definitionProviders.Add(context.ImplementationType);
            }
        });

        services.Configure<AbpWebhooksOptions>(options =>
        {
            options.DefinitionProviders.AddIfNotContains(definitionProviders);
        });
    }
}
