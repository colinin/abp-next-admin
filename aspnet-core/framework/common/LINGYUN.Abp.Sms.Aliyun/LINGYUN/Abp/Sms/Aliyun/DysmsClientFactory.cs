using AlibabaCloud.SDK.Dysmsapi20170525;
using LINGYUN.Abp.Aliyun;
using System;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Settings;

namespace LINGYUN.Abp.Sms.Aliyun;

public class DysmsClientFactory : AliyunClientFactory<Client>, IDysmsClientFactory, ITransientDependency
{
    public DysmsClientFactory(ISettingProvider settingProvider, IDistributedCache<AliyunBasicSessionCredentialsCacheItem> cache) : base(settingProvider, cache)
    {
    }

    protected override Client GetClient(string regionId, string accessKeyId, string accessKeySecret)
    {
        return new Client(
            new AlibabaCloud.OpenApiClient.Models.Config
            {
                RegionId = regionId,
                AccessKeyId = accessKeyId,
                AccessKeySecret = accessKeySecret,
            });
    }

    protected override Client GetSecurityTokenClient(string regionId, string accessKeyId, string accessKeySecret, string securityToken, DateTime? expiration = null)
    {
        return new Client(
            new AlibabaCloud.OpenApiClient.Models.Config
            {
                RegionId = regionId,
                AccessKeyId = accessKeyId,
                AccessKeySecret = accessKeySecret,
                SecurityToken = securityToken,
            });
    }
}
