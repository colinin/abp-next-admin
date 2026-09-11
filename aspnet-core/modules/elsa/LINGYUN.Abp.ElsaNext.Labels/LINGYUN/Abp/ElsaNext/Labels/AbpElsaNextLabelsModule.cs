using Elsa.Extensions;
using Elsa.Features.Services;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.ElsaNext.Labels;

[DependsOn(typeof(AbpElsaNextModule))]
public class AbpElsaNextLabelsModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<IModule>(elsa =>
        {
            elsa.UseLabels();
        });
    }
}
