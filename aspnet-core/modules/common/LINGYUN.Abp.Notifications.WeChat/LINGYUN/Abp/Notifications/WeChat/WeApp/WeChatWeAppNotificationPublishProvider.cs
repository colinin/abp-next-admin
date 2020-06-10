using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LINGYUN.Abp.Notifications.WeChat.WeApp
{
    /// <summary>
    /// 微信小程序消息推送提供者
    /// </summary>
    public class WeChatWeAppNotificationPublishProvider : NotificationPublishProvider
    {
        public override string Name => "WeChat.WeApp";

        protected IWeChatWeAppNotificationSender NotificationSender { get; }
        protected AbpWeChatWeAppNotificationOptions Options { get; }
        public WeChatWeAppNotificationPublishProvider(
            IServiceProvider serviceProvider,
            IWeChatWeAppNotificationSender notificationSender,
            IOptions<AbpWeChatWeAppNotificationOptions> options) 
            : base(serviceProvider)
        {
            Options = options.Value;
            NotificationSender = notificationSender;
        }

        public override async Task PublishAsync(NotificationInfo notification, IEnumerable<UserIdentifier> identifiers)
        {
            // step1 默认微信openid绑定的就是username,
            // 如果不是,需要自行处理openid获取逻辑

            // step2 调用微信消息推送接口

            foreach (var identifier in identifiers)
            {
                await SendWeChatTemplateMessagAsync(notification, identifier);
            }
        }

        protected virtual async Task SendWeChatTemplateMessagAsync(NotificationInfo notification, UserIdentifier identifier)
        {
            var templateId = GetOrDefaultTemplateId(notification.Data);
            Logger.LogDebug($"Get wechat weapp template id: {templateId}");

            var redirect = GetOrDefault(notification.Data, "RedirectPage", null);
            Logger.LogDebug($"Get wechat weapp redirect page: {redirect ?? "null"}");

            var weAppState = GetOrDefault(notification.Data, "WeAppState", Options.DefaultWeAppState);
            Logger.LogDebug($"Get wechat weapp state: {weAppState ?? null}");

            var weAppLang = GetOrDefault(notification.Data, "WeAppLanguage", Options.DefaultWeAppLanguage);
            Logger.LogDebug($"Get wechat weapp language: {weAppLang ?? null}");

            var weChatWeAppNotificationData = new WeChatWeAppSendNotificationData(identifier.UserName,
                templateId, redirect, weAppState, weAppLang);

            // 写入模板数据
            weChatWeAppNotificationData.WriteData(Options.DefaultMsgPrefix, notification.Data.Properties);

            Logger.LogDebug($"Sending wechat weapp notification: {notification.Name}");
            // 发送小程序订阅消息
            await NotificationSender.SendAsync(weChatWeAppNotificationData);
        }

        protected string GetOrDefaultTemplateId(NotificationData data)
        {
            return GetOrDefault(data, "TemplateId", Options.DefaultTemplateId);
        }

        protected string GetOrDefault(NotificationData data, string key, string defaultValue)
        {
            if (data.Properties.TryGetValue(key, out object value))
            {
                // 取得了数据就删除对应键值
                data.Properties.Remove(key);
                return value.ToString();
            }
            return defaultValue;
        }
    }
}
