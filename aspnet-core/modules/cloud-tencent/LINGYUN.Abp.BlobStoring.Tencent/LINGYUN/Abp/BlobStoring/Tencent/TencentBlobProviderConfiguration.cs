using System.Collections.Generic;
using Volo.Abp;
using Volo.Abp.BlobStoring;

namespace LINGYUN.Abp.BlobStoring.Tencent
{
    public class TencentBlobProviderConfiguration
    {
        /// <summary>
        /// AppId
        /// </summary>
        public string AppId {
            get => _containerConfiguration.GetConfiguration<string>(TencentBlobProviderConfigurationNames.AppId);
            set => _containerConfiguration.SetConfiguration(TencentBlobProviderConfigurationNames.AppId, Check.NotNullOrWhiteSpace(value, nameof(value)));
        }
        /// <summary>
        /// 区域
        /// </summary>
        public string Region {
            get => _containerConfiguration.GetConfiguration<string>(TencentBlobProviderConfigurationNames.Region);
            set => _containerConfiguration.SetConfiguration(TencentBlobProviderConfigurationNames.Region, value);
        }
        /// <summary>
        /// 命名空间
        /// </summary>
        public string BucketName
        {
            get => _containerConfiguration.GetConfiguration<string>(TencentBlobProviderConfigurationNames.BucketName);
            set => _containerConfiguration.SetConfiguration(TencentBlobProviderConfigurationNames.BucketName, Check.NotNullOrWhiteSpace(value, nameof(value)));
        }
        /// <summary>
        /// 命名空间不存在是否创建
        /// </summary>
        public bool CreateBucketIfNotExists
        {
            get => _containerConfiguration.GetConfigurationOrDefault(TencentBlobProviderConfigurationNames.CreateBucketIfNotExists, false);
            set => _containerConfiguration.SetConfiguration(TencentBlobProviderConfigurationNames.CreateBucketIfNotExists, value);
        }
        /// <summary>
        /// 创建命名空间时防盗链列表
        /// </summary>
        public List<string> CreateBucketReferer {
            get => _containerConfiguration.GetConfiguration<List<string>>(TencentBlobProviderConfigurationNames.CreateBucketReferer);
            set {
                if (value == null)
                {
                    _containerConfiguration.SetConfiguration(TencentBlobProviderConfigurationNames.CreateBucketReferer, new List<string>());
                }
                else
                {
                    _containerConfiguration.SetConfiguration(TencentBlobProviderConfigurationNames.CreateBucketReferer, value);
                }
            }
        }

        private readonly BlobContainerConfiguration _containerConfiguration;

        public TencentBlobProviderConfiguration(BlobContainerConfiguration containerConfiguration)
        {
            _containerConfiguration = containerConfiguration;
        }
    }
}
