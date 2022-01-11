using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using Volo.Abp.Modularity;
using Volo.Abp.Reflection;

namespace LINGYUN.Abp.BackgroundTasks;

public class AbpBackgroundTasksAbstractionsModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        AutoAddJobMonitors(context.Services);
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
