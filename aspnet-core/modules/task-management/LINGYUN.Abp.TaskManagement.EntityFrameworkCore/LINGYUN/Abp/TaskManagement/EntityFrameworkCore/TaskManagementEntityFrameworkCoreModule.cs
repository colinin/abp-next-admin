using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.TaskManagement.EntityFrameworkCore;

[DependsOn(typeof(TaskManagementDomainModule))]
[DependsOn(typeof(AbpEntityFrameworkCoreModule))]
public class TaskManagementEntityFrameworkCoreModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAbpDbContext<TaskManagementDbContext>(options =>
        {
            options.AddRepository<BackgroundJobInfo, EfCoreBackgroundJobInfoRepository>();
            options.AddRepository<BackgroundJobLog, EfCoreBackgroundJobLogRepository>();
        });
    }
}
