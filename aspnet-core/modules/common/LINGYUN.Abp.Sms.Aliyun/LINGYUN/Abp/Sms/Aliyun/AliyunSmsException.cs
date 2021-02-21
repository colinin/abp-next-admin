using LINGYUN.Abp.Aliyun;

namespace LINGYUN.Abp.Sms.Aliyun
{
    public class AliyunSmsException : AbpAliyunException
    {
        public AliyunSmsException(string code, string message)
            :base(code, message)
        {
        }
    }
}
