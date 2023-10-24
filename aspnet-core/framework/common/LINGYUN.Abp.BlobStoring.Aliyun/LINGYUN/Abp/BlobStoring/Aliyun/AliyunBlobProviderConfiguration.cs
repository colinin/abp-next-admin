using System.Collections.Generic;
using Volo.Abp;
using Volo.Abp.BlobStoring;

namespace LINGYUN.Abp.BlobStoring.Aliyun
{
    public class AliyunBlobProviderConfiguration
    {
        /// <summary>
        /// 数据中心
        /// </summary>
        /// <remarks>
        /// 详见 https://help.aliyun.com/document_detail/31837.html?spm=a2c4g.11186623.2.14.417cd47eLc9LHc#concept-zt4-cvy-5db
        /// </remarks>
        public string Endpoint
        {
            get => _containerConfiguration.GetConfiguration<string>(AliyunBlobProviderConfigurationNames.Endpoint);
            set => _containerConfiguration.SetConfiguration(AliyunBlobProviderConfigurationNames.Endpoint, Check.NotNullOrWhiteSpace(value, nameof(value)));
        }
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
