using Volo.Abp;
using Volo.Abp.BlobStoring;

namespace LINGYUN.Abp.BlobStoring.OssManagement
{
    public class OssManagementBlobProviderConfiguration
    {
        public string Bucket
        {
            get => _containerConfiguration.GetConfiguration<string>(OssManagementBlobProviderConfigurationNames.Bucket);
            set => _containerConfiguration.SetConfiguration(OssManagementBlobProviderConfigurationNames.Bucket, Check.NotNullOrWhiteSpace(value, nameof(value)));
        }

        private readonly BlobContainerConfiguration _containerConfiguration;

        public OssManagementBlobProviderConfiguration(BlobContainerConfiguration containerConfiguration)
        {
            _containerConfiguration = containerConfiguration;
        }
    }
}
