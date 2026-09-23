using LINGYUN.Abp.Dynamic.Definitions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using Volo.Abp.Features;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.Webhooks;

[DependsOn(
    typeof(AbpDynamicDefinitionsCoreModule),
    typeof(AbpFeaturesModule))]
public class AbpWebhooksCoreModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        AutoAddDefinitionProviders(context.Services);
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpDynamicDefinitionsOptions>(options =>
        {
            options.MapStrategy<WebhookDefinition>(DynamicDefinitionStrategy.Merge);
            options.MapStrategy<WebhookGroupDefinition>(DynamicDefinitionStrategy.Merge);
        });
    }

    private static void AutoAddDefinitionProviders(IServiceCollection services)
    {
        var definitionProviders = new List<Type>();

        services.OnRegistered(context =>
        {
            if (typeof(IWebhookDefinitionProvider).IsAssignableFrom(context.ImplementationType))
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
