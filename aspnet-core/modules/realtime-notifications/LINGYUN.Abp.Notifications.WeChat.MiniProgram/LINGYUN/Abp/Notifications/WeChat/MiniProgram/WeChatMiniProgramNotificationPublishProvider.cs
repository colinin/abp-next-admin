using LINGYUN.Abp.WeChat.Common.Security.Claims;
using LINGYUN.Abp.WeChat.MiniProgram.Features;
using LINGYUN.Abp.WeChat.MiniProgram.Messages;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Features;

namespace LINGYUN.Abp.Notifications.WeChat.MiniProgram;

/// <summary>
/// 微信小程序消息推送提供者
/// </summary>
/// <remarks>
/// 小程序订阅消息的模板数据本身就是动态键值对, 因此载荷只声明标量字段,
/// 模板数据仍然直接取自通知数据的属性包。
/// </remarks>
public class WeChatMiniProgramNotificationPublishProvider : NotificationPublishProvider
{
    public const string ProviderName = NotificationProviderNames.WechatMiniProgram;
    public override string Name => ProviderName;
    protected IFeatureChecker FeatureChecker => ServiceProvider.LazyGetRequiredService<IFeatureChecker>();
    protected ISubscribeMessager SubscribeMessager => ServiceProvider.LazyGetRequiredService<ISubscribeMessager>();
    protected IOptions<AbpNotificationsWeChatMiniProgramOptions> Options => ServiceProvider.LazyGetRequiredService<IOptions<AbpNotificationsWeChatMiniProgramOptions>>();
    protected INotificationChannelDataResolver ChannelDataResolver => ServiceProvider.LazyGetRequiredService<INotificationChannelDataResolver>();

    protected async override Task<bool> CanPublishAsync(NotificationInfo notification, CancellationToken cancellationToken = default)
    {
        if (!await FeatureChecker.IsEnabledAsync(true,
            WeChatMiniProgramFeatures.Enable,
            WeChatMiniProgramFeatures.Messages.Enable))
        {
            Logger.LogWarning(
                "{0} cannot push messages because the feature {1} is not enabled",
                Name,
                WeChatMiniProgramFeatures.Messages.Enable);
            return false;
        }
        return true;
    }

    protected async override Task PublishAsync(NotificationPublishContext context, CancellationToken cancellationToken = default)
    {
        // 微信不支持推送到所有用户
        // 在小程序里用户订阅消息后通过 api/subscribes/subscribe 接口订阅对应模板消息
        var channelData = ResolveChannelData(context.Notification.Data);

        foreach (var identifier in context.Users)
        {
            if (channelData.TemplateId.IsNullOrWhiteSpace())
            {
                context.Cancel("Wechat weapp template id be empty, can not send notification!");
                Logger.LogWarning(context.Reason);
                continue;
            }

            Logger.LogDebug($"Get wechat weapp template id: {channelData.TemplateId}");

            Logger.LogDebug($"Get wechat weapp redirect page: {channelData.RedirectPage ?? "null"}");
            Logger.LogDebug($"Get wechat weapp state: {channelData.WeAppState ?? null}");
            Logger.LogDebug($"Get wechat weapp language: {channelData.WeAppLanguage ?? null}");

            // TODO: 如果微信端发布通知,请组装好 openid 字段在通知数据内容里面
            var openId = ChannelDataResolver.GetOrNull(context.Notification.Data, AbpWeChatClaimTypes.OpenId);

            if (openId.IsNullOrWhiteSpace())
            {
                // 发送小程序订阅消息
                await SubscribeMessager
                    .SendAsync(
                        identifier.UserId,
                        channelData.TemplateId!,
                        channelData.RedirectPage,
                        channelData.WeAppLanguage,
                        channelData.WeAppState,
                        context.Notification.Data.ExtraProperties,
                        cancellationToken);
            }
            else
            {
                var weChatWeAppNotificationData = new SubscribeMessage(
                    openId,
                    channelData.TemplateId!,
                    channelData.RedirectPage,
                    channelData.WeAppState,
                    channelData.WeAppLanguage);

                // 写入模板数据
                weChatWeAppNotificationData.WriteData(context.Notification.Data.ExtraProperties);

                Logger.LogDebug($"Sending wechat weapp notification: {context.Notification.Name}");

                // 发送小程序订阅消息
                await SubscribeMessager.SendAsync(weChatWeAppNotificationData, cancellationToken);

                Logger.LogDebug("The notification: {0} with provider: {1} has successfully published!", context.Notification.Name, Name);
            }
        }
    }

