using Volo.Abp.Modularity;

namespace LINGYUN.Abp.ElsaNext.EntityFrameworkCore.Sqlite;

[DependsOn(typeof(AbpElsaNextEntityFrameworkCoreModule))]
public partial class AbpElsaNextEntityFrameworkCoreSqliteModule : AbpModule
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
