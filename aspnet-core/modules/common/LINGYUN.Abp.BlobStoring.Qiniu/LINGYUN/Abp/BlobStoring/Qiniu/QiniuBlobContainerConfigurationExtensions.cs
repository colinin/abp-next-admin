using System;
using Volo.Abp.BlobStoring;

namespace LINGYUN.Abp.BlobStoring.Qiniu
{
    public static class QiniuBlobContainerConfigurationExtensions
    {
        public static QiniuBlobProviderConfiguration GetQiniuConfiguration(
           this BlobContainerConfiguration containerConfiguration)
        {
            return new QiniuBlobProviderConfiguration(containerConfiguration);
        }

        public static BlobContainerConfiguration UseQiniu(
            this BlobContainerConfiguration containerConfiguration,
            Action<QiniuBlobProviderConfiguration> qiniuConfigureAction)
        {
            containerConfiguration.ProviderType = typeof(QiniuBlobProvider);

            qiniuConfigureAction(new QiniuBlobProviderConfiguration(containerConfiguration));

            return containerConfiguration;
        }
    }
}
