using Elsa.Options;
using LINGYUN.Abp.Elsa.Tests;
using LINGYUN.Abp.Webhooks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.Elsa.Activities.Webhooks.Tests;

[DependsOn(
    typeof(AbpElsaTestsModule),
    typeof(AbpElsaActivitiesWebhooksModule)
    )]
public class AbpElsaActivitiesWebhooksTestsModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<ElsaOptionsBuilder>(builder =>
        {
            builder
                .AddWebhooksActivities()
                .AddWorkflow<PublishWebhookWorkflow>();
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.Replace(ServiceDescriptor.Singleton<IWebhookPublisher, MemoryWebhookPublisher>());
    }
}