    /// <summary>
    /// 解析小程序渠道数据
    /// </summary>
    /// <remarks>
    /// 键名带本渠道前缀, 读取大小写不敏感;
    /// 为了让已安装的生产者平滑升级, 这里对历史遗留的**无前缀键**做只读回退;
    /// 缺省值来自渠道配置。
    /// </remarks>
    protected virtual WeChatMiniProgramSubscribeChannelData ResolveChannelData(NotificationData notificationData)
    {
        var channelData = ChannelDataResolver.Resolve<WeChatMiniProgramSubscribeChannelData>(notificationData)
            ?? new WeChatMiniProgramSubscribeChannelData();

        // 历史遗留的无前缀键只读回退
        ApplyLegacyKeys(channelData, notificationData);

        if (channelData.TemplateId.IsNullOrWhiteSpace())
        {
            channelData.TemplateId = Options.Value.DefaultTemplateId;
        }
        if (channelData.WeAppState.IsNullOrWhiteSpace())
        {
            channelData.WeAppState = Options.Value.DefaultState;
        }
        if (channelData.WeAppLanguage.IsNullOrWhiteSpace())
        {
            channelData.WeAppLanguage = Options.Value.DefaultLanguage;
        }

        return channelData;
    }

    /// <summary>
    /// 回退读取历史遗留的无前缀键
    /// </summary>
    /// <remarks>
    /// <para>
    /// 早期版本使用 <c>TemplateId</c> 这类无前缀键, 已安装的生产者可能仍在写入。
    /// 带前缀的键优先; 命中遗留键时记录一次警告以便推动迁移。
    /// </para>
    /// </remarks>
#pragma warning disable CS0618 // Type or member is obsolete: the only sanctioned read site for legacy keys
    protected virtual void ApplyLegacyKeys(
        WeChatMiniProgramSubscribeChannelData channelData,
        NotificationData notificationData)
    {
        if (channelData.TemplateId.IsNullOrWhiteSpace())
        {
            channelData.TemplateId = ReadLegacy(notificationData, WeChatMiniProgramNotificationDataKeys.Legacy.TemplateId);
        }
        if (channelData.RedirectPage.IsNullOrWhiteSpace())
        {
            channelData.RedirectPage = ReadLegacy(notificationData, WeChatMiniProgramNotificationDataKeys.Legacy.RedirectPage);
        }
        if (channelData.WeAppState.IsNullOrWhiteSpace())
        {
            channelData.WeAppState = ReadLegacy(notificationData, WeChatMiniProgramNotificationDataKeys.Legacy.WeAppState);
        }
        if (channelData.WeAppLanguage.IsNullOrWhiteSpace())
        {
            channelData.WeAppLanguage = ReadLegacy(notificationData, WeChatMiniProgramNotificationDataKeys.Legacy.WeAppLanguage);
        }
    }
#pragma warning restore CS0618

    private string? ReadLegacy(NotificationData notificationData, string legacyKey)
    {
        var value = ChannelDataResolver.GetOrNull(notificationData, legacyKey);
        if (!value.IsNullOrWhiteSpace())
        {
            Logger.LogWarning(
                "The wechat mini program notification data is using the legacy key '{LegacyKey}' without channel prefix. " +
                "Please migrate to the prefixed key '{Prefix}...'.",
                legacyKey,
                WeChatMiniProgramNotificationDataKeys.Prefix);
        }

        return value;
    }
}
