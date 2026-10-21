using LINGYUN.Abp.WeChat.Work;
using LINGYUN.Abp.WeChat.Work.Messages.Models;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Json.SystemTextJson;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.Notifications.Webhook.WeChat.Work;

[DependsOn(
    typeof(AbpNotificationsWebhookModule),
    typeof(AbpWeChatWorkModule))]
public class AbpNotificationsWebhookWeChatWorkModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        // 允许使用方在 PreConfigure 中覆盖(例如关闭 markdown v2)
        var preActions = context.Services.GetPreConfigureActions<AbpNotificationsWebhookWeChatWorkOptions>();

        Configure<AbpNotificationsWebhookWeChatWorkOptions>(options =>
        {
            options.UseMarkdownV2 = true;

            preActions.Configure(options);
        });

        Configure<AbpNotificationsWebhookOptions>(options =>
        {
            options.Contributors.Add(new WeChatWorkWebhookNotificationContributor());
        });

        // 模板卡片是抽象类型: 注册类型转换器, 使其能在渠道数据(JSON)中往返
        Configure<AbpSystemTextJsonSerializerOptions>(options =>
        {
            options.JsonSerializerOptions.Converters.Add(new WebhookTemplateCardSystemTextJsonConverter());
        });

        // 声明本渠道支持的载荷形态(注册期即校验存在性契约)
        Configure<AbpNotificationChannelDataOptions>(options =>
        {
            options.Add<WeChatWorkWebhookTextChannelData>();
            options.Add<WeChatWorkWebhookMarkdownChannelData>();
            options.Add<WeChatWorkWebhookMarkdownV2ChannelData>();
            options.Add<WeChatWorkWebhookImageChannelData>();
            options.Add<WeChatWorkWebhookNewsChannelData>();
            options.Add<WeChatWorkWebhookFileChannelData>();
            options.Add<WeChatWorkWebhookVoiceChannelData>();
            options.Add<WeChatWorkWebhookTemplateCardChannelData>();
        });

        // 消息载体工厂分派
        Configure<AbpNotificationsWebhookWeChatWorkMessageOptions>(options =>
        {
            options.MapDefaultMessageFactories();
        });
    }
}
