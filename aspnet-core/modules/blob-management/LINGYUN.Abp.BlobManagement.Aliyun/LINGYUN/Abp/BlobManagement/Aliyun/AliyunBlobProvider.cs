using Aliyun.OSS;
using LINGYUN.Abp.BlobStoring.Aliyun;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.BlobStoring;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Timing;

namespace LINGYUN.Abp.BlobManagement.Aliyun;

public class AliyunBlobProvider : IBlobProvider
{
    public const string ProviderName = "Aliyun";
    public string Name => ProviderName;

    protected IClock Clock { get; }
    protected IConfiguration Configuration { get; }
    protected ICurrentTenant CurrentTenant { get; }
    protected IOssClientFactory OssClientFactory { get; }
    protected IHttpClientFactory HttpClientFactory { get; }
    protected AliyunBlobNamingNormalizer BlobNamingNormalizer { get; }
    protected IBlobContainerConfigurationProvider ConfigurationProvider { get; }

    public AliyunBlobProvider(
        IClock clock,
        IConfiguration configuration,
        ICurrentTenant currentTenant,
        IOssClientFactory ossClientFactory,
        IHttpClientFactory httpClientFactory,
        AliyunBlobNamingNormalizer blobNamingNormalizer,
        IBlobContainerConfigurationProvider configurationProvider)
    {
        Clock = clock;
        Configuration = configuration;
        CurrentTenant = currentTenant;
        OssClientFactory = ossClientFactory;
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

        client.DeleteBucket(bucket);
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
            client.DeleteObject(bucket, objectName);
        }
    }

    public virtual Task DeleteFolderAsync(
        string containerName,
        string blobName,
        CancellationToken cancellationToken = default)
    {
        // 阿里云Oss没有目录的概念,新建对象时可以模拟目录
        return Task.CompletedTask;
    }

    public async virtual Task<Stream?> DownloadBlobAsync(
        string containerName,
        string blobName,
        CancellationToken cancellationToken = default)
    {
        var configuration = GetBlobConfiguration();

        var downloadUrl = await GenerateDownloadUrlAsync(
            containerName,
            blobName,
            TimeSpan.FromSeconds(configuration.PresignedGetExpirySeconds),
            cancellationToken);
        if (downloadUrl.IsNullOrWhiteSpace())
        {
            return null;
        }

        var httpClient = HttpClientFactory.CreateAliyunHttpClient();

        return await httpClient.GetStreamAsync(downloadUrl, cancellationToken);
    }

    public async virtual Task<string?> GenerateDownloadUrlAsync(
        string containerName,
        string blobName,
        TimeSpan expiration,
        CancellationToken cancellationToken = default)
    {
        var client = await CreateClientAsync();
        var bucket = NormalizeContainerName(containerName);
        var objectName = CalculateBlobName(blobName);

        if (!ObjectExists(client, bucket, objectName))
        {
            return null;
        }

        return client.GeneratePresignedUri(bucket, objectName, Clock.Now.Add(expiration)).ToString();
    }

    public virtual Task CreateFolderAsync(
        string containerName, 
        string blobName,
        CancellationToken cancellationToken = default)
    {
        // 阿里云Oss没有目录的概念,新建对象时可以模拟目录
        // https://cloud.tencent.com/document/product/436/13324
        return Task.CompletedTask;
    }

    public async virtual Task UploadBlobAsync(
        string containerName, 
        string blobName, 
        Stream content,
        string? contentType = null,
        CancellationToken cancellationToken = default)
    {
        var client = await CreateClientAsync();
        var bucket = NormalizeContainerName(containerName);
        var objectName = CalculateBlobName(blobName);

        CreateBucketIfNotExists(client, bucket);

        var objectMetadata = new ObjectMetadata();
        if (!contentType.IsNullOrWhiteSpace())
        {
            objectMetadata.ContentType = contentType;
        }

        client.PutObject(bucket, objectName, content, objectMetadata);
    }

    protected virtual AliyunBlobProviderConfiguration GetBlobConfiguration()
    {
        var configuration = ConfigurationProvider.Get<BlobManagementContainer>();
        var blobConfiguration = configuration.GetAliyunConfiguration();
        return blobConfiguration;
    }

    protected async virtual Task<IOss> CreateClientAsync()
    {
        return await OssClientFactory.CreateAsync();
    }

    protected virtual void CreateBucketIfNotExists(IOss oss, string bucket)
    {
        if (!BucketExists(oss, bucket))
        {
            oss.CreateBucket(bucket);
        }
    }

    protected virtual bool BucketExists(IOss oss, string bucket)
    {
        return oss.DoesBucketExist(bucket);
    }

    protected virtual bool ObjectExists(IOss oss, string bucket, string objectName)
    {
        return oss.DoesObjectExist(bucket, objectName);
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
