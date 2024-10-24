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
        IOptions<AbpOssManagementOptions> options)
        : base(options, serviceScopeFactory)
    {
        Clock = clock;
        Logger = logger;
        CurrentTenant = currentTenant;
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

        var response = await client.RemoveObjectsAsync(args);

        var tcs = new TaskCompletionSource<bool>();

        using var _ = response.Subscribe(
            onNext: (error) =>
            {
                Logger.LogWarning("Batch deletion of objects failed, error details {code}: {message}", error.Code, error.Message);
            },
            onError: tcs.SetException,
            onCompleted: () => tcs.SetResult(true));

        await tcs.Task;
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

    public async override Task<OssObject> CreateObjectAsync(CreateOssObjectRequest request)
    {
        var client = GetMinioClient();

        var bucket = GetBucket(request.Bucket);
        var prefixPath = GetPrefixPath();
        var objectPath = GetBlobPath(prefixPath, request.Path);
        var objectName = objectPath.IsNullOrWhiteSpace()
            ? request.Object
            : objectPath + request.Object;

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
            var emptyContent = "This is an OSS object that simulates a directory.".GetBytes();
            request.SetContent(new MemoryStream(emptyContent));
        }
        var putResponse = await client.PutObjectAsync(new PutObjectArgs()
           .WithBucket(bucket)
           .WithObject(objectName)
           .WithStreamData(request.Content)
           .WithObjectSize(request.Content.Length));
        
        if (request.ExpirationTime.HasValue)
        {
            var lifecycleRule = new LifecycleRule
            {
                Status = "Enabled",
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
        if (request.Object.EndsWith('/'))
        {
            // Minio系统设计并不支持目录的形式
            // 如果是目录的形式,那必定有文件存在,抛出目录不为空即可
            throw new BusinessException(code: OssManagementErrorCodes.ObjectDeleteWithNotEmpty);
        }

        var client = GetMinioClient();

        var bucket = GetBucket(request.Bucket);

        var prefixPath = GetPrefixPath();
        var objectPath = GetBlobPath(prefixPath, request.Path);
        var objectName = objectPath.IsNullOrWhiteSpace()
            ? request.Object
            : objectPath + request.Object;

        if (await BucketExists(client, bucket) &&
            await ObjectExists(client, bucket, objectName))
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

            var expiredObjectItem = client.ListObjectsAsync(listObjectArgs);

            var tcs = new TaskCompletionSource<bool>();
            using var _ = expiredObjectItem.Subscribe(
                onNext: (item) =>
                {
                    var lifecycleRule = new LifecycleRule
                    {
                        Status = "Enabled",
                        ID = item.Key,
                        Expiration = new Expiration(Clock.Normalize(request.ExpirationTime.DateTime))
                    };
                    var lifecycleConfiguration = new LifecycleConfiguration();
                    lifecycleConfiguration.Rules.Add(lifecycleRule);

                    var lifecycleArgs = new SetBucketLifecycleArgs()
                        .WithBucket(bucket)
                        .WithLifecycleConfiguration(lifecycleConfiguration);

                    var _ = client.SetBucketLifecycleAsync(lifecycleArgs);
                },
                onError: tcs.SetException,
                onCompleted: () => tcs.SetResult(true)
            );

            await tcs.Task; 
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

        var tcs = new TaskCompletionSource<bool>();

        var listObjectResult = client.ListObjectsAsync(listObjectArgs);

        var resultObjects = new List<OssObject>();

        using var _ = listObjectResult.Subscribe(
            onNext: (item) =>
            {
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
            },
            onError: (ex) =>
            {
                tcs.TrySetException(ex);
            },
            onCompleted: () =>
            {
                tcs.SetResult(true);
            }
        );

        await tcs.Task;

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
        var tcs = new TaskCompletionSource<bool>();
        var listObjectObs = client.ListObjectsAsync(
            new ListObjectsArgs()
                .WithBucket(bucket));

        var listObjects = new List<string>();
        using var _ = listObjectObs.Subscribe(
            (item) =>
            {
                listObjects.Add(item.Key);
                tcs.TrySetResult(true);
            },
            (ex) => tcs.TrySetException(ex),
            () => tcs.TrySetResult(true));

        await tcs.Task;

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
            : objectPath.EnsureEndsWith('/') + request.Object;

        if (!await ObjectExists(client, bucket, objectName))
        {
            throw new BusinessException(code: OssManagementErrorCodes.ObjectNotFound);
        }

        var memoryStream = new MemoryStream();
        var getObjectArgs = new GetObjectArgs()
                .WithBucket(bucket)
                .WithObject(objectName)
                .WithCallbackStream((stream) =>
                {
                    if (stream != null)
                    {
                        stream.CopyTo(memoryStream);
                        memoryStream.Seek(0, SeekOrigin.Begin);
                    }
                    else
                    {
                        memoryStream = null;
                    }
                });
        var getObjectResult = await client.GetObjectAsync(getObjectArgs);

        var ossObject = new OssObject(
            !objectPath.IsNullOrWhiteSpace()
                ? getObjectResult.ObjectName.Replace(objectPath, "")
                : getObjectResult.ObjectName,
            request.Path,
            getObjectResult.ETag,
            getObjectResult.LastModified,
            memoryStream.Length,
            getObjectResult.LastModified,
            getObjectResult.MetaData,
            getObjectResult.ObjectName.EndsWith("/"))
        {
            FullName = getObjectResult.ObjectName.Replace(prefixPath, "")
        };

        if (memoryStream.Length > 0)
        {
            ossObject.SetContent(memoryStream);
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

        return bucket.IsNullOrWhiteSpace()
            ? BlobNormalizeNamingService.NormalizeContainerName(configuration, minioConfiguration.BucketName!)
            : BlobNormalizeNamingService.NormalizeContainerName(configuration, bucket);
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

        return resultPath.Replace("//", "");
    }
}
