using LINYUN.Abp.Aliyun.Localization;
using Volo.Abp.Caching;
using Volo.Abp.Json;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.Settings;
using Volo.Abp.VirtualFileSystem;

namespace LINGYUN.Abp.Aliyun
{
    [DependsOn(
        typeof(AbpCachingModule),
        typeof(AbpSettingsModule),
        typeof(AbpJsonModule),
        typeof(AbpLocalizationModule))]
    public class AbpAliyunModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<AbpAliyunModule>();
            });

            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Add<AliyunResource>()
                    .AddVirtualJson("/LINGYUN/Abp/Aliyun/Localization/Resources");
            });
        }
    }
}
