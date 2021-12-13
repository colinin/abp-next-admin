using System;
using Volo.Abp.BlobStoring;

namespace LINGYUN.Abp.BlobStoring.OssManagement;

public static class OssManagementBlobContainerConfigurationExtensions
{
    public static OssManagementBlobProviderConfiguration GetOssManagementConfiguration(
        this BlobContainerConfiguration containerConfiguration)
    {
        return new OssManagementBlobProviderConfiguration(containerConfiguration);
    }

    public static BlobContainerConfiguration UseOssManagement(
        this BlobContainerConfiguration containerConfiguration,
        Action<OssManagementBlobProviderConfiguration> fileSystemConfigureAction)
    {
        containerConfiguration.ProviderType = typeof(OssManagementBlobProvider);
        containerConfiguration.NamingNormalizers.TryAdd<OssManagementBlobNamingNormalizer>();

        fileSystemConfigureAction(new OssManagementBlobProviderConfiguration(containerConfiguration));

        return containerConfiguration;
    }
}
