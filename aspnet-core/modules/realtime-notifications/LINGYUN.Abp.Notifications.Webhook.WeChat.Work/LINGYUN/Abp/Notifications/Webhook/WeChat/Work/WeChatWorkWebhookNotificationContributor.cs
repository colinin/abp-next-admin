using LINGYUN.Abp.Notifications.Webhook.WeChat.Work;
using LINGYUN.Abp.WeChat.Work.Messages.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp;

namespace LINGYUN.Abp.Notifications.Webhook.WeChat.Work;

/// <summary>
/// 企业微信 Webhook 通知贡献者
/// </summary>
/// <remarks>
/// <para>
/// 优先解析生产者写入的**渠道数据(载荷)**, 载荷缺失时才回退到按
/// <see cref="NotificationInfo.ContentType"/> 推导,
/// 因此历史用法(只写标准数据)行为不变。
/// </para>
/// <para>
/// 一个 Webhook 只能承载一条消息, 因此声明多个载荷时取
/// <see cref="NotificationChannelDataAttribute.Priority"/> 最小的那个,
/// 并按 1:1 映射构造载体 —— 不再像旧实现那样把未知内容类型静默丢弃。
/// </para>
/// </remarks>
public class WeChatWorkWebhookNotificationContributor : IWebhookNotificationContributor
{
    /// <summary>
    /// 贡献者名称(仅用于日志)
    /// </summary>
    public string Name => WeChatWorkWebhookNotificationProvider.ProviderName;

    public async virtual Task ContributeAsync(IWebhookNotificationContext context)
    {
        Check.NotNull(context, nameof(context));

        var options = context.ServiceProvider
            .GetRequiredService<IOptions<AbpNotificationsWebhookWeChatWorkOptions>>().Value;
        var messageOptions = context.ServiceProvider
            .GetRequiredService<IOptions<AbpNotificationsWebhookWeChatWorkMessageOptions>>().Value;
        var resolver = context.ServiceProvider
            .GetRequiredService<INotificationChannelDataResolver>();
        var notificationDataSerializer = context.ServiceProvider
            .GetRequiredService<INotificationDataSerializer>();

        var standardData = await notificationDataSerializer.ToStandard(context.Notification.Data);

        try
        {
            var payloads = resolver.Resolve(
                WeChatWorkWebhookNotificationProvider.ProviderName,
                context.Notification.Data);

            if (payloads.Count == 0)
            {
                // 回退: 生产者未写入载荷, 按内容类型推导(保持历史行为)
                payloads = ResolveContentTypePayloads(
                    context.Notification.ContentType,
                    standardData.Message,
                    options);
            }

            if (payloads.Count == 0)
            {
                Logger(context).LogDebug(
                    "No wechat work webhook channel data was resolved for the notification: {name}, skip it.",
                    context.Notification.Name);

                return;
            }

            // 一个 Webhook 只能承载一条消息: 按 Priority 取第一个(解析结果已排序)
            var channelData = (WeChatWorkWebhookChannelData)payloads[0];

            ApplyStandardDataFallback(channelData, standardData);

            var message = messageOptions.Create(
                new WeChatWorkWebhookMessageFactoryContext(
                    context.Notification,
                    standardData,
                    channelData,
                    options));

            context.Webhook = new WebhookNotificationData(
                context.Notification.Name,
                message.Message);
            context.Handled = true;
        }
        catch (Exception ex)
        {
            Logger(context).LogWarning(
                "Failed to parse the content of the wechat work webhook message: {message}",
                ex.Message);
        }
    }

    /// <summary>
    /// 按通知内容类型推导载荷(兼容未写入渠道数据的历史生产者)
    /// </summary>
    protected virtual IReadOnlyList<NotificationChannelData> ResolveContentTypePayloads(
        NotificationContentType contentType,
        string? message,
        AbpNotificationsWebhookWeChatWorkOptions options)
    {
        var content = message ?? string.Empty;

        switch (contentType)
        {
            case NotificationContentType.Text:
                return new NotificationChannelData[]
                {
                    new WeChatWorkWebhookTextChannelData { Content = content }
                };

            case NotificationContentType.Markdown:
                return options.UseMarkdownV2
                    ? new NotificationChannelData[]
                    {
                        new WeChatWorkWebhookMarkdownV2ChannelData { Content = content }
                    }
                    : new NotificationChannelData[]
                    {
                        new WeChatWorkWebhookMarkdownChannelData { Content = content }
                    };

            case NotificationContentType.Html:
                // HTML 先转为 markdown 再投递
                var markdown = CommonMark.CommonMarkConverter.Convert(content);

                return options.UseMarkdownV2
                    ? new NotificationChannelData[]
                    {
                        new WeChatWorkWebhookMarkdownV2ChannelData { Content = markdown }
                    }
                    : new NotificationChannelData[]
                    {
                        new WeChatWorkWebhookMarkdownChannelData { Content = markdown }
                    };

            default:
                return Array.Empty<NotificationChannelData>();
        }
    }

    /// <summary>
    /// 用标准数据补全载荷中未指定的内容
    /// </summary>
    /// <remarks>
    /// 生产者只声明"用哪种载体"而不写内容时(例如只写 <c>wx-work-hook:markdown</c>),
    /// 使用已本地化/渲染完成的通知正文, 避免发出空内容。
    /// </remarks>
    protected virtual void ApplyStandardDataFallback(
        WeChatWorkWebhookChannelData channelData,
        NotificationStandardData standardData)
    {
        if (standardData.Message.IsNullOrWhiteSpace())
        {
            return;
        }

        switch (channelData)
        {
            case WeChatWorkWebhookTextChannelData text when text.Content.IsNullOrWhiteSpace():
                text.Content = standardData.Message;
                break;

            case WeChatWorkWebhookMarkdownChannelData markdown when markdown.Content.IsNullOrWhiteSpace():
                markdown.Content = standardData.Message;
                break;

            case WeChatWorkWebhookMarkdownV2ChannelData markdownV2 when markdownV2.Content.IsNullOrWhiteSpace():
                markdownV2.Content = standardData.Message;
                break;
        }
    }

    private static ILogger Logger(IWebhookNotificationContext context)
    {
        return context.ServiceProvider.GetService<ILogger<WeChatWorkWebhookNotificationContributor>>()
            ?? NullLogger<WeChatWorkWebhookNotificationContributor>.Instance;
    }
}
