using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Minio;
using Minio.DataModel.Args;
using Minio.DataModel.ILM;
using Minio.Exceptions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Net.Http;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.BlobStoring;
using Volo.Abp.BlobStoring.Minio;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Timing;

namespace LINGYUN.Abp.OssManagement.Minio;

/// <summary>
/// Oss容器的Minio实现
/// </summary>
public class MinioOssContainer : OssContainerBase, IOssObjectExpireor
{
    protected IMinioBlobNameCalculator MinioBlobNameCalculator { get; }
    protected IBlobNormalizeNamingService BlobNormalizeNamingService { get; }
    protected IBlobContainerConfigurationProvider ConfigurationProvider { get; }
    protected IHttpClientFactory HttpClientFactory { get; }

    protected IClock Clock { get; }
    protected ICurrentTenant CurrentTenant { get; }
    protected ILogger<MinioOssContainer> Logger { get; }

    public MinioOssContainer(
        IClock clock,
        ICurrentTenant currentTenant,
        ILogger<MinioOssContainer> logger,
        IMinioBlobNameCalculator minioBlobNameCalculator, 
        IBlobNormalizeNamingService blobNormalizeNamingService, 
        IBlobContainerConfigurationProvider configurationProvider,
        IServiceScopeFactory serviceScopeFactory,
        IHttpClientFactory httpClientFactory,
        IOptions<AbpOssManagementOptions> options)
        : base(options, serviceScopeFactory)
    {
        Clock = clock;
        Logger = logger;
        CurrentTenant = currentTenant;
        HttpClientFactory = httpClientFactory;
        MinioBlobNameCalculator = minioBlobNameCalculator;
        BlobNormalizeNamingService = blobNormalizeNamingService;
        ConfigurationProvider = configurationProvider;
    }

    public async override Task BulkDeleteObjectsAsync(BulkDeleteObjectRequest request)
    {
        var client = GetMinioClient();

        var bucket = GetBucket(request.Bucket);

        var prefixPath = GetPrefixPath();
        var path = GetBlobPath(prefixPath, request.Path);

        var args = new RemoveObjectsArgs()
            .WithBucket(bucket)
            .WithObjects(request.Objects.Select((x) => path + x.RemovePreFix("/")).ToList());

        var errors = await client.RemoveObjectsAsync(args);

        foreach (var error in errors)
        {
            Logger.LogWarning("Batch deletion of objects failed, error details {code}: {message}", error.Code, error.Message);
        }
    }

    public async override Task<OssContainer> CreateAsync(string name)
    {
        var client = GetMinioClient();

        var bucket = GetBucket(name);

        if (await BucketExists(client, bucket))
        {
            throw new BusinessException(code: OssManagementErrorCodes.ContainerAlreadyExists);
        }

        await client.MakeBucketAsync(new MakeBucketArgs().WithBucket(bucket));

        return new OssContainer(
            name,
            Clock.Now,
            0L,
            Clock.Now,
            new Dictionary<string, string>());
    }

    public async override Task<bool> ObjectExistsAsync(GetOssObjectRequest request)
    {
        var client = GetMinioClient();

        var bucket = GetBucket(request.Bucket);
        var prefixPath = GetPrefixPath();
        var objectPath = GetBlobPath(prefixPath, request.Path);
        var objectName = objectPath.IsNullOrWhiteSpace()
            ? request.Object
            : objectPath + request.Object;

        return await ObjectExists(client, bucket, objectName);
    }

