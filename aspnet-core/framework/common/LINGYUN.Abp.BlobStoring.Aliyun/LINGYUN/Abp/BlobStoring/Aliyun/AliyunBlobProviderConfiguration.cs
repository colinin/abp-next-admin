using AlibabaCloud.OSS.V2.Models;
using System.Collections.Generic;
using Volo.Abp.BlobStoring;

namespace LINGYUN.Abp.BlobStoring.Aliyun;

public class AliyunBlobProviderConfiguration
{
    /// <summary>
    /// 默认签名版本
    /// </summary>
    public const string DefaultSignatureVersion = "v1"; //TODO: 阿里云 OSS SDKv2 SignerV4.ResourcePath存在缺陷, 临时使用v1签名
    /// <summary>
    /// 默认跳过服务器证书验证
    /// </summary>
    public const bool DefaultInsecureSkipVerify = false;
    /// <summary>
    /// 默认命名空间不存在是否创建
    /// </summary>
    public const bool DefaultCreateBucketIfNotExists = false;
    /// <summary>
    /// 默认预签名链接过期时间
    /// </summary>
    public const int DefaultPresignedGetExpirySeconds = 600;
    /// <summary>
    /// 命名空间
    /// </summary>
    public string BucketName
    {
        get => _containerConfiguration.GetConfiguration<string>(AliyunBlobProviderConfigurationNames.BucketName);
        set => _containerConfiguration.SetConfiguration(AliyunBlobProviderConfigurationNames.BucketName, value);
    }
    /// <summary>
    /// 签名版本（可选项：v1、v4）
    /// 默认: v1
    /// </summary>
    public string SignatureVersion {
        get => _containerConfiguration.GetConfigurationOrDefault(AliyunBlobProviderConfigurationNames.SignatureVersion, DefaultSignatureVersion);
        set => _containerConfiguration.SetConfiguration(AliyunBlobProviderConfigurationNames.SignatureVersion, value);
    }
    /// <summary>
    /// 跳过服务器证书验证
    /// </summary>
    public bool InsecureSkipVerify {
        get => _containerConfiguration.GetConfigurationOrDefault(AliyunBlobProviderConfigurationNames.InsecureSkipVerify, DefaultInsecureSkipVerify);
        set => _containerConfiguration.SetConfiguration(AliyunBlobProviderConfigurationNames.InsecureSkipVerify, value);
    }
    /// <summary>
    /// 命名空间不存在是否创建
    /// </summary>
    public bool CreateBucketIfNotExists
    {
        get => _containerConfiguration.GetConfigurationOrDefault(AliyunBlobProviderConfigurationNames.CreateBucketIfNotExists, DefaultCreateBucketIfNotExists);
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


    public int PresignedGetExpirySeconds {
        get => _containerConfiguration.GetConfigurationOrDefault(AliyunBlobProviderConfigurationNames.PresignedGetExpirySeconds, DefaultPresignedGetExpirySeconds);
        set => _containerConfiguration.SetConfiguration(AliyunBlobProviderConfigurationNames.PresignedGetExpirySeconds, value);
    }

    public string Endpoint {
        get => _containerConfiguration.GetConfigurationOrDefault<string>(AliyunBlobProviderConfigurationNames.Endpoint, null);
        set => _containerConfiguration.SetConfiguration(AliyunBlobProviderConfigurationNames.Endpoint, value);
    }
    public bool? UsePathStyle {
        get => _containerConfiguration.GetConfigurationOrDefault<bool?>(AliyunBlobProviderConfigurationNames.UsePathStyle, null);
        set => _containerConfiguration.SetConfiguration(AliyunBlobProviderConfigurationNames.UsePathStyle, value);
    }
    public bool? UseCName {
        get => _containerConfiguration.GetConfigurationOrDefault<bool?>(AliyunBlobProviderConfigurationNames.UseCName, null);
        set => _containerConfiguration.SetConfiguration(AliyunBlobProviderConfigurationNames.UseCName, value);
    }
    public bool? UseDualStackEndpoint {
        get => _containerConfiguration.GetConfigurationOrDefault<bool?>(AliyunBlobProviderConfigurationNames.UseDualStackEndpoint, null);
        set => _containerConfiguration.SetConfiguration(AliyunBlobProviderConfigurationNames.UseDualStackEndpoint, value);
    }
    public bool? UseAccelerateEndpoint {
        get => _containerConfiguration.GetConfigurationOrDefault<bool?>(AliyunBlobProviderConfigurationNames.UseAccelerateEndpoint, null);
        set => _containerConfiguration.SetConfiguration(AliyunBlobProviderConfigurationNames.UseAccelerateEndpoint, value);
    }
    public bool? UseInternalEndpoint {
        get => _containerConfiguration.GetConfigurationOrDefault<bool?>(AliyunBlobProviderConfigurationNames.UseInternalEndpoint, null);
        set => _containerConfiguration.SetConfiguration(AliyunBlobProviderConfigurationNames.UseInternalEndpoint, value);
    }
    public bool? DisableUploadCrc64Check {
        get => _containerConfiguration.GetConfigurationOrDefault<bool?>(AliyunBlobProviderConfigurationNames.DisableUploadCrc64Check, null);
        set => _containerConfiguration.SetConfiguration(AliyunBlobProviderConfigurationNames.DisableUploadCrc64Check, value);
    }
    public bool? DisableDownloadCrc64Check {
        get => _containerConfiguration.GetConfigurationOrDefault<bool?>(AliyunBlobProviderConfigurationNames.DisableDownloadCrc64Check, null);
        set => _containerConfiguration.SetConfiguration(AliyunBlobProviderConfigurationNames.DisableDownloadCrc64Check, value);
    }

    private readonly BlobContainerConfiguration _containerConfiguration;

    public AliyunBlobProviderConfiguration(BlobContainerConfiguration containerConfiguration)
    {
        _containerConfiguration = containerConfiguration;
    }
}
