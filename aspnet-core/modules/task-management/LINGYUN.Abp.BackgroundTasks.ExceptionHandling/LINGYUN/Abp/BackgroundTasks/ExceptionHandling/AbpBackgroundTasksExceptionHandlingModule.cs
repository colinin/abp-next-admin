using LINGYUN.Abp.BackgroundTasks.Jobs;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.BackgroundTasks.ExceptionHandling;

[DependsOn(typeof(AbpBackgroundTasksModule))]
[DependsOn(typeof(AbpBackgroundTasksJobsModule))]
public class AbpBackgroundTasksExceptionHandlingModule : AbpModule
{

}
