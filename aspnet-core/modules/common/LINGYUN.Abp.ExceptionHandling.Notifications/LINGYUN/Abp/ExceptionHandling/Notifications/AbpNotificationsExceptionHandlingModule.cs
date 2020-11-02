using LINGYUN.Abp.ExceptionHandling.Localization;
using LINGYUN.Abp.Notifications;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace LINGYUN.Abp.ExceptionHandling.Notifications
{
    [DependsOn(
        typeof(AbpExceptionHandlingModule),
        typeof(AbpNotificationModule))]
    public class AbpNotificationsExceptionHandlingModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<AbpNotificationsExceptionHandlingModule>();
            });

            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Get<ExceptionHandlingResource>()
                    .AddVirtualJson("/LINGYUN/Abp/ExceptionHandling/Notifications/Localization/Resources");
            });
        }
    }
}
