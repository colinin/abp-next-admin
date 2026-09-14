using Volo.Abp.Modularity;

namespace LINGYUN.Abp.ElsaNext.EntityFrameworkCore;

[DependsOn(typeof(AbpElsaNextModule))]
public partial class AbpElsaNextEntityFrameworkCoreModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigureWorkflowManagement(context);
        PreConfigureWorkflowRuntime(context);
        PreConfigureAlterations(context);
        PreConfigureAgent(context);
        PreConfigureLabels(context);
        PreConfigureSecrets(context);
        PreConfigureWeaver(context);
    }
}