    public async override Task<OssObject> CreateObjectAsync(CreateOssObjectRequest request)
    {
        var client = GetMinioClient();

        var bucket = GetBucket(request.Bucket);
        var prefixPath = GetPrefixPath();
        var objectPath = GetBlobPath(prefixPath, request.Path);
        var objectName = objectPath.IsNullOrWhiteSpace()
            ? request.Object
            : objectPath + request.Object;
        var isDir = false;

        if (!request.Overwrite && await ObjectExists(client, bucket, objectName))
        {
            throw new BusinessException(code: OssManagementErrorCodes.ObjectAlreadyExists);
        }

        // 没有bucket则创建
        if (!await BucketExists(client, bucket))
        {
            var configuration = GetMinioConfiguration();
            if (!configuration.CreateBucketIfNotExists)
            {
                throw new BusinessException(code: OssManagementErrorCodes.ContainerNotFound);
            }
            await client.MakeBucketAsync(new MakeBucketArgs().WithBucket(bucket));
        }
        if (request.Content.IsNullOrEmpty())
        {
            isDir = true;
            var emptyContent = "This is an OSS object that simulates a directory.".GetBytes();
            request.SetContent(new MemoryStream(emptyContent));
        }
        var putResponse = await client.PutObjectAsync(new PutObjectArgs()
           .WithBucket(bucket)
           .WithObject(isDir ? $"{objectName}/_dir" : objectName)
           .WithStreamData(request.Content)
           .WithObjectSize(request.Content.Length));
        
        if (request.ExpirationTime.HasValue)
        {
            var lifecycleRule = new LifecycleRule
            {
                Status = LifecycleRule.LifecycleRuleStatusEnabled,
                ID = putResponse.Etag,
                Expiration = new Expiration(Clock.Now.Add(request.ExpirationTime.Value))
            };
            var lifecycleConfiguration = new LifecycleConfiguration();
            lifecycleConfiguration.Rules.Add(lifecycleRule);

            var lifecycleArgs = new SetBucketLifecycleArgs()
                .WithBucket(bucket)
                .WithLifecycleConfiguration(lifecycleConfiguration);

            await client.SetBucketLifecycleAsync(lifecycleArgs);
        }

        var ossObject = new OssObject(
            !objectPath.IsNullOrWhiteSpace()
                ? objectName.Replace(objectPath, "")
                : objectName,
            objectPath.Replace(prefixPath, ""),
            putResponse.Etag,
            Clock.Now,
            putResponse.Size,
            Clock.Now,
            new Dictionary<string, string>(),
            objectName.EndsWith('/'))
        {
            FullName = objectName.Replace(prefixPath, "")
        };

        if (!Equals(request.Content, Stream.Null))
        {
            request.Content.Seek(0, SeekOrigin.Begin);
            ossObject.SetContent(request.Content);
        }

        return ossObject;
    }

    public async override Task DeleteObjectAsync(GetOssObjectRequest request)
    {
        var client = GetMinioClient();

        var bucket = GetBucket(request.Bucket);

        var prefixPath = GetPrefixPath();
        var objectPath = GetBlobPath(prefixPath, request.Path);
        var objectName = objectPath.IsNullOrWhiteSpace()
            ? request.Object
            : objectPath + request.Object;

        if (objectName.EndsWith('/') && await BucketExists(client, bucket))
        {
            var objectNames = new List<string>();
            var objects = client.ListObjectsEnumAsync(
                new ListObjectsArgs()
                    .WithBucket(bucket)
                    .WithPrefix(objectName)
                    .WithRecursive(true));

            await foreach (var @object in objects)
            {
                objectNames.Add(@object.Key);
            }

            var errors = await client.RemoveObjectsAsync(
                new RemoveObjectsArgs()
                    .WithBucket(bucket)
                    .WithObjects(objectNames));

            foreach (var error in errors)
            {
                Logger.LogWarning("Batch deletion of objects failed, error details {code}: {message}", error.Code, error.Message);
            }

            return;
        }

        if (await ObjectExists(client, bucket, objectName))
        {
            var removeObjectArgs = new RemoveObjectArgs()
                .WithBucket(bucket)
                .WithObject(objectName);

            await client.RemoveObjectAsync(removeObjectArgs);
        }
    }

    public async override Task<bool> ExistsAsync(string name)
    {
        var client = GetMinioClient();

        var bucket = GetBucket(name);

        return await BucketExists(client, bucket);
    }

