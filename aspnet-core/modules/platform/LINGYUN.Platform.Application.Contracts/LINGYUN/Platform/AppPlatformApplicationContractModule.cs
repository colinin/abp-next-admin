using LINGYUN.Platform.Localization;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace LINGYUN.Platform
{
    [DependsOn(typeof(AppPlatformDomainSharedModule))]
    public class AppPlatformApplicationContractModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<AppPlatformApplicationContractModule>();
            });

            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Get<PlatformResource>()
                    .AddVirtualJson("/LINGYUN/Platform/Localization/ApplicationContracts");
            });

        }
    }
}
