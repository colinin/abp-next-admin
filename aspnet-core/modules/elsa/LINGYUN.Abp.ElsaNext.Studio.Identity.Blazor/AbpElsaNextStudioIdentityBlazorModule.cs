using LINGYUN.Abp.ElsaNext.Studio.Blazor;
using LINGYUN.Abp.ElsaNext.Studio.Identity.Blazor.Extensions;
using Volo.Abp.Identity;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.ElsaNext.Studio.Identity.Blazor;

[DependsOn(
    typeof(AbpIdentityDomainModule),
    typeof(AbpElsaNextStudioBlazorModule))]
public class AbpElsaNextStudioIdentityBlazorModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddIdentityUIHintHandlers();
    }
}
