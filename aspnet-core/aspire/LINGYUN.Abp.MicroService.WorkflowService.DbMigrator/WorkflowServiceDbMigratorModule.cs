using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.MicroService.WorkflowService;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(WorkflowServiceMigrationsEntityFrameworkCoreModule)
    )]
public class WorkflowServiceDbMigratorModule : AbpModule
{
}
