using LINGYUN.Abp.EntityFrameworkCore.MySQL;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.ElsaNext.EntityFrameworkCore.MySQL;

[DependsOn(
    typeof(AbpElsaNextEntityFrameworkCoreModule),
    typeof(AbpEntityFrameworkCoreMySQLMicrotingModule))]
public partial class AbpElsaNextEntityFrameworkCoreMySQLModule : AbpModule
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
        PreConfigureQuartz(context);
    }
}
