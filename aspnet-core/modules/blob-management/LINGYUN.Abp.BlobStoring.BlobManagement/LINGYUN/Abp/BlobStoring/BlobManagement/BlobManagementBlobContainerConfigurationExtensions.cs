using System;
using Volo.Abp.BlobStoring;

namespace LINGYUN.Abp.BlobStoring.BlobManagement;

public static class BlobManagementBlobContainerConfigurationExtensions
{
    public static BlobManagementBlobProviderConfiguration GetBlobManagementConfiguration(
    this BlobContainerConfiguration containerConfiguration)
    {
        return new BlobManagementBlobProviderConfiguration(containerConfiguration);
    }

    public static BlobContainerConfiguration UseBlobManagement(
        this BlobContainerConfiguration containerConfiguration,
        Action<BlobManagementBlobProviderConfiguration>? blobConfigureAction = null)
    {
        containerConfiguration.ProviderType = typeof(BlobManagementBlobProvider);
        containerConfiguration.NamingNormalizers.TryAdd<BlobManagementBlobNamingNormalizer>();

        blobConfigureAction?.Invoke(new BlobManagementBlobProviderConfiguration(containerConfiguration));

        return containerConfiguration;
    }
}
