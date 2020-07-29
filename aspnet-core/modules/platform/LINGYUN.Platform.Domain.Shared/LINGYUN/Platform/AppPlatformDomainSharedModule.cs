using LINGYUN.Platform.Localization;
using Volo.Abp.Localization;
using Volo.Abp.Localization.ExceptionHandling;
using Volo.Abp.Modularity;
using Volo.Abp.Validation.Localization;
using Volo.Abp.VirtualFileSystem;

namespace LINGYUN.Platform
{
    [DependsOn(typeof(AbpLocalizationModule))]
    public class AppPlatformDomainSharedModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<AppPlatformDomainSharedModule>();
            });

            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Add<PlatformResource>("en")
                    .AddBaseTypes(typeof(AbpValidationResource))
                    .AddVirtualJson("/LINGYUN/Platform/Localization/Resources");
            });

            Configure<AbpExceptionLocalizationOptions>(options =>
            {
                options.MapCodeNamespace("Platform.VersionFile", typeof(PlatformResource));
            });
        }
    }
}
