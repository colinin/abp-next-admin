using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.MicroService.TaskService;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(TaskServiceMigrationsEntityFrameworkCoreModule)
    )]
public class TaskServiceDbMigratorModule : AbpModule
{
}
