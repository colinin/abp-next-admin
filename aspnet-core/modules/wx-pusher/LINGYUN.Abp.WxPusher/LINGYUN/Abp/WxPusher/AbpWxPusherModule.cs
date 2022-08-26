using LINGYUN.Abp.Features.LimitValidation;
using LINGYUN.Abp.WxPusher.Localization;
using LINGYUN.Abp.WxPusher.Messages;
using LINGYUN.Abp.WxPusher.QrCode;
using LINGYUN.Abp.WxPusher.User;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Collections.Generic;
using Volo.Abp.Caching;
using Volo.Abp.Json;
using Volo.Abp.Json.SystemTextJson;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.Settings;
using Volo.Abp.VirtualFileSystem;

namespace LINGYUN.Abp.WxPusher;

[DependsOn(
    typeof(AbpJsonModule),
    typeof(AbpSettingsModule),
    typeof(AbpCachingModule),
    typeof(AbpFeaturesLimitValidationModule))]
public class AbpWxPusherModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddWxPusherClient();
        context.Services.TryAddSingleton<IWxPusherUserStore>(NullWxPusherUserStore.Instance);

        Configure<AbpSystemTextJsonSerializerOptions>(options =>
        {
            options.UnsupportedTypes.TryAdd<WxPusherResult<int>>();
            options.UnsupportedTypes.TryAdd<WxPusherResult<string>>();
            options.UnsupportedTypes.TryAdd<WxPusherResult<CreateQrcodeResult>>();
            options.UnsupportedTypes.TryAdd<WxPusherResult<GetScanQrCodeResult>>();
            options.UnsupportedTypes.TryAdd<WxPusherResult<List<SendMessageResult>>>();
            options.UnsupportedTypes.TryAdd<WxPusherResult<WxPusherPagedResult<UserProfile>>>();
        });

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
