using System;
using Volo.Abp.BlobStoring;

namespace LINGYUN.Abp.BlobStoring.Nexus
{
    public static class NexusBlobContainerConfigurationExtensions
    {
        public static NexusBlobProviderConfiguration GetNexusConfiguration(
           this BlobContainerConfiguration containerConfiguration)
        {
            return new NexusBlobProviderConfiguration(containerConfiguration);
        }

        public static BlobContainerConfiguration UseNexus(
            this BlobContainerConfiguration containerConfiguration,
            Action<NexusBlobProviderConfiguration> nexusConfigureAction)
        {
            containerConfiguration.ProviderType = typeof(NexusBlobProvider);
            containerConfiguration.NamingNormalizers.TryAdd<NexusBlobNamingNormalizer>();

            nexusConfigureAction(new NexusBlobProviderConfiguration(containerConfiguration));

            return containerConfiguration;
        }
    }
}
