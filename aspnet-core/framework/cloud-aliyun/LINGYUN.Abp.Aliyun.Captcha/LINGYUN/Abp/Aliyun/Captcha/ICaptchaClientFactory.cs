using AlibabaCloud.SDK.Captcha20230305;
using System.Threading.Tasks;

namespace LINGYUN.Abp.Aliyun.Captcha;

public interface ICaptchaClientFactory
{
    /// <summary>
    /// 构建验证码客户端
    /// </summary>
    /// <returns></returns>
    Task<Client> CreateAsync();
}
