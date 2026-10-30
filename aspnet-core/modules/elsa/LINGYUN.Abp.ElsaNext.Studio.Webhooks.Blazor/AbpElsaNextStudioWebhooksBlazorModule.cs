using LINGYUN.Abp.ElsaNext.Studio.Blazor;
using LINGYUN.Abp.ElsaNext.Studio.Webhooks.Blazor.Extensions;
using LINGYUN.Abp.ElsaNext.Webhooks;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.ElsaNext.Studio.Webhooks.Blazor;

[DependsOn(
    typeof(AbpElsaNextWebhooksModule),
    typeof(AbpElsaNextStudioBlazorModule))]
public class AbpElsaNextStudioWebhooksBlazorModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddWebhooksUIHintHandlers();
    }
}