    public async virtual Task ExpireAsync(ExprieOssObjectRequest request)
    {
        var client = GetMinioClient();

        var bucket = GetBucket(request.Bucket);

        var bucketListResult = await client.ListBucketsAsync();
        var expiredBuckets = bucketListResult.Buckets.Take(request.Batch);

        foreach (var expiredBucket in expiredBuckets)
        {
            var listObjectArgs = new ListObjectsArgs()
                .WithBucket(expiredBucket.Name);

            var expiredObjects = client.ListObjectsEnumAsync(listObjectArgs);

            await foreach (var item in expiredObjects)
            {
                var lifecycleRule = new LifecycleRule
                {
                    Status = LifecycleRule.LifecycleRuleStatusEnabled,
                    ID = item.Key,
                    Expiration = new Expiration(Clock.Normalize(request.ExpirationTime.DateTime))
                };
                var lifecycleConfiguration = new LifecycleConfiguration();
                lifecycleConfiguration.Rules.Add(lifecycleRule);

                var lifecycleArgs = new SetBucketLifecycleArgs()
                    .WithBucket(bucket)
                    .WithLifecycleConfiguration(lifecycleConfiguration);

                await client.SetBucketLifecycleAsync(lifecycleArgs);
            }
        }
    }

    public async override Task<OssContainer> GetAsync(string name)
    {
        var client = GetMinioClient();

        var bucket = GetBucket(name);

        var bucketListResult = await client.ListBucketsAsync();

        var bucketInfo = bucketListResult.Buckets.FirstOrDefault((x) => x.Name == bucket);
        if (bucketInfo == null)
        {
            throw new BusinessException(code: OssManagementErrorCodes.ContainerNotFound);
        }

        return new OssContainer(
            bucketInfo.Name,
            bucketInfo.CreationDateDateTime,
            0L,
            bucketInfo.CreationDateDateTime,
            new Dictionary<string, string>());
    }

    public async override Task<GetOssContainersResponse> GetListAsync(GetOssContainersRequest request)
    {
        var client = GetMinioClient();

        var bucketListResult = await client.ListBucketsAsync();

        var totalCount = bucketListResult.Buckets.Count;

        var resultObjects = bucketListResult.Buckets
            .AsQueryable()
            .OrderBy(x => x.Name)
            .PageBy(request.Current, request.MaxKeys ?? 10)
            .Select(x => new OssContainer(
                x.Name,
                x.CreationDateDateTime,
                0L,
                null,
                new Dictionary<string, string>()))
            .ToList();

        return new GetOssContainersResponse(
            request.Prefix,
            request.Marker,
            null,
            totalCount,
            resultObjects);
    }

    public async override Task<GetOssObjectsResponse> GetObjectsAsync(GetOssObjectsRequest request)
    {
        var client = GetMinioClient();

        var bucket = GetBucket(request.BucketName);

        var prefixPath = GetPrefixPath();
        var objectPath = GetBlobPath(prefixPath, request.Prefix);
        var marker = !objectPath.IsNullOrWhiteSpace() && !request.Marker.IsNullOrWhiteSpace()
            ? request.Marker.Replace(objectPath, "")
            : request.Marker;

        var listObjectArgs = new ListObjectsArgs()
            .WithBucket(bucket)
            .WithPrefix(objectPath);

        var resultObjects = new List<OssObject>();

        var listObjectResult = client.ListObjectsEnumAsync(listObjectArgs);

        await foreach (var item in listObjectResult)
        {
            // 作为目录占位,无需显示
            if (item.Key.EndsWith("_dir"))
            {
                continue;
            }
            resultObjects.Add(new OssObject(
                    !objectPath.IsNullOrWhiteSpace()
                        ? item.Key.Replace(objectPath, "")
                        : item.Key,
                    request.Prefix,
                    item.ETag,
                    item.LastModifiedDateTime,
                    item.Size.To<long>(),
                    item.LastModifiedDateTime,
                    new Dictionary<string, string>(),
                    item.IsDir));
        }

        var totalCount = resultObjects.Count;
        resultObjects = resultObjects
            .AsQueryable()
            .OrderBy(x => x.Name)
            .PageBy(request.Current, request.MaxKeys ?? 10)
            .ToList();

        return new GetOssObjectsResponse(
            bucket,
            request.Prefix,
            request.Marker,
            marker,
            "/",
            totalCount,
            resultObjects);
    }

