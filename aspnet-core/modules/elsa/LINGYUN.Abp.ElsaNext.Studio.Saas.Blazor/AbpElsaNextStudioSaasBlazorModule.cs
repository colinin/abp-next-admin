using LINGYUN.Abp.ElsaNext.Studio.Blazor;
using LINGYUN.Abp.ElsaNext.Studio.Saas.Blazor.Extensions;
using LINGYUN.Abp.Saas;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.ElsaNext.Studio.Saas.Blazor;

[DependsOn(
    typeof(AbpSaasDomainModule),
    typeof(AbpElsaNextStudioBlazorModule))]
public class AbpElsaNextStudioSaasBlazorModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddSaasUIHintHandlers();
    }
}
