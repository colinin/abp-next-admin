using COSXML;
using COSXML.Auth;
using LINGYUN.Abp.Tencent;
using Microsoft.Extensions.Caching.Memory;
using System.Threading.Tasks;
using Volo.Abp.BlobStoring;
using Volo.Abp.DependencyInjection;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Settings;

namespace LINGYUN.Abp.BlobStoring.Tencent;

public class CosClientFactory : AbstractTencentCloudClientFactory<CosXml, TencentBlobProviderConfiguration>,
    ICosClientFactory,
    ITransientDependency
{
    protected IBlobContainerConfigurationProvider ConfigurationProvider { get; }

    public CosClientFactory(
        IMemoryCache clientCache,
        ICurrentTenant currentTenant,
        ISettingProvider settingProvider,
        IBlobContainerConfigurationProvider configurationProvider)
        : base(clientCache, currentTenant, settingProvider)
    {
        ConfigurationProvider = configurationProvider;
    }

    public virtual async Task<CosXml> CreateAsync<TContainer>()
    {
        var configuration = ConfigurationProvider.Get<TContainer>();

        return await CreateAsync(configuration.GetTencentConfiguration());
    }

    protected override CosXml CreateClient(TencentBlobProviderConfiguration configuration, TencentCloudClientCacheItem cloudCache)
    {
        var configBuilder = new CosXmlConfig.Builder();
        configBuilder
            .SetAppid(configuration.AppId)
            .SetRegion(configuration.Region);

        var cred = new DefaultQCloudCredentialProvider(
            cloudCache.SecretId,
            cloudCache.SecretKey,
            cloudCache.DurationSecond);

        // TODO: 推荐全局单个对象，需要解决缓存过期事件
        return new CosXmlServer(configBuilder.Build(), cred);
    }
}
