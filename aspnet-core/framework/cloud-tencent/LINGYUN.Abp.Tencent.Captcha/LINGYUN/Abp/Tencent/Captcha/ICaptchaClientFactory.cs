using System.Threading.Tasks;
using TencentCloud.Captcha.V20190722;

namespace LINGYUN.Abp.Tencent.Captcha;

public interface ICaptchaClientFactory
{
    Task<CaptchaClient> CreateAsync();
}
