using LINGYUN.Abp.IdGenerator;
using LINGYUN.Abp.Notifications.Localization;
using LINGYUN.Abp.RealTime;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.BackgroundWorkers;
using Volo.Abp.EventBus;
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
        typeof(AbpNotificationsCoreModule),
        typeof(AbpBackgroundWorkersModule),
        typeof(AbpBackgroundJobsAbstractionsModule),
        typeof(AbpIdGeneratorModule),
        typeof(AbpJsonModule),
        typeof(AbpLocalizationModule),
        typeof(AbpRealTimeModule),
        typeof(AbpEventBusModule),
        typeof(AbpTextTemplatingCoreModule))]
    public class AbpNotificationsModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<AbpNotificationsModule>();
            });

            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Get<NotificationsResource>()
                    .AddVirtualJson("/LINGYUN/Abp/Notifications/Localization/Resources");
            });

            Configure<AbpSystemTextJsonSerializerOptions>(options =>
            {
                options.UnsupportedTypes.Add<NotificationInfo>();
            });
        }
    }
}
