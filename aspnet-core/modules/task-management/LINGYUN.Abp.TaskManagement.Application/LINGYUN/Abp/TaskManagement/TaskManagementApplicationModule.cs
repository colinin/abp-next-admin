using LINGYUN.Abp.Dynamic.Queryable;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Application;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.TaskManagement;

[DependsOn(typeof(TaskManagementApplicationContractsModule))]
[DependsOn(typeof(AbpDynamicQueryableApplicationModule))]
[DependsOn(typeof(TaskManagementDomainModule))]
[DependsOn(typeof(AbpDddApplicationModule))]
public class TaskManagementApplicationModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddMapperlyObjectMapper<TaskManagementApplicationModule>();
    }
}
