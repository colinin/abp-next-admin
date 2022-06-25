using LINGYUN.Abp.IdGenerator;
using LINGYUN.Abp.Notifications.Localization;
using LINGYUN.Abp.RealTime;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.BackgroundWorkers;
using Volo.Abp.Json;
using Volo.Abp.Json.SystemTextJson;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.TextTemplating;
using Volo.Abp.VirtualFileSystem;

namespace LINGYUN.Abp.Notifications
{
    // TODO: 需要重命名 AbpNotificationsModule
    [DependsOn(
        typeof(AbpBackgroundWorkersModule),
        typeof(AbpBackgroundJobsAbstractionsModule),
        typeof(AbpIdGeneratorModule),
        typeof(AbpJsonModule),
        typeof(AbpLocalizationModule),
        typeof(AbpRealTimeModule),
        typeof(AbpTextTemplatingCoreModule))]
    public class AbpNotificationModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            AutoAddDefinitionProviders(context.Services);
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<AbpNotificationModule>();
            });

            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Add<NotificationsResource>("en")
                    .AddVirtualJson("/LINGYUN/Abp/Notifications/Localization/Resources");
            });

            Configure<AbpSystemTextJsonSerializerOptions>(options =>
            {
                options.UnsupportedTypes.Add<NotificationInfo>();
            });

            var preActions = context.Services.GetPreConfigureActions<AbpNotificationOptions>();
            Configure<AbpNotificationOptions>(options =>
            {
                preActions.Configure(options);
            });
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

            Configure<AbpNotificationOptions>(options =>
            {
                options.DefinitionProviders.AddIfNotContains(definitionProviders);
            });
        }
    }
}
