using System;
using Volo.Abp.BlobStoring;

namespace LINGYUN.Abp.BlobStoring.Aliyun
{
    public static class AliyunBlobContainerConfigurationExtensions
    {
        public static AliyunBlobProviderConfiguration GetAliyunConfiguration(
           this BlobContainerConfiguration containerConfiguration)
        {
            return new AliyunBlobProviderConfiguration(containerConfiguration);
        }

        public static BlobContainerConfiguration UseAliyun(
            this BlobContainerConfiguration containerConfiguration,
            Action<AliyunBlobProviderConfiguration> aliyunConfigureAction)
        {
            containerConfiguration.ProviderType = typeof(AliyunBlobProvider);

            aliyunConfigureAction(new AliyunBlobProviderConfiguration(containerConfiguration));

            return containerConfiguration;
        }
    }
}
