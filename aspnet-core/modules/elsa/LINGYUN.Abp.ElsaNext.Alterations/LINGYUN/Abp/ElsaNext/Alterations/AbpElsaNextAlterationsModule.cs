using Elsa.Alterations.Extensions;
using Elsa.Features.Contracts;
using Elsa.Features.Services;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.ElsaNext.Alterations;

[DependsOn(
    typeof(AbpElsaNextModule))]
public class AbpElsaNextAlterationsModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<IModule>(elsa =>
        {
            elsa.UseAlterations();
        });
    }

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        // TODO: 强制启用Studio中的Alterations?
        var registry = context.ServiceProvider.GetRequiredService<IInstalledFeatureRegistry>();
        registry.Add(new("Alterations.ShellFeatures.Alterations", "Elsa", "Alterations.ShellFeatures.Alterations"));
    }
}
