using LINGYUN.Abp.ExceptionHandling;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.BackgroundTasks.ExceptionHandling;

[DependsOn(typeof(AbpExceptionHandlingModule))]
public class AbpBackgroundTasksExceptionHandlingModule : AbpModule
{

}
