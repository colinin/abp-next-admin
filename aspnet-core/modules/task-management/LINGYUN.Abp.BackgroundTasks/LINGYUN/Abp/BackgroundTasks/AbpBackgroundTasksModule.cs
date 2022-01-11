using LINGYUN.Abp.BackgroundTasks.Internal;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Auditing;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.DistributedLocking;
using Volo.Abp.Guids;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.BackgroundTasks;

[DependsOn(typeof(AbpAuditingModule))]
[DependsOn(typeof(AbpBackgroundTasksAbstractionsModule))]
[DependsOn(typeof(AbpBackgroundJobsAbstractionsModule))]
[DependsOn(typeof(AbpDistributedLockingAbstractionsModule))]
[DependsOn(typeof(AbpGuidsModule))]
public class AbpBackgroundTasksModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddTransient(typeof(BackgroundJobAdapter<>));
        context.Services.AddHostedService<DefaultBackgroundWorker>();
    }
}
