using Hangfire;
using Volo.Abp.Hangfire;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.BackgroundTasks.Hangfire;

[DependsOn(typeof(AbpBackgroundTasksModule))]
[DependsOn(typeof(AbpHangfireModule))]
public class AbpBackgroundTasksHangfireModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddHangfire((provider, configuration) =>
        {
            configuration.UseFilter(new HangfireJobExecutedAttribute(provider));
        });
    }
}
