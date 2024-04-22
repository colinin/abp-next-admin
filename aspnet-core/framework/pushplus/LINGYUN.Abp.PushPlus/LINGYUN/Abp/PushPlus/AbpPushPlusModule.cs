using LINGYUN.Abp.Features.LimitValidation;
using LINGYUN.Abp.PushPlus.Localization;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Caching;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.Settings;
using Volo.Abp.VirtualFileSystem;

namespace LINGYUN.Abp.PushPlus;

[DependsOn(
    typeof(AbpSettingsModule),
    typeof(AbpCachingModule),
    typeof(AbpFeaturesLimitValidationModule))]
public class AbpPushPlusModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddPushPlusClient();

        //Configure<AbpSystemTextJsonSerializerOptions>(options =>
        //{
        //    options.UnsupportedTypes.TryAdd<PushPlusResponse<int>>();
        //    options.UnsupportedTypes.TryAdd<PushPlusResponse<string>>();
        //    options.UnsupportedTypes.TryAdd<PushPlusResponse<object>>();
        //    options.UnsupportedTypes.TryAdd<PushPlusResponse<PushPlusWebhook>>();

        //    options.UnsupportedTypes.TryAdd<PushPlusResponse<PushPlusMessage>>();
        //    options.UnsupportedTypes.TryAdd<PushPlusResponse<SendPushPlusMessageResult>>();
        //    options.UnsupportedTypes.TryAdd<PushPlusResponse<PushPlusPagedResponse<PushPlusMessage>>>();

        //    options.UnsupportedTypes.TryAdd<PushPlusResponse<PushPlusChannel>>();

        //    options.UnsupportedTypes.TryAdd<PushPlusResponse<PushPlusToken>>();

        //    options.UnsupportedTypes.TryAdd<PushPlusResponse<PushPlusTopicForMe>>();
        //    options.UnsupportedTypes.TryAdd<PushPlusResponse<PushPlusTopicProfile>>();
        //    options.UnsupportedTypes.TryAdd<PushPlusResponse<PushPlusTopicQrCode>>();
        //    options.UnsupportedTypes.TryAdd<PushPlusResponse<PushPlusPagedResponse<PushPlusTopicUser>>>();
        //    options.UnsupportedTypes.TryAdd<PushPlusResponse<PushPlusPagedResponse<PushPlusTopic>>>();

        //    options.UnsupportedTypes.TryAdd<PushPlusResponse<PushPlusUserLimitTime>>();
        //    options.UnsupportedTypes.TryAdd<PushPlusResponse<PushPlusUserProfile>>();
        //});

        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<AbpPushPlusModule>();
        });

        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Add<PushPlusResource>()
                .AddVirtualJson("/LINGYUN/Abp/PushPlus/Localization/Resources");
        });
    }
}
