using LINYUN.Abp.Aliyun.Localization;
using Volo.Abp.Json;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace LINGYUN.Abp.Aliyun
{
    [DependsOn(
        typeof(AbpJsonModule),
        typeof(AbpLocalizationModule))]
    public class AbpAliyunCloudModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<AbpAliyunCloudModule>();
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
