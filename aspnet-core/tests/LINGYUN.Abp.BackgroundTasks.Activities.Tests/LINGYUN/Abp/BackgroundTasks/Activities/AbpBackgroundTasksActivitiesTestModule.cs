using Volo.Abp.Modularity;

namespace LINGYUN.Abp.BackgroundTasks.Activities;

[DependsOn(
    typeof(AbpBackgroundTasksActivitiesModule),
    typeof(AbpBackgroundTasksTestModule))]
public class AbpBackgroundTasksActivitiesTestModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpBackgroundTasksOptions>(options =>
        {
            options.Map("TEST:001", JobExceptionType.Business);
        });
    }
}
