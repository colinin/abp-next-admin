using System;
using Volo.Abp.BlobStoring;

namespace LINGYUN.Abp.BlobStoring.Tencent;

public static class TencentBlobContainerConfigurationExtensions
{
    public static TencentBlobProviderConfiguration GetTencentConfiguration(
       this BlobContainerConfiguration containerConfiguration)
    {
        return new TencentBlobProviderConfiguration(containerConfiguration);
    }

    public static BlobContainerConfiguration UseTencentCloud(
        this BlobContainerConfiguration containerConfiguration,
        Action<TencentBlobProviderConfiguration> tencentCloudConfigureAction)
    {
        containerConfiguration.ProviderType = typeof(TencentCloudBlobProvider);

        tencentCloudConfigureAction(new TencentBlobProviderConfiguration(containerConfiguration));

        return containerConfiguration;
    }
}
