using LINGYUN.Abp.BackgroundTasks;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AutoMapper;
using Volo.Abp.Domain;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.TaskManagement;

[DependsOn(typeof(TaskManagementDomainSharedModule))]
[DependsOn(typeof(AbpAutoMapperModule))]
[DependsOn(typeof(AbpDddDomainModule))]
[DependsOn(typeof(AbpBackgroundTasksModule))]
public class TaskManagementDomainModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAutoMapperObjectMapper<TaskManagementDomainModule>();

        Configure<AbpAutoMapperOptions>(options =>
        {
            options.AddProfile<TaskManagementDomainMapperProfile>(validate: true);
        });
    }
}