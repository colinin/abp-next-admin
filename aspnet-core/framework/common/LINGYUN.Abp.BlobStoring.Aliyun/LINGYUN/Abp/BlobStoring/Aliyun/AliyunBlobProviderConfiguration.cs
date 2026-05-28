using AlibabaCloud.OSS.V2.Models;
using System.Collections.Generic;
using Volo.Abp.BlobStoring;

namespace LINGYUN.Abp.BlobStoring.Aliyun;

public class AliyunBlobProviderConfiguration
{
    /// <summary>
    /// 命名空间
    /// </summary>
    public string BucketName
    {
        get => _containerConfiguration.GetConfiguration<string>(AliyunBlobProviderConfigurationNames.BucketName);
        set => _containerConfiguration.SetConfiguration(AliyunBlobProviderConfigurationNames.BucketName, value);
    }
    /// <summary>
    /// 跳过服务器证书验证
    /// </summary>
    public bool InsecureSkipVerify {
        get => _containerConfiguration.GetConfigurationOrDefault(AliyunBlobProviderConfigurationNames.InsecureSkipVerify, false);
        set => _containerConfiguration.SetConfiguration(AliyunBlobProviderConfigurationNames.InsecureSkipVerify, value);
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
    /// 创建命名空间时的Acl
    /// </summary>
    public BucketAclType? CreateBucketAcl {
        get => _containerConfiguration.GetConfigurationOrDefault<BucketAclType?>(AliyunBlobProviderConfigurationNames.CreateBucketAcl);
        set => _containerConfiguration.SetConfiguration(AliyunBlobProviderConfigurationNames.CreateBucketAcl, value);
    }
    /// <summary>
    /// 创建命名空间时防盗链列表
    /// </summary>
    public List<string> CreateBucketReferer
    {
        get => _containerConfiguration.GetConfigurationOrDefault(AliyunBlobProviderConfigurationNames.CreateBucketReferer, new List<string>());
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

    /// <summary>
    /// Default value: 7 * 24 * 3600.
    /// </summary>
    public int PresignedGetExpirySeconds {
        get => _containerConfiguration.GetConfigurationOrDefault(AliyunBlobProviderConfigurationNames.PresignedGetExpirySeconds, _defaultExpirySeconds);
        set => _containerConfiguration.SetConfiguration(AliyunBlobProviderConfigurationNames.PresignedGetExpirySeconds, value);
    }

    private int _defaultExpirySeconds = 7 * 24 * 3600;

    private readonly BlobContainerConfiguration _containerConfiguration;

    public AliyunBlobProviderConfiguration(BlobContainerConfiguration containerConfiguration)
    {
        _containerConfiguration = containerConfiguration;
    }
}
