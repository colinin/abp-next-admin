using Microsoft.Extensions.Logging;
using Volo.Abp;
using Volo.Abp.ExceptionHandling;
using Volo.Abp.Localization;
using Volo.Abp.Logging;

namespace LINYUN.Abp.Sms.Aliyun
{
    public class AliyunSmsException : AbpException, IHasErrorCode, ILocalizeErrorMessage, IHasLogLevel
    {
        public AliyunSmsException(string code, string message)
            :base(message)
        {
            Code = code;
            LogLevel = LogLevel.Warning;
        }

        public LogLevel LogLevel { get; set; }

        public string Code { get; }

        public string LocalizeMessage(LocalizationContext context)
        {
            return AliyunSmsResponse.GetErrorMessage(Code, Message)
                .Localize(context.LocalizerFactory);
        }
    }
}
