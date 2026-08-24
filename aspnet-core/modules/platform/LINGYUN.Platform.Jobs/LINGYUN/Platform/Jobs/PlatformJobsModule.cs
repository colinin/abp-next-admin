using LINGYUN.Platform.Localization;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace LINGYUN.Platform.Jobs;

[DependsOn(
    typeof(AbpBackgroundJobsAbstractionsModule),
    typeof(PlatformDomainModule))]
public class PlatformJobsModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<PlatformJobsModule>();
        });

        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Get<PlatformResource>()
                .AddVirtualJson("/LINGYUN/Platform/Jobs/Localization/Resources");
        });
    }
}
