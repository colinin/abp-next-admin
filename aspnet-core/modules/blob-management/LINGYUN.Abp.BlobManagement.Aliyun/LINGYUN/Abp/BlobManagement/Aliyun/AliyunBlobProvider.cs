using AlibabaCloud.OSS.V2;
using AlibabaCloud.OSS.V2.Models;
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
        using var client = await CreateClientAsync();
        var bucket = NormalizeContainerName(name);
        var configuration = GetBlobConfiguration();

        await CreateBucketIfNotExists(client, bucket, configuration.CreateBucketAcl, cancellationToken);
    }

    public async virtual Task DeleteContainerAsync(
        string name,
        CancellationToken cancellationToken = default)
    {
        using var ossClient = await CreateClientAsync();
        var bucket = NormalizeContainerName(name);

        if (!await BucketExists(ossClient, bucket, cancellationToken))
        {
            return;
        }

        await ossClient.DeleteBucketAsync(
            new DeleteBucketRequest
            {
                Bucket = bucket,
            },
            cancellationToken: cancellationToken);
    }

    public async virtual Task DeleteBlobAsync(
        string containerName,
        string blobName,
        CancellationToken cancellationToken = default)
    {
        using var ossClient = await CreateClientAsync();
        var bucket = NormalizeContainerName(containerName);
        var objectName = CalculateBlobName(blobName);

        if (await ObjectExists(ossClient, bucket, objectName, cancellationToken))
        {
            await ossClient.DeleteObjectAsync(
                new DeleteObjectRequest
                {
                    Bucket = bucket,
                    Key = objectName,
                },
                cancellationToken: cancellationToken);
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
        //using var ossClient = await CreateClientAsync();
        //var bucket = NormalizeContainerName(containerName);
        //var objectName = CalculateBlobName(blobName);

        //if (!await ObjectExists(ossClient, bucket, objectName, cancellationToken))
        //{
        //    return null;
        //}

        //var result = await ossClient.GetObjectAsync(
        //    new GetObjectRequest
        //    {
        //        Bucket = bucket,
        //        Key = objectName,
        //    });

        //return result.Body;

        var configuration = GetBlobConfiguration();

        var downloadUrl = await InternalGeneratePresignedUrlAsync(
            containerName,
            blobName,
            TimeSpan.FromSeconds(configuration.PresignedGetExpirySeconds),
            cancellationToken: cancellationToken);
        if (downloadUrl.IsNullOrWhiteSpace())
        {
            return null;
        }

        var httpClient = HttpClientFactory.CreateAliyunHttpClient();

        return await httpClient.GetStreamAsync(downloadUrl, cancellationToken);
    }

    public virtual Task<string?> GeneratePresignedUrlAsync(
        string containerName,
        string blobName,
        TimeSpan expiration,
        bool isAttachmentContent = true,
        CancellationToken cancellationToken = default)
    {
        // TODO: 阿里云SDK2.0不支持Bucket跨域配置, 不启用阿里云的预览方式
        //return InternalGeneratePresignedUrlAsync(
        //    containerName,
        //    blobName,
        //    expiration,
        //    isAttachmentContent,
        //    cancellationToken);
        return Task.FromResult<string?>(null);
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
        using var ossClient = await CreateClientAsync();
        var bucket = NormalizeContainerName(containerName);
        var objectName = CalculateBlobName(blobName);
        var configuration = GetBlobConfiguration();

        await CreateBucketIfNotExists(ossClient, bucket, configuration.CreateBucketAcl, cancellationToken);

        await ossClient.PutObjectAsync(
            new PutObjectRequest
            {
                Bucket = bucket,
                Key = objectName,
                Body = content,
                ContentType = contentType,
            },
            cancellationToken: cancellationToken);
    }

    protected virtual AliyunBlobProviderConfiguration GetBlobConfiguration()
    {
        var configuration = ConfigurationProvider.Get<BlobManagementContainer>();
        return configuration.GetAliyunConfiguration();
    }

    protected async virtual Task<Client> CreateClientAsync()
    {
        var configuration = GetBlobConfiguration();
        return await OssClientFactory.CreateAsync(configuration);
    }

    protected async virtual Task CreateBucketIfNotExists(
        Client ossClient, 
        string bucket, 
        BucketAclType? bucketAcl = null,
        CancellationToken cancellationToken = default)
    {
        if (!await BucketExists(ossClient, bucket, cancellationToken))
        {
            await ossClient.PutBucketAsync(
                new PutBucketRequest
                {
                    Bucket = bucket,
                    Acl = bucketAcl?.GetString(),
                },
                cancellationToken: cancellationToken);
        }
    }

    protected async virtual Task<string?> InternalGeneratePresignedUrlAsync(
        string containerName,
        string blobName,
        TimeSpan expiration,
        bool isAttachmentContent = true,
        CancellationToken cancellationToken = default)
    {
        using var ossClient = await CreateClientAsync();
        var bucket = NormalizeContainerName(containerName);
        var objectName = CalculateBlobName(blobName);

        if (!await ObjectExists(ossClient, bucket, objectName, cancellationToken))
        {
            return null;
        }

        var fileName = Path.GetFileName(blobName);
        var type = isAttachmentContent ? "attachment" : "inline";
        var disposition = $"{type}; filename=\"{Uri.EscapeDataString(fileName)}\"; " +
                             $"filename*=UTF-8''{Uri.EscapeDataString(fileName)}";

        var presignResult = ossClient.Presign(
            new GetObjectRequest
            {
                Bucket = bucket,
                Key = objectName,
                ResponseContentDisposition = disposition,
            },
            Clock.Now.Add(expiration));

        return presignResult.Url;
    }

    protected async virtual Task<bool> BucketExists(
        Client ossClient, 
        string bucket,
        CancellationToken cancellationToken = default)
    {
        return await ossClient.IsBucketExistAsync(bucket, cancellationToken);
    }

    protected async virtual Task<bool> ObjectExists(
        Client ossClient, 
        string bucket, 
        string objectName,
        CancellationToken cancellationToken = default)
    {
        return await ossClient.IsObjectExistAsync(bucket, objectName, cancellationToken: cancellationToken);
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
