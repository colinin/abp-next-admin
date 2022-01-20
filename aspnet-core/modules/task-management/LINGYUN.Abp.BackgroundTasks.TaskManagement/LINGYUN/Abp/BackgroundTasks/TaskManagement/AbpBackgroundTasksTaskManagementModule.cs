using LINGYUN.Abp.TaskManagement.HttpApi.Client;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.BackgroundTasks.TaskManagement;

[DependsOn(typeof(AbpBackgroundTasksModule))]
[DependsOn(typeof(TaskManagementHttpApiClientModule))]
public class AbpBackgroundTasksTaskManagementModule : AbpModule
{

}
