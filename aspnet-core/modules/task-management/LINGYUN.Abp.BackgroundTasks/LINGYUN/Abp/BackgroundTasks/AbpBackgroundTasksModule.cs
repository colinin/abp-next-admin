using LINGYUN.Abp.BackgroundTasks.Internal;
using LINGYUN.Abp.BackgroundTasks.Localization;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Auditing;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.DistributedLocking;
using Volo.Abp.Guids;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace LINGYUN.Abp.BackgroundTasks;

[DependsOn(typeof(AbpAuditingModule))]
[DependsOn(typeof(AbpLocalizationModule))]
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

        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<AbpBackgroundTasksModule>();
        });

        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Add<BackgroundTasksResource>("en")
                .AddVirtualJson("/LINGYUN/Abp/BackgroundTasks/Localization/Resources");
        });
    }
}
