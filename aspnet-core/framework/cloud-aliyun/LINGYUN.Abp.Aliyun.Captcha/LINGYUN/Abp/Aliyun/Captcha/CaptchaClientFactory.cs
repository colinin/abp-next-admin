using AlibabaCloud.SDK.Captcha20230305;
using LINGYUN.Abp.Aliyun;
using System;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Settings;

namespace LINGYUN.Abp.Aliyun.Captcha;

public class CaptchaClientFactory : AliyunClientFactory<Client>, ICaptchaClientFactory, ITransientDependency
{
    protected ICaptchaRegionProvider CaptchaRegionProvider { get; }
    public CaptchaClientFactory(
        ICaptchaRegionProvider captchaRegionProvider,
        ISettingProvider settingProvider, 
        IDistributedCache<AliyunBasicSessionCredentialsCacheItem> cache) 
        : base(settingProvider, cache)
    {
        CaptchaRegionProvider = captchaRegionProvider;
    }

    protected override Client GetClient(string regionId, string accessKeyId, string accessKeySecret)
    {
        return new Client(
            new AlibabaCloud.OpenApiClient.Models.Config
            {
                Endpoint = CaptchaRegionProvider.GetEndpoint(),
                AccessKeyId = accessKeyId,
                AccessKeySecret = accessKeySecret,
            });
    }

    protected override Client GetSecurityTokenClient(string regionId, string accessKeyId, string accessKeySecret, string securityToken, DateTime? expiration = null)
    {
        return new Client(
            new AlibabaCloud.OpenApiClient.Models.Config
            {
                Endpoint = CaptchaRegionProvider.GetEndpoint(),
                AccessKeyId = accessKeyId,
                AccessKeySecret = accessKeySecret,
                SecurityToken = securityToken,
            });
    }
}
