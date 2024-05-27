using System.Collections.Generic;
using Volo.Abp;
using Volo.Abp.BlobStoring;

namespace LINGYUN.Abp.BlobStoring.Aliyun
{
    public class AliyunBlobProviderConfiguration
    {
        /// <summary>
        /// 命名空间
        /// </summary>
        public string BucketName
        {
            get => _containerConfiguration.GetConfiguration<string>(AliyunBlobProviderConfigurationNames.BucketName);
            set => _containerConfiguration.SetConfiguration(AliyunBlobProviderConfigurationNames.BucketName, Check.NotNullOrWhiteSpace(value, nameof(value)));
        }
        /// <summary>
        /// 命名空间不存在是否创建
        /// </summary>
        public bool CreateBucketIfNotExists
        {
            get => _containerConfiguration.GetConfigurationOrDefault(AliyunBlobProviderConfigurationNames.CreateBucketIfNotExists, false);
            set => _containerConfiguration.SetConfiguration(AliyunBlobProviderConfigurationNames.CreateBucketIfNotExists, value);
        }
        /// <summary>
        /// 创建命名空间时防盗链列表
        /// </summary>
        public List<string> CreateBucketReferer
        {
            get => _containerConfiguration.GetConfiguration<List<string>>(AliyunBlobProviderConfigurationNames.CreateBucketReferer);
            set
            {
                if (value == null)
                {
                    _containerConfiguration.SetConfiguration(AliyunBlobProviderConfigurationNames.CreateBucketReferer, new List<string>());
                }
                else
                {
                    _containerConfiguration.SetConfiguration(AliyunBlobProviderConfigurationNames.CreateBucketReferer, value);
                }
            }
        }

        private readonly BlobContainerConfiguration _containerConfiguration;

        public AliyunBlobProviderConfiguration(BlobContainerConfiguration containerConfiguration)
        {
            _containerConfiguration = containerConfiguration;
        }
    }
}
