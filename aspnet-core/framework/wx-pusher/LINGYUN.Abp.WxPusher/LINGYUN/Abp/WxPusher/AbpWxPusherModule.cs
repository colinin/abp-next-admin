using LINGYUN.Abp.Features.LimitValidation;
using LINGYUN.Abp.WxPusher.Localization;
using LINGYUN.Abp.WxPusher.User;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Volo.Abp.Caching;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.Settings;
using Volo.Abp.VirtualFileSystem;

namespace LINGYUN.Abp.WxPusher;

[DependsOn(
    typeof(AbpSettingsModule),
    typeof(AbpCachingModule),
    typeof(AbpFeaturesLimitValidationModule))]
public class AbpWxPusherModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddWxPusherClient();
        context.Services.TryAddSingleton<IWxPusherUserStore>(NullWxPusherUserStore.Instance);

        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<AbpWxPusherModule>();
        });

        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Add<WxPusherResource>()
                .AddVirtualJson("/LINGYUN/Abp/WxPusher/Localization/Resources");
        });
    }
}
