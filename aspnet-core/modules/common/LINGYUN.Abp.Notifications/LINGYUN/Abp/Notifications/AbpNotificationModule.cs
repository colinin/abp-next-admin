using LINGYUN.Abp.IdGenerator;
using LINGYUN.Abp.Notifications.Internal;
using LINGYUN.Abp.RealTime;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using Volo.Abp;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.BackgroundWorkers;
using Volo.Abp.Json;
using Volo.Abp.Modularity;

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

        public override void OnPostApplicationInitialization(ApplicationInitializationContext context)
        {
            var options = context.ServiceProvider.GetRequiredService<IOptions<AbpNotificationCleanupOptions>>().Value;
            if (options.IsEnabled)
            {
                context.ServiceProvider
                    .GetRequiredService<IBackgroundWorkerManager>()
                    .Add(
                        context.ServiceProvider
                            .GetRequiredService<NotificationCleanupBackgroundWorker>()
                    );
            }
        }

        private static void AutoAddDefinitionProviders(IServiceCollection services)
        {
            var definitionProviders = new List<Type>();

            services.OnRegistred(context =>
            {
                if (typeof(INotificationDefinitionProvider).IsAssignableFrom(context.ImplementationType))
                {
                    definitionProviders.Add(context.ImplementationType);
                }
            });

            services.Configure<AbpNotificationOptions>(options =>
            {
                services.ExecutePreConfiguredActions(options);
                options.DefinitionProviders.AddIfNotContains(definitionProviders);
            });
        }
    }
}
