using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Sms;

namespace LINGYUN.Abp.Notifications.Sms
{
    /// <summary>
    /// 短信通知的默认实现者
    /// </summary>
    public class SmsNotificationSender : ISmsNotificationSender, ITransientDependency
    {
        public ILogger Logger { protected get; set; }
        protected ISmsSender SmsSender { get; }

        public SmsNotificationSender(ISmsSender smsSender)
        {
            SmsSender = smsSender;

            Logger = NullLogger<SmsNotificationSender>.Instance;
        }

        /// <summary>
        /// 发送通知
        /// </summary>
        /// <param name="notification"></param>
        /// <param name="phoneNumbers"></param>
        /// <returns></returns>
        public virtual async Task SendAsync(NotificationInfo notification, string phoneNumbers)
        {
            var templateCode = notification.Data.TryGetData("TemplateCode");
            if (templateCode == null)
            {
                Logger.LogWarning("sms template code is empty, can not send sms message!");
                return;
            }
            var message = new SmsMessage(phoneNumbers, "SmsNotification");

            // TODO: 后期增强功能,增加短信模板、通知模板功能
            message.Properties.Add("TemplateCode", templateCode);
            message.Properties.Add("SignName", notification.Data.TryGetData("SignName"));

            await SmsSender.SendAsync(message);
        }
    }
}
