using Microsoft.Extensions.Caching.Memory;
using TencentCloud.Captcha.V20190722;
using TencentCloud.Common;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Settings;

namespace LINGYUN.Abp.Tencent.Captcha;

public class CaptchaClientFactory : AbstractTencentCloudClientFactory<CaptchaClient>, ICaptchaClientFactory, ITransientDependency
{
    public CaptchaClientFactory(
        IMemoryCache clientCache, 
        ISettingProvider settingProvider) 
        : base(clientCache, settingProvider)
    {
    }

    protected override CaptchaClient CreateClient(TencentCloudClientCacheItem cloudCache)
    {
        return new CaptchaClient(
            new Credential
            {
                SecretId = cloudCache.SecretId,
                SecretKey = cloudCache.SecretKey
            },
            "");
    }
}
