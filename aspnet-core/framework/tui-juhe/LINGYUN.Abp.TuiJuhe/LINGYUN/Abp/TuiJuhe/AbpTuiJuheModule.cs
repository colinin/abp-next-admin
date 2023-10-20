using LINGYUN.Abp.Features.LimitValidation;
using LINGYUN.Abp.TuiJuhe.Localization;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Caching;
using Volo.Abp.Json.SystemTextJson;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.Settings;
using Volo.Abp.VirtualFileSystem;

namespace LINGYUN.Abp.TuiJuhe;

[DependsOn(
    typeof(AbpJsonSystemTextJsonModule),
    typeof(AbpSettingsModule),
    typeof(AbpCachingModule),
    typeof(AbpFeaturesLimitValidationModule))]
public class AbpTuiJuheModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddTuiJuheClient();

        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<AbpTuiJuheModule>();
        });

        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Add<TuiJuheResource>()
                .AddVirtualJson("/LINGYUN/Abp/TuiJuhe/Localization/Resources");
        });
    }
}
