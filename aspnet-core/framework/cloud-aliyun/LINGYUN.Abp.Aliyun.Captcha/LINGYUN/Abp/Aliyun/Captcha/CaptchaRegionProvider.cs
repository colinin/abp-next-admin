using System;
using System.Globalization;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.Aliyun.Captcha;

public class CaptchaRegionProvider : ICaptchaRegionProvider, ISingletonDependency
{
    public virtual string GetEndpoint()
    {
        return CultureInfo.CurrentCulture.Name.Contains("zh", StringComparison.CurrentCultureIgnoreCase) 
            ? "captcha-dualstack.cn-shanghai.aliyuncs.com"
            : "captcha-dualstack.ap-southeast-1.aliyuncs.com";
    }

    public virtual string GetRegion()
    {
        return CultureInfo.CurrentCulture.Name.Contains("zh", StringComparison.CurrentCultureIgnoreCase) ? "cn" : "sgp";
    }
}
