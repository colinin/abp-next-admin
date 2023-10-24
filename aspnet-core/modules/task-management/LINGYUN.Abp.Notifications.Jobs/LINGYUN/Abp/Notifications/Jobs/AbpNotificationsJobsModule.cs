using LINGYUN.Abp.BackgroundTasks;
using LINGYUN.Abp.Notifications.Localization;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace LINGYUN.Abp.Notifications.Jobs;

[DependsOn(typeof(AbpNotificationsModule))]
[DependsOn(typeof(AbpBackgroundTasksAbstractionsModule))]
public class AbpNotificationsJobsModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<AbpNotificationsJobsModule>();
        });

        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Get<NotificationsResource>()
                .AddVirtualJson("/LINGYUN/Abp/Notifications/Jobs/Localization/Resources");
        });
    }
}
