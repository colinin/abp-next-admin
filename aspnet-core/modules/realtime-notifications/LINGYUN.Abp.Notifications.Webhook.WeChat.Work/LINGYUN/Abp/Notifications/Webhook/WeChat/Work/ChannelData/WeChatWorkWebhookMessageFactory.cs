using LINGYUN.Abp.WeChat.Work.Messages;
using LINGYUN.Abp.WeChat.Work.Messages.Models;
using Volo.Abp;

namespace LINGYUN.Abp.Notifications.Webhook.WeChat.Work;

/// <summary>
/// 企业微信 Webhook 消息工厂
/// </summary>
public static class WeChatWorkWebhookMessageFactory
{
    /// <summary>
    /// 注册内置消息工厂
    /// </summary>
    /// <remarks>
    /// 每种载荷对应一种 <c>msgtype</c>, 一一映射, 不做隐式降级:
    /// 生产者选择了模板卡片, 就一定发模板卡片, 而不是静默发成文本。
    /// 唯一的例外是 markdown 与 markdown_v2 的取舍, 由渠道选项
    /// <see cref="AbpNotificationsWebhookWeChatWorkOptions.UseMarkdownV2"/> 决定。
    /// </remarks>
    public static AbpNotificationsWebhookWeChatWorkMessageOptions MapDefaultMessageFactories(
        this AbpNotificationsWebhookWeChatWorkMessageOptions options)
    {
        Check.NotNull(options, nameof(options));

        return options
            .Map(WeChatWorkWebhookMessageKind.Text, context =>
            {
                var payload = (WeChatWorkWebhookTextChannelData)context.ChannelData;

                return WeChatWorkWebhookMessageFactoryResult.Create(
                    new WeChatWorkWebhookTextMessage(new WebhookTextMessage(payload.Content!)),
                    WeChatWorkWebhookMessageKind.Text);
            })
            .Map(WeChatWorkWebhookMessageKind.Markdown, context =>
            {
                var payload = (WeChatWorkWebhookMarkdownChannelData)context.ChannelData;

                return WeChatWorkWebhookMessageFactoryResult.Create(
                    new WeChatWorkWebhookMarkdownMessage(new WebhookMarkdownMessage(payload.Content!)),
                    WeChatWorkWebhookMessageKind.Markdown);
            })
            .Map(WeChatWorkWebhookMessageKind.MarkdownV2, context =>
            {
                var payload = (WeChatWorkWebhookMarkdownV2ChannelData)context.ChannelData;

                return WeChatWorkWebhookMessageFactoryResult.Create(
                    new WeChatWorkWebhookMarkdownV2Message(new WebhookMarkdownV2Message(payload.Content!)),
                    WeChatWorkWebhookMessageKind.MarkdownV2);
            })
            .Map(WeChatWorkWebhookMessageKind.Image, context =>
            {
                var payload = (WeChatWorkWebhookImageChannelData)context.ChannelData;

                return WeChatWorkWebhookMessageFactoryResult.Create(
                    new WeChatWorkWebhookImageMessage(payload.Image!),
                    WeChatWorkWebhookMessageKind.Image);
            })
            .Map(WeChatWorkWebhookMessageKind.News, context =>
            {
                var payload = (WeChatWorkWebhookNewsChannelData)context.ChannelData;

                return WeChatWorkWebhookMessageFactoryResult.Create(
                    new WeChatWorkWebhookNewsMessage(payload.News!),
                    WeChatWorkWebhookMessageKind.News);
            })
            .Map(WeChatWorkWebhookMessageKind.File, context =>
            {
                var payload = (WeChatWorkWebhookFileChannelData)context.ChannelData;

                return WeChatWorkWebhookMessageFactoryResult.Create(
                    new WeChatWorkWebhookFileMessage(payload.File!),
                    WeChatWorkWebhookMessageKind.File);
            })
            .Map(WeChatWorkWebhookMessageKind.Voice, context =>
            {
                var payload = (WeChatWorkWebhookVoiceChannelData)context.ChannelData;

                return WeChatWorkWebhookMessageFactoryResult.Create(
                    new WeChatWorkWebhookVoiceMessage(payload.Voice!),
                    WeChatWorkWebhookMessageKind.Voice);
            })
            .Map(WeChatWorkWebhookMessageKind.TemplateCard, context =>
            {
                var payload = (WeChatWorkWebhookTemplateCardChannelData)context.ChannelData;

                return WeChatWorkWebhookMessageFactoryResult.Create(
                    new WeChatWorkWebhookTemplateCardMessage(payload.TemplateCard!),
                    WeChatWorkWebhookMessageKind.TemplateCard);
            });
    }
}
