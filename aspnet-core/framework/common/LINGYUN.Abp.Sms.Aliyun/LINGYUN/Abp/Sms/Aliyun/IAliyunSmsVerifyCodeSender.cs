using System.Threading.Tasks;

namespace LINGYUN.Abp.Sms.Aliyun;
/// <summary>
/// 阿里云发送短信验证码接口
/// </summary>
public interface IAliyunSmsVerifyCodeSender
{
    /// <summary>
    /// 发送短信验证码
    /// </summary>
    /// <param name="message"></param>
    /// <returns></returns>
    Task SendAsync(SmsVerifyCodeMessage message);
}
