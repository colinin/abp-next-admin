using Volo.Abp.EventBus;
using Volo.Abp.Json.SystemTextJson;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.Webhooks.EventBus;

[DependsOn(
    typeof(AbpWebhooksModule),
    typeof(AbpEventBusModule))]
public class AbpWebhooksEventBusModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpSystemTextJsonSerializerOptions>(options =>
        {
            options.UnsupportedTypes.TryAdd<WebhooksEventData>();
        });
    }
}