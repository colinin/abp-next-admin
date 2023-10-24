using Microsoft.Extensions.Logging;
using Volo.Abp;
using Volo.Abp.ExceptionHandling;
using Volo.Abp.Logging;

namespace LINGYUN.Abp.Aliyun
{
    public class AbpAliyunException : AbpException, IHasErrorCode, IHasLogLevel
    {
        public LogLevel LogLevel { get; set; }

        public string Code { get; }

        public AbpAliyunException(string code, string message)
            : base(message)
        {
            Code = code;
            LogLevel = LogLevel.Warning;
        }
    }
}
