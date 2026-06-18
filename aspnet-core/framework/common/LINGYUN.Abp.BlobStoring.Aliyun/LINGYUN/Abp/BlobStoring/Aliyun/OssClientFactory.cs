using AlibabaCloud.OSS.V2;
using AlibabaCloud.OSS.V2.Credentials;
using AlibabaCloud.OSS.V2.Transport;
using LINGYUN.Abp.Aliyun;
using System;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Settings;

namespace LINGYUN.Abp.BlobStoring.Aliyun;

public class OssClientFactory : AliyunClientFactory<Client, AliyunBlobProviderConfiguration>, IOssClientFactory, ITransientDependency
{
    public OssClientFactory(
        ISettingProvider settingProvider,
        IDistributedCache<AliyunBasicSessionCredentialsCacheItem> cache) 
        : base(settingProvider, cache)
    {
    }

    /// <summary>
    /// 普通方式构建Oss客户端
    /// </summary>
    /// <param name="regionId"></param>
    /// <param name="accessKeyId"></param>
    /// <param name="accessKeySecret"></param>
    /// <returns></returns>
    protected override Client GetClient(string regionId, string accessKeyId, string accessKeySecret)
    {

        return new Client(
            new Configuration
            {
                Region = regionId,
                SignatureVersion = AliyunBlobProviderConfiguration.DefaultSignatureVersion,
                InsecureSkipVerify = AliyunBlobProviderConfiguration.DefaultInsecureSkipVerify,
                CredentialsProvider = new StaticCredentialsProvider(accessKeyId, accessKeySecret),
                HttpTransport = HttpTransport.Shared,
            });
        //return new OssClient(
        //    regionId,
        //    accessKeyId,
        //    accessKeySecret);
    }

    /// <summary>
    /// 普通方式构建Oss客户端
    /// </summary>
    /// <param name="configuration"></param>
    /// <param name="regionId"></param>
    /// <param name="accessKeyId"></param>
    /// <param name="accessKeySecret"></param>
    /// <returns></returns>
    protected override Client GetClient(AliyunBlobProviderConfiguration configuration, string regionId, string accessKeyId, string accessKeySecret)
    {
        return new Client(
            new Configuration
            {
                Region = regionId,
                Endpoint = configuration.Endpoint,
                SignatureVersion = configuration.SignatureVersion,
                InsecureSkipVerify = configuration.InsecureSkipVerify,
                UseCName = configuration.UseCName,
                UsePathStyle = configuration.UsePathStyle,
                UseAccelerateEndpoint = configuration.UseAccelerateEndpoint,
                UseDualStackEndpoint = configuration.UseDualStackEndpoint,
                UseInternalEndpoint = configuration.UseInternalEndpoint,
                DisableUploadCrc64Check = configuration.DisableUploadCrc64Check,
                DisableDownloadCrc64Check = configuration.DisableDownloadCrc64Check,
                CredentialsProvider = new StaticCredentialsProvider(accessKeyId, accessKeySecret),
                HttpTransport = HttpTransport.Shared,
            });
    }

    /// <summary>
    /// 通过用户安全令牌构建Oss客户端
    /// </summary>
    /// <param name="regionId"></param>
    /// <param name="accessKeyId"></param>
    /// <param name="accessKeySecret"></param>
    /// <param name="securityToken"></param>
    /// <returns></returns>
    protected override Client GetSecurityTokenClient(
        string regionId, 
        string accessKeyId, 
        string accessKeySecret, 
        string securityToken,
        DateTime? expiration = null)
    {
        return new Client(
            new Configuration
            {
                Region = regionId,
                SignatureVersion = AliyunBlobProviderConfiguration.DefaultSignatureVersion,
                InsecureSkipVerify = AliyunBlobProviderConfiguration.DefaultInsecureSkipVerify,
                CredentialsProvider = new StaticCredentialsProvider(accessKeyId, accessKeySecret, securityToken),
                HttpTransport = HttpTransport.Shared,
            });
    }
    /// <summary>
    /// 通过用户安全令牌构建Oss客户端
    /// </summary>
    /// <param name="configuration"></param>
    /// <param name="regionId"></param>
    /// <param name="accessKeyId"></param>
    /// <param name="accessKeySecret"></param>
    /// <param name="securityToken"></param>
    /// <returns></returns>
    protected override Client GetSecurityTokenClient(
        AliyunBlobProviderConfiguration configuration, 
        string regionId, 
        string accessKeyId, 
        string accessKeySecret, 
        string securityToken,
        DateTime? expiration = null)
    {
        return new Client(
            new Configuration
            {
                Region = regionId,
                Endpoint = configuration.Endpoint,
                SignatureVersion = configuration.SignatureVersion,
                InsecureSkipVerify = configuration.InsecureSkipVerify,
                UseCName = configuration.UseCName,
                UsePathStyle = configuration.UsePathStyle,
                UseAccelerateEndpoint = configuration.UseAccelerateEndpoint,
                UseDualStackEndpoint = configuration.UseDualStackEndpoint,
                UseInternalEndpoint = configuration.UseInternalEndpoint,
                DisableUploadCrc64Check = configuration.DisableUploadCrc64Check,
                DisableDownloadCrc64Check = configuration.DisableDownloadCrc64Check,
                CredentialsProvider = new StaticCredentialsProvider(accessKeyId, accessKeySecret, securityToken),
                HttpTransport = HttpTransport.Shared,
            });
    }
}
