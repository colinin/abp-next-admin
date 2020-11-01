using LINGYUN.Abp.Notifications.WeChat.WeApp.Features;
using LINGYUN.Abp.WeChat.Authorization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Features;

namespace LINGYUN.Abp.Notifications.WeChat.WeApp
{
    /// <summary>
    /// 微信小程序消息推送提供者
    /// </summary>
    public class WeChatWeAppNotificationPublishProvider : NotificationPublishProvider
    {
        public override string Name => "WeChat.WeApp";

        private IFeatureChecker _featureChecker;
        protected IFeatureChecker FeatureChecker => LazyGetRequiredService(ref _featureChecker);

        private IWeChatOpenIdFinder _weChatOpenIdFinder;
        protected IWeChatOpenIdFinder WeChatOpenIdFinder => LazyGetRequiredService(ref _weChatOpenIdFinder);

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

        protected override async Task PublishAsync(NotificationInfo notification, IEnumerable<UserIdentifier> identifiers, CancellationToken cancellationToken = default)
        {

            // 先检测微信小程序的功能限制
            var publishEnabled = await FeatureChecker.GetAsync(WeChatWeAppFeatures.Notifications.Default, false);

            if (!publishEnabled)
            {
                return;
            }

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

            var weAppState = GetOrDefault(notification.Data, "WeAppState", Options.DefaultWeAppState);
            Logger.LogDebug($"Get wechat weapp state: {weAppState ?? null}");

            var weAppLang = GetOrDefault(notification.Data, "WeAppLanguage", Options.DefaultWeAppLanguage);
            Logger.LogDebug($"Get wechat weapp language: {weAppLang ?? null}");

            // TODO: 如果微信端发布通知,请组装好 wx-code 字段在通知数据内容里面
            string weChatCode = GetOrDefault(notification.Data, "wx-code", "");

            WeChatOpenId openId = weChatCode.IsNullOrWhiteSpace()
                ? await WeChatOpenIdFinder.FindByUserNameAsync(identifier.UserName) // 按照实际情况,需要自行实现 IUserWeChatCodeFinder 接口以获取微信Code,然后通过Code来获取OpenId
                : await WeChatOpenIdFinder.FindAsync(weChatCode);
            

            var weChatWeAppNotificationData = new WeChatWeAppSendNotificationData(openId.OpenId,
                templateId, redirect, weAppState, weAppLang);

            // 写入模板数据
            weChatWeAppNotificationData.WriteStandardData(NotificationData.ToStandardData(Options.DefaultMsgPrefix, notification.Data));

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
                // data.Properties.Remove(key);
                return value.ToString();
            }
            return defaultValue;
        }
    }
}
