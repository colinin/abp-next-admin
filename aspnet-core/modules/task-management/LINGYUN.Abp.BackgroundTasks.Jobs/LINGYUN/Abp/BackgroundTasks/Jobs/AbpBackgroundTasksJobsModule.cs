using LINGYUN.Abp.Dapr.Client;
using LINGYUN.Abp.Dapr.Client.DynamicProxying;
using Microsoft.Extensions.DependencyInjection;
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
    protected const string DontWrapResultField = "_AbpDontWrapResult";

    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<AbpHttpClientBuilderOptions>(options =>
        {
            options.ProxyClientBuildActions.Add((remoteService, builder) =>
            {
                builder.ConfigureHttpClient(client =>
                {
                    // 后台作业一般都是内部调用, 不需要包装结果
                    client.DefaultRequestHeaders.TryAddWithoutValidation(DontWrapResultField, "true");
                });
            });
        });
    }

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

        Configure<AbpDaprClientProxyOptions>(options =>
        {
            options.ProxyRequestActions.Add((remoteService, request) =>
            {
                // 后台作业一般都是内部调用, 不需要包装结果
                request.Headers.TryAddWithoutValidation(DontWrapResultField, "true");
            });
        });

        context.Services.AddHttpClient(BackgroundTasksConsts.DefaultHttpClient);
    }
}
