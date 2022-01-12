using LINGYUN.Abp.BackgroundTasks.Jobs;
using LINGYUN.Abp.BackgroundTasks.Localization;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace LINGYUN.Abp.BackgroundTasks.ExceptionHandling;

[DependsOn(typeof(AbpBackgroundTasksModule))]
[DependsOn(typeof(AbpBackgroundTasksJobsModule))]
public class AbpBackgroundTasksExceptionHandlingModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<AbpBackgroundTasksExceptionHandlingModule>();
        });

        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Get<BackgroundTasksResource>()
                .AddVirtualJson("/LINGYUN/Abp/BackgroundTasks/ExceptionHandling/Localization/Resources");
        });
    }
}
