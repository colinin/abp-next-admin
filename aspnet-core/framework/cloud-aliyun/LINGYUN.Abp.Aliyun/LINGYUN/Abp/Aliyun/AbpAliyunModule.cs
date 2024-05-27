using LINGYUN.Abp.Aliyun.Localization;
using LINGYUN.Abp.Features.LimitValidation;
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
        typeof(AbpLocalizationModule),
        typeof(AbpFeaturesLimitValidationModule))]
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
                    .Add<AliyunResource>("zh-Hans") // 中国区云服务,默认使用简体中文
                    .AddVirtualJson("/LINGYUN/Abp/Aliyun/Localization/Resources");
            });
        }
    }
}
