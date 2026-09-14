using Volo.Abp.Modularity;

namespace LINGYUN.Abp.ElsaNext.EntityFrameworkCore.PostgreSql;

[DependsOn(typeof(AbpElsaNextEntityFrameworkCoreModule))]
public partial class AbpElsaNextEntityFrameworkCorePostgreSqlModule : AbpModule
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
