using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Sms;

namespace LINGYUN.Abp.Notifications.Sms
{
    public class SmsNotificationPublishProvider : NotificationPublishProvider
    {
        private IUserPhoneFinder _userPhoneFinder;
        protected IUserPhoneFinder UserPhoneFinder => LazyGetRequiredService(ref _userPhoneFinder);

        private ISmsSender _smsSender;
        protected ISmsSender SmsSender => LazyGetRequiredService(ref _smsSender);

        protected NotificationSmsOptions Options { get; }

        public SmsNotificationPublishProvider(
            IServiceProvider serviceProvider,
            IOptions<NotificationSmsOptions> options) 
            : base(serviceProvider)
        {
            Options = options.Value;
        }

        public override string Name => "Sms";

        protected override async Task PublishAsync(
            NotificationInfo notification,
            IEnumerable<UserIdentifier> identifiers,
            CancellationToken cancellationToken = default)
        {
            if (!identifiers.Any())
            {
                return;
            }

            var templateCode = notification.Data.TryGetData("TemplateCode");
            if (templateCode == null)
            {
                Logger.LogWarning("sms template code is empty, can not send sms message!");
                return;
            }

            var sendToPhones = await UserPhoneFinder.FindByUserIdsAsync(identifiers.Select(usr => usr.UserId), cancellationToken);
            if (!sendToPhones.Any())
            {
                return;
            }
            var message = new SmsMessage(sendToPhones.JoinAsString(","), "SmsNotification");

            // TODO: 后期增强功能,增加短信模板、通知模板功能
            message.Properties.Add("TemplateCode", templateCode);
            message.Properties.Add("SignName", notification.Data.TryGetData("SignName"));

            foreach (var property in notification.Data.Properties)
            {
                // TODO: 可以扩展下存储短信模板,根据模板变量自动匹配
                // 必须加上需要发送短信的前缀让用户自己选择是否发送短信,因为资费太贵了...
                if (property.Key.StartsWith(Options.TemplateParamsPrefix))
                {
                    message.Properties.Add(property.Key.Replace(Options.TemplateParamsPrefix, ""), property.Value);
                }
            }

            await SmsSender.SendAsync(message);
        }
    }
}
