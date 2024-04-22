using LINGYUN.Abp.Sms.Tencent;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Volo.Abp.Sms
{
    public static class TencentSmsSenderExtensions
    {
        /// <summary>
        /// 扩展短信接口
        /// </summary>
        /// <param name="smsSender"></param>
        /// <param name="templateCode">短信模板号</param>
        /// <param name="phoneNumber">发送手机号</param>
        /// <param name="templateParams">短信模板参数</param>
        /// <returns></returns>
        public static async Task SendAsync(
            this ISmsSender smsSender,
            string templateCode,
            string phoneNumber,
            IDictionary<string, object> templateParams = null)
        {
            var smsMessage = new SmsMessage(phoneNumber, nameof(TencentCloudSmsSender));
            smsMessage.Properties.Add("TemplateCode", templateCode);
            if(templateParams != null)
            {
                smsMessage.Properties.Add("TemplateParam", templateParams);
            }
            await smsSender.SendAsync(smsMessage);
        }

        /// <summary>
        /// 扩展短信接口
        /// </summary>
        /// <param name="smsSender"></param>
        /// <param name="signName">短信签名</param>
        /// <param name="templateCode">短信模板号</param>
        /// <param name="phoneNumber">发送手机号</param>
        /// <param name="templateParams">短信模板参数</param>
        /// <returns></returns>
        public static async Task SendAsync(
            this ISmsSender smsSender,
            string signName,
            string templateCode,
            string phoneNumber,
            IDictionary<string, object> templateParams = null)
        {
            var smsMessage = new SmsMessage(phoneNumber, nameof(TencentCloudSmsSender));
            smsMessage.Properties.Add("SignName", signName);
            smsMessage.Properties.Add("TemplateCode", templateCode);
            if (templateParams != null)
            {
                smsMessage.Properties.Add("TemplateParam", templateParams);
            }
            await smsSender.SendAsync(smsMessage);
        }
    }
}
