using LINGYUN.Abp.Dapr.Client;
using Volo.Abp.Emailing;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;
using Volo.Abp.Sms;

namespace LINGYUN.Abp.BackgroundTasks.Jobs;

[DependsOn(typeof(AbpEmailingModule))]
[DependsOn(typeof(AbpSmsModule))]
[DependsOn(typeof(AbpHttpClientModule))]
[DependsOn(typeof(AbpDaprClientModule))]
public class AbpBackgroundTasksJobsModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpBackgroundTasksOptions>(options =>
        {
            options.AddProvider<ConsoleJob>(DefaultJobNames.ConsoleJob);
            options.AddProvider<SendEmailJob>(DefaultJobNames.SendEmailJob);
            options.AddProvider<SendSmsJob>(DefaultJobNames.SendSmsJob);
            options.AddProvider<SleepJob>(DefaultJobNames.SleepJob);
            options.AddProvider<ServiceInvocationJob>(DefaultJobNames.ServiceInvocationJob);
            options.AddProvider<HttpRequestJob>(DefaultJobNames.HttpRequestJob);
        });
    }
}
