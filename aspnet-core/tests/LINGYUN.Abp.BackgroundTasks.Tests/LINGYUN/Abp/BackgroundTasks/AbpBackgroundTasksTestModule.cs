using LINGYUN.Abp.Tests;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.BackgroundTasks;

[DependsOn(
    typeof(AbpBackgroundTasksModule),
    typeof(AbpTestsBaseModule))]
public class AbpBackgroundTasksTestModule : AbpModule
{
}
