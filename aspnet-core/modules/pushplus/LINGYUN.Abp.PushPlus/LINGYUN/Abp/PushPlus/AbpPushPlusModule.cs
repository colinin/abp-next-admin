using LINGYUN.Abp.Features.LimitValidation;
using LINGYUN.Abp.PushPlus.Channel.Webhook;
using LINGYUN.Abp.PushPlus.Message;
using LINGYUN.Abp.PushPlus.Setting;
using LINGYUN.Abp.PushPlus.Token;
using LINGYUN.Abp.PushPlus.Topic;
using LINGYUN.Abp.PushPlus.User;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Caching;
using Volo.Abp.Json;
using Volo.Abp.Json.SystemTextJson;
using Volo.Abp.Modularity;
using Volo.Abp.Settings;

namespace LINGYUN.Abp.PushPlus;

[DependsOn(
    typeof(AbpJsonModule),
    typeof(AbpSettingsModule),
    typeof(AbpCachingModule),
    typeof(AbpFeaturesLimitValidationModule))]
public class AbpPushPlusModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddPushPlusClient();

        Configure<AbpSystemTextJsonSerializerOptions>(options =>
        {
            options.UnsupportedTypes.TryAdd<PushPlusResponse<int>>();
            options.UnsupportedTypes.TryAdd<PushPlusResponse<string>>();
            options.UnsupportedTypes.TryAdd<PushPlusResponse<object>>();
            options.UnsupportedTypes.TryAdd<PushPlusResponse<PushPlusWebhook>>();

            options.UnsupportedTypes.TryAdd<PushPlusResponse<PushPlusMessage>>();
            options.UnsupportedTypes.TryAdd<PushPlusResponse<SendPushPlusMessageResult>>();
            options.UnsupportedTypes.TryAdd<PushPlusResponse<PushPlusPagedResponse<PushPlusMessage>>>();

            options.UnsupportedTypes.TryAdd<PushPlusResponse<PushPlusChannel>>();

            options.UnsupportedTypes.TryAdd<PushPlusResponse<PushPlusToken>>();

            options.UnsupportedTypes.TryAdd<PushPlusResponse<PushPlusTopicForMe>>();
            options.UnsupportedTypes.TryAdd<PushPlusResponse<PushPlusTopicProfile>>();
            options.UnsupportedTypes.TryAdd<PushPlusResponse<PushPlusTopicQrCode>>();
            options.UnsupportedTypes.TryAdd<PushPlusResponse<PushPlusPagedResponse<PushPlusTopicUser>>>();
            options.UnsupportedTypes.TryAdd<PushPlusResponse<PushPlusPagedResponse<PushPlusTopic>>>();

            options.UnsupportedTypes.TryAdd<PushPlusResponse<PushPlusUserLimitTime>>();
            options.UnsupportedTypes.TryAdd<PushPlusResponse<PushPlusUserProfile>>();
        });
    }
}
