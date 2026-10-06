using LINGYUN.Abp.MicroService.TaskService.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.MicroService.TaskService.DbMigrator;

[DependsOn(
    typeof(TaskServiceMigrationsEntityFrameworkCoreModule),
    typeof(AbpAutofacModule)
    )]
public partial class TaskServiceDbMigratorModule : AbpModule
{
}
