using LINGYUN.Abp.BackgroundTasks.Internal;
using LINGYUN.Abp.BackgroundTasks.Localization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Collections.Generic;
using Volo.Abp.Auditing;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.BackgroundWorkers;
using Volo.Abp.Data;
using Volo.Abp.Guids;
using Volo.Abp.Json;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace LINGYUN.Abp.BackgroundTasks;

[DependsOn(typeof(AbpAuditingModule))]
[DependsOn(typeof(AbpBackgroundTasksAbstractionsModule))]
[DependsOn(typeof(AbpBackgroundJobsAbstractionsModule))]
[DependsOn(typeof(AbpBackgroundWorkersModule))]
[DependsOn(typeof(AbpGuidsModule))]
[DependsOn(typeof(AbpJsonModule))]
public class AbpBackgroundTasksModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var configuration = context.Services.GetConfiguration();

        context.Services.AddTransient(typeof(BackgroundJobAdapter<>));
        context.Services.AddSingleton(typeof(BackgroundWorkerAdapter<>));

        if (!context.Services.IsDataMigrationEnvironment())
        {
            context.Services.Replace(ServiceDescriptor.Transient(typeof(IBackgroundJobManager), typeof(BackgroundJobManager)));
            context.Services.Replace(ServiceDescriptor.Transient(typeof(IBackgroundWorkerManager), typeof(BackgroundWorkerManager)));
            context.Services.AddHostedService<DefaultBackgroundWorker>();
        }

        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<AbpBackgroundTasksModule>();
        });

        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Get<BackgroundTasksResource>()
                .AddVirtualJson("/LINGYUN/Abp/BackgroundTasks/Localization/Resources");
        });

        Configure<AbpBackgroundTasksOptions>(options =>
        {
            options.JobMonitors.AddIfNotContains(typeof(JobExecutedEvent));
            options.JobMonitors.AddIfNotContains(typeof(JobLogEvent));
        });
    }
}
