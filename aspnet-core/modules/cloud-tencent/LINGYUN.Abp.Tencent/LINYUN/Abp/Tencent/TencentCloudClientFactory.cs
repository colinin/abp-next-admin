using Microsoft.Extensions.Caching.Memory;
using System.Linq;
using TencentCloud.Common;
using TencentCloud.Common.Profile;
using Volo.Abp;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Settings;

namespace LINGYUN.Abp.Tencent;

public class TencentCloudClientFactory<TClient> : AbstractTencentCloudClientFactory<TClient>
{
    public TencentCloudClientFactory(
        IMemoryCache clientCache, 
        ICurrentTenant currentTenant, 
        ISettingProvider settingProvider) 
        : base(clientCache, currentTenant, settingProvider)
    {
    }

    protected override TClient CreateClient(TencentCloudClientCacheItem cloudCache)
    {
        var clientCtr = typeof(TClient)
            .GetConstructors()
            .Where(x => x.GetParameters().Length == 3)
            .FirstOrDefault();
        if (clientCtr != null)
        {
            var cred = new Credential
            {
                SecretId = cloudCache.SecretId,
                SecretKey = cloudCache.SecretKey,
            };

            var httpProfile = new HttpProfile
            {
                ReqMethod = cloudCache.HttpMethod ?? "POST",
                Timeout = cloudCache.Timeout,
                // 不同的api需要的区域不同, 有的
                Endpoint = cloudCache.ApiEndPoint,
                WebProxy = cloudCache.WebProxy,
            };
            var clientProfile = new ClientProfile
            {
                HttpProfile = httpProfile,
            };

            // 通过反射创建客户端实例
            // TODO: 如果影响到性能需要调整到通过Options手动创建实例
            return (TClient)clientCtr.Invoke(new object[] { cred, cloudCache.EndPoint, clientProfile });
        }

        throw new AbpException($"Failed to specify initialization Type for client {typeof(TClient).FullName}. Client instance could not be created");
    }
}
