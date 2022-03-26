using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.Features;
using Volo.Abp.Guids;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.Webhooks;

//[DependsOn(typeof(AbpBackgroundJobsAbstractionsModule))]
// 防止未引用实现无法发布到后台作业
[DependsOn(typeof(AbpBackgroundJobsModule))]
[DependsOn(typeof(AbpFeaturesModule))]
[DependsOn(typeof(AbpGuidsModule))]
[DependsOn(typeof(AbpHttpClientModule))]
public class AbpWebhooksModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        AutoAddDefinitionProviders(context.Services);
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
