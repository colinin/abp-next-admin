using DotNetCore.CAP.Processor;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp;
using Volo.Abp.BackgroundWorkers;
using Volo.Abp.EventBus;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.EventBus.CAP
{
    /// <summary>
    /// AbpCAPEventBusModule
    /// </summary>
    [DependsOn(
        typeof(AbpEventBusModule),
        typeof(AbpBackgroundWorkersModule))]
    public class AbpCAPEventBusModule : AbpModule
    {
        /// <summary>
        /// ConfigureServices
        /// </summary>
        /// <param name="context"></param>
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var configuration = context.Services.GetConfiguration();

            Configure<AbpCAPEventBusOptions>(configuration.GetSection("CAP:Abp"));

            context.Services.AddTransient<IFailedThresholdCallbackNotifier, FailedThresholdCallbackNotifier>();

            context.Services.AddCAPEventBus(options =>
            {
                configuration.GetSection("CAP:EventBus").Bind(options);
                context.Services.ExecutePreConfiguredActions(options);
                if (options.FailedThresholdCallback == null)
                {
                    options.FailedThresholdCallback = async (failed) =>
                    {
                        var exceptionNotifier = failed.ServiceProvider.GetService<IFailedThresholdCallbackNotifier>();
                        if (exceptionNotifier != null)
                        {
                            // TODO: 作为异常处理?
                            await exceptionNotifier.NotifyAsync(new AbpCAPExecutionFailedException(failed.MessageType, failed.Message));
                        }
                    };
                }
            });
        }

        /// <summary>
        /// OnApplicationInitialization
        /// </summary>
        /// <param name="context"></param>
        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            context.ServiceProvider
                    .GetRequiredService<IBackgroundWorkerManager>()
                    .Add(
                        context.ServiceProvider
                            .GetRequiredService<AbpCapExpiresMessageCleanupBackgroundWorker>()
                    );
        }
    }
}
