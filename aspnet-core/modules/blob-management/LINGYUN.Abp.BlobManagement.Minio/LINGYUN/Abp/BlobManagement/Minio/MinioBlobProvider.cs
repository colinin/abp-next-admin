using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Minio;
using Minio.DataModel.Args;
using Minio.Exceptions;
using System;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.BlobStoring;
using Volo.Abp.BlobStoring.Minio;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Abp.BlobManagement.Minio;

public class MinioBlobProvider : IBlobProvider
{
    public const string ProviderName = "Minio";
    public string Name => ProviderName;

    protected ICurrentTenant CurrentTenant { get; }
    protected IHttpClientFactory HttpClientFactory { get; }
    protected MinioBlobNamingNormalizer BlobNamingNormalizer { get; }
    protected IBlobContainerConfigurationProvider ConfigurationProvider { get; }

    public MinioBlobProvider(
        ICurrentTenant currentTenant,
        IHttpClientFactory httpClientFactory,
        MinioBlobNamingNormalizer blobNamingNormalizer,
        IBlobContainerConfigurationProvider configurationProvider)
    {
        CurrentTenant = currentTenant;
        HttpClientFactory = httpClientFactory;
        BlobNamingNormalizer = blobNamingNormalizer;
        ConfigurationProvider = configurationProvider;
    }

    public async virtual Task CreateContainerAsync(
        string name,
        CancellationToken cancellationToken = default)
    {
        var client = GetMinioClient();
        var bucket = NormalizeContainerName(name);

        if (await BucketExists(client, bucket))
        {
            return;
        }

        await client.MakeBucketAsync(new MakeBucketArgs().WithBucket(bucket), cancellationToken);
    }

    public async virtual Task DeleteContainerAsync(
        string name,
        CancellationToken cancellationToken = default)
    {
        var client = GetMinioClient();
        var bucket = NormalizeContainerName(name);

        if (!await BucketExists(client, bucket))
        {
            return;
        }

        var deleteBucketArgs = new RemoveBucketArgs()
                .WithBucket(bucket);

        await client.RemoveBucketAsync(deleteBucketArgs, cancellationToken);
    }

    public async virtual Task DeleteBlobAsync(
        string containerName,
        string blobName,
        CancellationToken cancellationToken = default)
    {
        var client = GetMinioClient();
        var bucket = NormalizeContainerName(containerName);
        var objectName = CalculateBlobName(blobName);

        if (await ObjectExists(client, bucket, objectName))
        {
            var removeObjectArgs = new RemoveObjectArgs()
                .WithBucket(bucket)
                .WithObject(objectName);

            await client.RemoveObjectAsync(removeObjectArgs, cancellationToken);
        }
    }

    public virtual Task DeleteFolderAsync(
        string containerName,
        string blobName,
        CancellationToken cancellationToken = default)
    {
        // Minio没有目录的概念,新建对象时可以模拟目录
        return Task.CompletedTask;
    }

    public async virtual Task<Stream?> DownloadBlobAsync(
        string containerName,
        string blobName,
        CancellationToken cancellationToken = default)
    {
        var client = GetMinioClient();
        var bucket = NormalizeContainerName(containerName);
        var objectName = CalculateBlobName(blobName);

        if (!await ObjectExists(client, bucket, objectName))
        {
            return null;
        }

        var configuration = GetBlobConfiguration();

        var downloadUrl = await client.PresignedGetObjectAsync(
                new PresignedGetObjectArgs()
                    .WithBucket(bucket)
                    .WithObject(objectName)
                    .WithExpiry(configuration.PresignedGetExpirySeconds));

        var httpClient = HttpClientFactory.CreateMinioHttpClient();

        return await httpClient.GetStreamAsync(downloadUrl, cancellationToken);
    }

    public virtual Task CreateFolderAsync(
        string containerName, 
        string blobName,
        CancellationToken cancellationToken = default)
    {
        // Minio没有目录的概念,新建对象时可以模拟目录
        return Task.CompletedTask;
    }

    public async virtual Task UploadBlobAsync(
        string containerName, 
        string blobName, 
        Stream content, 
        CancellationToken cancellationToken = default)
    {
        var client = GetMinioClient();
        var bucket = NormalizeContainerName(containerName);
        var objectName = CalculateBlobName(blobName);

        await CreateBucketIfNotExists(client, bucket);

        await client.PutObjectAsync(new PutObjectArgs()
           .WithBucket(bucket)
           .WithObject(objectName)
           .WithStreamData(content)
           .WithObjectSize(content.Length),
           cancellationToken);
    }

    protected virtual IMinioClient GetMinioClient()
    {
        var configuration = GetBlobConfiguration();

        var client = new MinioClient()
            .WithEndpoint(configuration.EndPoint)
            .WithCredentials(configuration.AccessKey, configuration.SecretKey);

        if (configuration.WithSSL)
        {
            client.WithSSL();
        }

        return client.Build();
    }

    protected async virtual Task CreateBucketIfNotExists(IMinioClient client, string bucket)
    {
        if (!await client.BucketExistsAsync(new BucketExistsArgs().WithBucket(bucket)))
        {
            await client.MakeBucketAsync(new MakeBucketArgs().WithBucket(bucket));
        }
    }

    protected async virtual Task<bool> BucketExists(IMinioClient client, string bucket)
    {
        var args = new BucketExistsArgs().WithBucket(bucket);

        return await client.BucketExistsAsync(args);
    }

    protected async virtual Task<bool> ObjectExists(IMinioClient client, string bucket, string @object)
    {
        if (await client.BucketExistsAsync(new BucketExistsArgs().WithBucket(bucket)))
        {
            try
            {
                await client.StatObjectAsync(new StatObjectArgs().WithBucket(bucket).WithObject(@object));
            }
            catch (Exception e)
            {
                if (e is ObjectNotFoundException)
                {
                    return false;
                }

                throw;
            }

            return true;
        }

        return false;
    }

    protected virtual MinioBlobProviderConfiguration GetBlobConfiguration()
    {
        var configuration = ConfigurationProvider.Get<BlobManagementContainer>();
        var blobConfiguration = configuration.GetMinioConfiguration();
        return blobConfiguration;
    }

    protected virtual string NormalizeContainerName(string containerName)
    {
        if (!containerName.StartsWith("abp-"))
        {
            containerName = "abp-" + containerName;
        }
        return BlobNamingNormalizer.NormalizeContainerName(containerName);
    }

    protected virtual string GetPrefixPath()
    {
        return CurrentTenant.Id == null
           ? $"host/"
           : $"tenants/{CurrentTenant.Id.Value.ToString("D")}/";
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
