using LINGYUN.Abp.IdGenerator;
using LINGYUN.Abp.Notifications.Internal;
using LINGYUN.Abp.RealTime;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.BackgroundWorkers;
using Volo.Abp.Json;
using Volo.Abp.Modularity;
using Volo.Abp.Threading;

namespace LINGYUN.Abp.Notifications
{
    // TODO: 需要重命名 AbpNotificationsModule
    [DependsOn(
        typeof(AbpBackgroundWorkersModule),
        typeof(AbpBackgroundJobsAbstractionsModule),
        typeof(AbpIdGeneratorModule),
        typeof(AbpJsonModule),
        typeof(AbpRealTimeModule))]
    public class AbpNotificationModule : AbpModule
    {

        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            AutoAddDefinitionProviders(context.Services);
        }

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            AsyncHelper.RunSync(() => OnApplicationInitializationAsync(context));
        }

        public override async Task OnApplicationInitializationAsync(ApplicationInitializationContext context)
        {
            var options = context.ServiceProvider.GetRequiredService<IOptions<AbpNotificationCleanupOptions>>().Value;
            if (options.IsEnabled)
            {
                await context.ServiceProvider
                    .GetRequiredService<IBackgroundWorkerManager>()
                    .AddAsync(
                        context.ServiceProvider
                            .GetRequiredService<NotificationCleanupBackgroundWorker>()
                    );
            }
        }

        private void AutoAddDefinitionProviders(IServiceCollection services)
        {
            var definitionProviders = new List<Type>();

            services.OnRegistred(context =>
            {
                if (typeof(INotificationDefinitionProvider).IsAssignableFrom(context.ImplementationType))
                {
                    definitionProviders.Add(context.ImplementationType);
                }
            });

            var preActions = services.GetPreConfigureActions<AbpNotificationOptions>();
            Configure<AbpNotificationOptions>(options =>
            {
                preActions.Configure(options);
                options.DefinitionProviders.AddIfNotContains(definitionProviders);
            });
        }
    }
}
