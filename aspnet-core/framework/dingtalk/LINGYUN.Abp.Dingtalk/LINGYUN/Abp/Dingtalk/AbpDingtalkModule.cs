using LINGYUN.Abp.Dingtalk.Localization;
using LINGYUN.Abp.Features.LimitValidation;
using Volo.Abp.Caching;
using Volo.Abp.Json;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.Settings;
using Volo.Abp.VirtualFileSystem;

namespace LINGYUN.Abp.Dingtalk;

[DependsOn(
    typeof(AbpCachingModule),
    typeof(AbpSettingsModule),
    typeof(AbpJsonModule),
    typeof(AbpLocalizationModule),
    typeof(AbpFeaturesLimitValidationModule))]
public class AbpDingtalkModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<AbpDingtalkModule>();
        });

        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Add<DingtalkReousrce>("zh-Hans")
                .AddVirtualJson("/LINGYUN/Abp/Dingtalk/Localization/Resources");
        });
    }
}
