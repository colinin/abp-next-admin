using LINGYUN.Abp.BackgroundTasks.Internal;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using Volo.Abp.Auditing;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.DistributedLocking;
using Volo.Abp.Guids;
using Volo.Abp.Modularity;
using Volo.Abp.Reflection;

namespace LINGYUN.Abp.BackgroundTasks;

[DependsOn(typeof(AbpAuditingModule))]
[DependsOn(typeof(AbpBackgroundTasksAbstractionsModule))]
[DependsOn(typeof(AbpBackgroundJobsAbstractionsModule))]
[DependsOn(typeof(AbpDistributedLockingAbstractionsModule))]
[DependsOn(typeof(AbpGuidsModule))]
public class AbpBackgroundTasksModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        AutoAddJobMonitors(context.Services);
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddTransient(typeof(BackgroundJobAdapter<>));
        context.Services.AddHostedService<DefaultBackgroundWorker>();
    }

    private static void AutoAddJobMonitors(IServiceCollection services)
    {
        var jobMonitors = new List<Type>();

        services.OnRegistred(context =>
        {
            if (ReflectionHelper.IsAssignableToGenericType(context.ImplementationType, typeof(JobEventBase<>)))
            {
                jobMonitors.Add(context.ImplementationType);
            }
        });

        services.Configure<AbpBackgroundTasksOptions>(options =>
        {
            options.JobMonitors.AddIfNotContains(jobMonitors);
        });
    }
}
