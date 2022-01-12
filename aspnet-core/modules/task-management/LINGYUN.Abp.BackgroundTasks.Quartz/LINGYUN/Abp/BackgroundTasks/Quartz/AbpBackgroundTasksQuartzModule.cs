using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Volo.Abp;
using Volo.Abp.Modularity;
using Volo.Abp.Quartz;

namespace LINGYUN.Abp.BackgroundTasks.Quartz;

[DependsOn(typeof(AbpBackgroundTasksModule))]
[DependsOn(typeof(AbpQuartzModule))]
public class AbpBackgroundTasksQuartzModule : AbpModule
{
    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        var _scheduler = context.ServiceProvider.GetRequiredService<IScheduler>();

        _scheduler.ListenerManager.AddJobListener(context.ServiceProvider.GetRequiredService<QuartzJobListener>());
        _scheduler.ListenerManager.AddTriggerListener(context.ServiceProvider.GetRequiredService<QuartzTriggerListener>());

    }
}
