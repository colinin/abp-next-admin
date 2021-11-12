using LINGYUN.Abp.WeChat.MiniProgram.Messages;
using LINGYUN.Abp.WeChat.Security.Claims;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace LINGYUN.Abp.Notifications.WeChat.MiniProgram
{
    /// <summary>
    /// 微信小程序消息推送提供者
    /// </summary>
    public class WeChatMiniProgramNotificationPublishProvider : NotificationPublishProvider
    {
        public const string ProviderName = "WeChat.MiniProgram";
        public override string Name => ProviderName;
        protected ISubscribeMessager SubscribeMessager { get; }
        protected AbpNotificationsWeChatMiniProgramOptions Options { get; }
        public WeChatMiniProgramNotificationPublishProvider(
            IServiceProvider serviceProvider,
            ISubscribeMessager subscribeMessager,
            IOptions<AbpNotificationsWeChatMiniProgramOptions> options)
            : base(serviceProvider)
        {
            Options = options.Value;
            SubscribeMessager = subscribeMessager;
        }

        protected override async Task PublishAsync(NotificationInfo notification, IEnumerable<UserIdentifier> identifiers, CancellationToken cancellationToken = default)
        {
            // step1 默认微信openid绑定的就是username,
            // 如果不是,需要自行处理openid获取逻辑

            // step2 调用微信消息推送接口

            // 微信不支持推送到所有用户
            // 在小程序里用户订阅消息后通过 api/subscribes/subscribe 接口订阅对应模板消息
            foreach (var identifier in identifiers)
            {
                await SendWeChatTemplateMessagAsync(notification, identifier, cancellationToken);
            }
        }

        protected virtual async Task SendWeChatTemplateMessagAsync(NotificationInfo notification, UserIdentifier identifier, CancellationToken cancellationToken = default)
        {
            var templateId = GetOrDefaultTemplateId(notification.Data);
            if (templateId.IsNullOrWhiteSpace())
            {
                Logger.LogWarning("Wechat weapp template id be empty, can not send notification!");
                return;
            }

            Logger.LogDebug($"Get wechat weapp template id: {templateId}");

            var redirect = GetOrDefault(notification.Data, "RedirectPage", null);
            Logger.LogDebug($"Get wechat weapp redirect page: {redirect ?? "null"}");

            var weAppState = GetOrDefault(notification.Data, "WeAppState", Options.DefaultState);
            Logger.LogDebug($"Get wechat weapp state: {weAppState ?? null}");

            var weAppLang = GetOrDefault(notification.Data, "WeAppLanguage", Options.DefaultLanguage);
            Logger.LogDebug($"Get wechat weapp language: {weAppLang ?? null}");

            // TODO: 如果微信端发布通知,请组装好 openid 字段在通知数据内容里面
            string openId = GetOrDefault(notification.Data, AbpWeChatClaimTypes.OpenId, "");

            if (openId.IsNullOrWhiteSpace())
            {
                // 发送小程序订阅消息
                await SubscribeMessager
                    .SendAsync(
                        identifier.UserId, templateId, redirect, weAppLang,
                        weAppState, notification.Data.ExtraProperties, cancellationToken);
            }
            else
            {
                var weChatWeAppNotificationData = new SubscribeMessage(templateId, redirect, weAppState, weAppLang);
                // 写入模板数据
                weChatWeAppNotificationData.WriteData(notification.Data.ExtraProperties);

                Logger.LogDebug($"Sending wechat weapp notification: {notification.Name}");

                // 发送小程序订阅消息
                await SubscribeMessager.SendAsync(weChatWeAppNotificationData, cancellationToken);
            }
        }

        protected string GetOrDefaultTemplateId(NotificationData data)
        {
            return GetOrDefault(data, "TemplateId", Options.DefaultTemplateId);
        }

        protected string GetOrDefault(NotificationData data, string key, string defaultValue)
        {
            if (data.ExtraProperties.TryGetValue(key, out object value))
            {
                // 取得了数据就删除对应键值
                // data.Properties.Remove(key);
                return value.ToString();
            }
            return defaultValue;
        }
    }
}
