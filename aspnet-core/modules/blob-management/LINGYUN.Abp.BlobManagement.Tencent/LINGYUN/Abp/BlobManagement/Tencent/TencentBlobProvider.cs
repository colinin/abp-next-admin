using COSXML;
using COSXML.Model.Bucket;
using COSXML.Model.Object;
using COSXML.Model.Tag;
using LINGYUN.Abp.BlobStoring.Tencent;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.BlobStoring;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Abp.BlobManagement.Tencent;

public class TencentBlobProvider : IBlobProvider
{
    public const string ProviderName = "TencentCloud";
    public string Name => ProviderName;

    protected IConfiguration Configuration { get; }
    protected ICurrentTenant CurrentTenant { get; }
    protected ICosClientFactory CosClientFactory { get; }
    protected IHttpClientFactory HttpClientFactory { get; }
    protected TencentBlobNamingNormalizer BlobNamingNormalizer { get; }
    protected IBlobContainerConfigurationProvider ConfigurationProvider { get; }

    public TencentBlobProvider(
        IConfiguration configuration,
        ICurrentTenant currentTenant,
        ICosClientFactory cosClientFactory,
        IHttpClientFactory httpClientFactory,
        TencentBlobNamingNormalizer blobNamingNormalizer,
        IBlobContainerConfigurationProvider configurationProvider)
    {
        Configuration = configuration;
        CurrentTenant = currentTenant;
        CosClientFactory = cosClientFactory;
        HttpClientFactory = httpClientFactory;
        BlobNamingNormalizer = blobNamingNormalizer;
        ConfigurationProvider = configurationProvider;
    }

    public async virtual Task CreateContainerAsync(
        string name,
        CancellationToken cancellationToken = default)
    {
        var client = await CreateClientAsync();
        var bucket = NormalizeContainerName(name);

        CreateBucketIfNotExists(client, bucket);
    }

    public async virtual Task DeleteContainerAsync(
        string name,
        CancellationToken cancellationToken = default)
    {
        var client = await CreateClientAsync();
        var bucket = NormalizeContainerName(name);

        if (!BucketExists(client, bucket))
        {
            return;
        }

        client.DeleteBucket(new DeleteBucketRequest(bucket));
    }

    public async virtual Task DeleteBlobAsync(
        string containerName,
        string blobName,
        CancellationToken cancellationToken = default)
    {
        var client = await CreateClientAsync();
        var bucket = NormalizeContainerName(containerName);
        var objectName = CalculateBlobName(blobName);

        if (ObjectExists(client, bucket, objectName))
        {
            client.DeleteObject(new DeleteObjectRequest(bucket, objectName));
        }
    }

    public virtual Task DeleteFolderAsync(
        string containerName,
        string blobName,
        CancellationToken cancellationToken = default)
    {
        // 腾讯云Oss没有目录的概念,新建对象时可以模拟目录
        return Task.CompletedTask;
    }

    public async virtual Task<Stream?> DownloadBlobAsync(
        string containerName,
        string blobName,
        CancellationToken cancellationToken = default)
    {
        var client = await CreateClientAsync();
        var bucket = NormalizeContainerName(containerName);
        var objectName = CalculateBlobName(blobName);

        if (!ObjectExists(client, bucket, objectName))
        {
            return null;
        }

        var configuration = await GetBlobConfiguration();
        var blobProviderConfig = Configuration.GetSection($"BlobManagement:{ProviderName}");

        // See: https://cloud.tencent.com/document/product/436/47238
        var preSignatureStruct = new PreSignatureStruct
        {
            appid = configuration.AppId,//"1250000000"; //腾讯云账号 APPID
            region = configuration.Region,//"COS_REGION"; //存储桶地域
            bucket = bucket,//"examplebucket-1250000000"; //存储桶
            key = objectName, //对象键
            httpMethod = "GET", //HTTP 请求方法
            isHttps = true, //生成 HTTPS 请求 URL
            signDurationSecond = blobProviderConfig.GetValue("SignDurationSecond", 600), //请求签名时间
            headers = null, //签名中需要校验的 header
            queryParameters = null //签名中需要校验的 URL 中请求参数
        };

        var downloadUrl = client.GenerateSignURL(preSignatureStruct);

        var httpClient = HttpClientFactory.CreateTencentHttpClient();

        return await httpClient.GetStreamAsync(downloadUrl, cancellationToken);
    }

    public virtual Task CreateFolderAsync(
        string containerName, 
        string blobName,
        CancellationToken cancellationToken = default)
    {
        // 腾讯云Oss没有目录的概念,新建对象时可以模拟目录
        // https://cloud.tencent.com/document/product/436/13324
        return Task.CompletedTask;
    }

    public async virtual Task UploadBlobAsync(
        string containerName, 
        string blobName, 
        Stream content, 
        CancellationToken cancellationToken = default)
    {
        var client = await CreateClientAsync();
        var bucket = NormalizeContainerName(containerName);
        var objectName = CalculateBlobName(blobName);

        CreateBucketIfNotExists(client, bucket);

        client.PutObject(new PutObjectRequest(bucket, objectName, content));
    }

    protected async virtual Task<CosXml> CreateClientAsync()
    {
        return await CosClientFactory.CreateAsync<BlobManagementContainer>();
    }

    protected virtual void CreateBucketIfNotExists(CosXml cos, string bucket)
    {
        if (!cos.DoesBucketExist(new DoesBucketExistRequest(bucket)))
        {
            cos.PutBucket(new PutBucketRequest(bucket));
        }
    }

    protected virtual bool BucketExists(CosXml cos, string bucket)
    {
        var request = new DoesBucketExistRequest(bucket);
        return cos.DoesBucketExist(request);
    }

    protected virtual bool ObjectExists(CosXml cos, string bucket, string objectName)
    {
        var request = new DoesObjectExistRequest(bucket, objectName);
        return cos.DoesObjectExist(request);
    }

    protected async virtual Task<TencentBlobProviderConfiguration> GetBlobConfiguration()
    {
        return await CosClientFactory.GetConfigurationAsync<BlobManagementContainer>();
    }

    protected virtual string GetPrefixPath()
    {
        return CurrentTenant.Id == null
           ? $"host/"
           : $"tenants/{CurrentTenant.Id.Value.ToString("D")}/";
    }

    protected virtual string NormalizeContainerName(string containerName)
    {
        if (!containerName.StartsWith("abp-"))
        {
            containerName = "abp-" + containerName;
        }
        return BlobNamingNormalizer.NormalizeContainerName(containerName);
    }

    protected virtual string CalculateBlobName(string blobName)
    {
        var blobPath = GetPrefixPath();

        if (!blobName.IsNullOrWhiteSpace())
        {
            blobPath += blobName.RemovePreFix("/");
        }

        return BlobNamingNormalizer.NormalizeBlobName(blobPath);
    }
}
