using LINYUN.Abp.Aliyun;

namespace LINYUN.Abp.Sms.Aliyun
{
    public class AliyunSmsException : AbpAliyunException
    {
        public AliyunSmsException(string code, string message)
            :base(code, message)
        {
        }
    }
}