    protected async override Task DeleteBucketAsync(string name)
    {
        var client = GetMinioClient();
        var bucket = GetBucket(name);

        if (!await BucketExists(client, bucket))
        {
            throw new BusinessException(code: OssManagementErrorCodes.ContainerNotFound);
        }

        // 非空目录无法删除
        var listObjects = new List<string>();

        var listObjectObs = client.ListObjectsEnumAsync(
            new ListObjectsArgs()
                .WithBucket(bucket));
        await foreach (var item in listObjectObs)
        {
            listObjects.Add(item.Key);
        }

        if (listObjects.Count > 0)
        {
            throw new BusinessException(code: OssManagementErrorCodes.ContainerDeleteWithNotEmpty);
        }

        var deleteBucketArgs = new RemoveBucketArgs()
                .WithBucket(bucket);

        await client.RemoveBucketAsync(deleteBucketArgs);
    }

    protected async override Task<OssObject> GetOssObjectAsync(GetOssObjectRequest request)
    {
        var client = GetMinioClient();

        var bucket = GetBucket(request.Bucket);

        var prefixPath = GetPrefixPath();
        var objectPath = GetBlobPath(prefixPath, request.Path);
        var objectName = objectPath.IsNullOrWhiteSpace()
            ? request.Object
            : objectPath + request.Object;

        if (!await ObjectExists(client, bucket, objectName))
        {
            throw new BusinessException(code: OssManagementErrorCodes.ObjectNotFound);
        }

        var getObjectResult = await client.StatObjectAsync(
            new StatObjectArgs()
                .WithBucket(bucket)
                .WithObject(objectName));

        var ossObject = new OssObject(
            !objectPath.IsNullOrWhiteSpace()
                ? getObjectResult.ObjectName.Replace(objectPath, "")
                : getObjectResult.ObjectName,
            request.Path,
            getObjectResult.ETag,
            getObjectResult.LastModified,
            getObjectResult.Size,
            getObjectResult.LastModified,
            getObjectResult.MetaData,
            getObjectResult.ObjectName.EndsWith("/"))
        {
            FullName = getObjectResult.ObjectName.Replace(prefixPath, "")
        };

        if (getObjectResult.Size > 0)
        {
            var objectUrl = await client.PresignedGetObjectAsync(
                new PresignedGetObjectArgs()
                    .WithBucket(bucket)
                    .WithObject(objectName)
                    .WithExpiry(3600));
            var httpClient = HttpClientFactory.CreateMinioHttpClient();

            ossObject.SetContent(await httpClient.GetStreamAsync(objectUrl));
        }

        return ossObject;
    }

    protected virtual IMinioClient GetMinioClient()
    {
        var configuration = GetMinioConfiguration();

        var client = new MinioClient()
            .WithEndpoint(configuration.EndPoint)
            .WithCredentials(configuration.AccessKey, configuration.SecretKey);

        if (configuration.WithSSL)
        {
            client.WithSSL();
        }

        return client.Build();
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

    protected virtual MinioBlobProviderConfiguration GetMinioConfiguration()
    {
        var configuration = ConfigurationProvider.Get<AbpOssManagementContainer>();

        return configuration.GetMinioConfiguration();
    }

    protected virtual string GetBucket(string bucket)
    {
        var configuration = ConfigurationProvider.Get<AbpOssManagementContainer>();
        var minioConfiguration = configuration.GetMinioConfiguration();
        var blobPath = minioConfiguration.BucketName;
        if (string.Equals(bucket, blobPath, StringComparison.InvariantCultureIgnoreCase))
        {
            return bucket;
        }
        
        return bucket;
    }

    protected virtual string GetPrefixPath()
    {
        return CurrentTenant.Id == null
           ? $"host/"
           : $"tenants/{CurrentTenant.Id.Value.ToString("D")}/";
    }

    protected virtual string GetBlobPath(string basePath, string path)
    {
        var resultPath = $"{basePath}{(
            path.IsNullOrWhiteSpace() ? "" :
            path.Replace("./", "").RemovePreFix("/"))}";

        return resultPath.Replace("//", "").EnsureEndsWith('/');
    }
}
