using LINYUN.Abp.Tencent.Localization;
using Volo.Abp.Json;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace LINGYUN.Abp.Tencent
{
    [DependsOn(
        typeof(AbpJsonModule),
        typeof(AbpLocalizationModule))]
    public class AbpTencentCloudModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<AbpTencentCloudModule>();
            });

            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Add<TencentResource>()
                    .AddVirtualJson("/LINGYUN/Abp/Tencent/Localization/Resources");
            });
        }
    }
}
