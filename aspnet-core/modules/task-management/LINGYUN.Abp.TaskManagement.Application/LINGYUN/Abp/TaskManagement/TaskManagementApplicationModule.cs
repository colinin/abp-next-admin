using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Application;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.TaskManagement;

[DependsOn(typeof(TaskManagementApplicationContractsModule))]
[DependsOn(typeof(TaskManagementDomainModule))]
[DependsOn(typeof(AbpDddApplicationModule))]
public class TaskManagementApplicationModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAutoMapperObjectMapper<TaskManagementApplicationModule>();

        Configure<AbpAutoMapperOptions>(options =>
        {
            options.AddProfile<TaskManagementApplicationMapperProfile>(validate: true);
        });
    }
}
