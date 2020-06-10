using LINGYUN.Abp.Notifications.Internal;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.Json;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.Notifications
{
    [DependsOn(
        typeof(AbpBackgroundJobsModule),
        typeof(AbpJsonModule))]
    public class AbpNotificationModule : AbpModule
    {

        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            AutoAddDefinitionProviders(context.Services);
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddTransient<INotificationDispatcher, DefaultNotificationDispatcher>();
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
                options.DefinitionProviders.AddIfNotContains(definitionProviders);
            });
        }
    }
}
