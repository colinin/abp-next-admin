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
public class WeChatMiniProgramNotificationPublishProvider : NotificationPublishProvider
{
    public const string ProviderName = NotificationProviderNames.WechatMiniProgram;
    public override string Name => ProviderName;
    protected IFeatureChecker FeatureChecker => ServiceProvider.LazyGetRequiredService<IFeatureChecker>();
    protected ISubscribeMessager SubscribeMessager => ServiceProvider.LazyGetRequiredService<ISubscribeMessager>();
    protected IOptions<AbpNotificationsWeChatMiniProgramOptions> Options => ServiceProvider.LazyGetRequiredService<IOptions<AbpNotificationsWeChatMiniProgramOptions>>();

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
        // step1 默认微信openid绑定的就是username,
        // 如果不是,需要自行处理openid获取逻辑

        // step2 调用微信消息推送接口

        // 微信不支持推送到所有用户
        // 在小程序里用户订阅消息后通过 api/subscribes/subscribe 接口订阅对应模板消息
        foreach (var identifier in context.Users)
        {
            var templateId = GetOrDefaultTemplateId(context.Notification.Data);
            if (templateId.IsNullOrWhiteSpace())
            {
                context.Cancel("Wechat weapp template id be empty, can not send notification!");
                Logger.LogWarning(context.Reason);
                continue;
            }

            Logger.LogDebug($"Get wechat weapp template id: {templateId}");

            var redirect = GetOrDefault(context.Notification.Data, "RedirectPage", null);
            Logger.LogDebug($"Get wechat weapp redirect page: {redirect ?? "null"}");

            var weAppState = GetOrDefault(context.Notification.Data, "WeAppState", Options.Value.DefaultState);
            Logger.LogDebug($"Get wechat weapp state: {weAppState ?? null}");

            var weAppLang = GetOrDefault(context.Notification.Data, "WeAppLanguage", Options.Value.DefaultLanguage);
            Logger.LogDebug($"Get wechat weapp language: {weAppLang ?? null}");

            // TODO: 如果微信端发布通知,请组装好 openid 字段在通知数据内容里面
            var openId = GetOrDefault(context.Notification.Data, AbpWeChatClaimTypes.OpenId, "");

            if (openId.IsNullOrWhiteSpace())
            {
                // 发送小程序订阅消息
                await SubscribeMessager
                    .SendAsync(
                        identifier.UserId, templateId, redirect, weAppLang,
                        weAppState, context.Notification.Data.ExtraProperties, cancellationToken);
            }
            else
            {
                var weChatWeAppNotificationData = new SubscribeMessage(templateId, redirect, weAppState, weAppLang);
                // 写入模板数据
                weChatWeAppNotificationData.WriteData(context.Notification.Data.ExtraProperties);

                Logger.LogDebug($"Sending wechat weapp notification: {context.Notification.Name}");

                // 发送小程序订阅消息
                await SubscribeMessager.SendAsync(weChatWeAppNotificationData, cancellationToken);

                Logger.LogDebug("The notification: {0} with provider: {1} has successfully published!", context.Notification.Name, Name);
            }
        }
    }

    protected string GetOrDefaultTemplateId(NotificationData data)
    {
        return GetOrDefault(data, "TemplateId", Options.Value.DefaultTemplateId);
    }

    protected string GetOrDefault(NotificationData data, string key, string defaultValue)
    {
        if (data.ExtraProperties.TryGetValue(key, out var value))
        {
            // 取得了数据就删除对应键值
            // data.Properties.Remove(key);
            return value.ToString();
        }
        return defaultValue;
    }
}
