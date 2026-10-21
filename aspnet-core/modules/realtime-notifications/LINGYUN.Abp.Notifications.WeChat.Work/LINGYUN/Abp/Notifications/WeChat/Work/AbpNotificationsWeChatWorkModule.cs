using LINGYUN.Abp.WeChat.Work;
using LINGYUN.Abp.WeChat.Work.Localization;
using LINGYUN.Abp.WeChat.Work.Messages.Models;
using Volo.Abp.Json.SystemTextJson;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace LINGYUN.Abp.Notifications.WeChat.Work;

[DependsOn(
    typeof(AbpWeChatWorkModule),
    typeof(AbpNotificationsModule))]
public class AbpNotificationsWeChatWorkModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<AbpNotificationsWeChatWorkModule>();
        });

        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Get<WeChatWorkResource>()
                .AddVirtualJson("/LINGYUN/Abp/Notifications/WeChat/Work/Localization/Resources");
        });

        Configure<AbpNotificationsPublishOptions>(options =>
        {
            options.PublishProviders.Add<WeChatWorkNotificationPublishProvider>();
        });

        // 模板卡片是抽象类型: 注册类型转换器, 使其能在渠道数据(JSON)中往返
        Configure<AbpSystemTextJsonSerializerOptions>(options =>
        {
            options.JsonSerializerOptions.Converters.Add(new TemplateCardSystemTextJsonConverter());
        });

        // 声明本渠道支持的载荷形态(注册期即校验存在性契约)
        Configure<AbpNotificationChannelDataOptions>(options =>
        {
            options.Add<WeChatWorkMiniProgramNoticeChannelData>();
            options.Add<WeChatWorkMpNewChannelData>();
            options.Add<WeChatWorkTemplateCardChannelData>();
            options.Add<WeChatWorkTextCardChannelData>();
            options.Add<WeChatWorkVideoChannelData>();
            options.Add<WeChatWorkVoiceChannelData>();
            options.Add<WeChatWorkFileChannelData>();
        });

        // 消息载体工厂分派
        Configure<AbpWeChatWorkNotificationMessageOptions>(options =>
        {
            options.MapDefaultMessageFactories();
        });
    }
}
