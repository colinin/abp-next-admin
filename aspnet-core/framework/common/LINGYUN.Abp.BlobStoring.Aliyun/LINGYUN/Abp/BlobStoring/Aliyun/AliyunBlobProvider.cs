using AlibabaCloud.OSS.V2;
using AlibabaCloud.OSS.V2.Models;
using LINGYUN.Abp.Aliyun.Features;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Volo.Abp.BlobStoring;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Features;
using Volo.Abp.Timing;

namespace LINGYUN.Abp.BlobStoring.Aliyun;

[RequiresFeature(AliyunFeatureNames.BlobStoring.Enable)]
public class AliyunBlobProvider : BlobProviderBase, ITransientDependency
{
    protected IClock Clock { get; }
    protected IOssClientFactory OssClientFactory { get; }
    protected IHttpClientFactory HttpClientFactory { get; }
    protected IAliyunBlobNameCalculator AliyunBlobNameCalculator { get; }

    public AliyunBlobProvider(
        IClock clock,
        IOssClientFactory ossClientFactory,
        IHttpClientFactory httpClientFactory,
        IAliyunBlobNameCalculator aliyunBlobNameCalculator)
    {
        Clock = clock;
        OssClientFactory = ossClientFactory;
        HttpClientFactory = httpClientFactory;
        AliyunBlobNameCalculator = aliyunBlobNameCalculator;
    }

    public override async Task<bool> DeleteAsync(BlobProviderDeleteArgs args)
    {
        var ossClient = await GetOssClientAsync(args);
        var blobName = AliyunBlobNameCalculator.Calculate(args);

        if (await BlobExistsAsync(ossClient, args, blobName))
        {
            var deleteObjectRes = await ossClient.DeleteObjectAsync(
                new DeleteObjectRequest
                {
                    Bucket = GetBucketName(args),
                    Key = blobName,
                },
                cancellationToken: args.CancellationToken);
            return deleteObjectRes.DeleteMarker == true;
        }

        return false;
    }

    public override async Task<bool> ExistsAsync(BlobProviderExistsArgs args)
    {
        var ossClient = await GetOssClientAsync(args);
        var blobName = AliyunBlobNameCalculator.Calculate(args);

        return await BlobExistsAsync(ossClient, args, blobName);
    }

    public override async Task<Stream> GetOrNullAsync(BlobProviderGetArgs args)
    {
        var ossClient = await GetOssClientAsync(args);
        var blobName = AliyunBlobNameCalculator.Calculate(args);

        if (!await BlobExistsAsync(ossClient, args, blobName))
        {
            return null;
        }

        var result = await ossClient.GetObjectAsync(
            new GetObjectRequest
            {
                Bucket = GetBucketName(args),
                Key = blobName,
            });
        return result.Body;

        // TODO: 阿里云sdk预签名不可用[2026/05/23]
        //var configuration = args.Configuration.GetAliyunConfiguration();
        //var presignResult = ossClient.Presign(
        //    new GetObjectRequest
        //    {
        //        Bucket = GetBucketName(args),
        //        Key = blobName,
        //    }, 
        //    Clock.Now.AddSeconds(configuration.PresignedGetExpirySeconds));

        //var httpClient = HttpClientFactory.CreateAliyunHttpClient();

        //return await httpClient.GetStreamAsync(presignResult.Url, args.CancellationToken);
    }

    public override async Task SaveAsync(BlobProviderSaveArgs args)
    {
        var ossClient = await GetOssClientAsync(args);
        var blobName = AliyunBlobNameCalculator.Calculate(args);
        var configuration = args.Configuration.GetAliyunConfiguration();

        // 先检查Bucket
        if (configuration.CreateBucketIfNotExists)
        {
            await CreateBucketIfNotExists(ossClient, args, configuration.CreateBucketAcl);
        }

        var bucketName = GetBucketName(args);

        // 是否已存在
        if (await BlobExistsAsync(ossClient, args, blobName))
        {
            // 是否覆盖
            if (!args.OverrideExisting)
            {
                throw new BlobAlreadyExistsException($"Saving BLOB '{args.BlobName}' does already exists in the bucketName '{GetBucketName(args)}'! Set {nameof(args.OverrideExisting)} if it should be overwritten.");
            }
            else
            {
                // 删除原文件
                await ossClient.DeleteObjectAsync(
                    new DeleteObjectRequest
                    {
                        Bucket = bucketName,
                        Key = blobName,
                    },
                    cancellationToken: args.CancellationToken);
            }
        }
        // 保存
        await ossClient.PutObjectAsync(
            new PutObjectRequest
            {
                Bucket = bucketName,
                Key = blobName,
                Body = args.BlobStream,
            },
            cancellationToken: args.CancellationToken);
    }

    protected async virtual Task<Client> GetOssClientAsync(BlobProviderArgs args)
    {
        return await OssClientFactory.CreateAsync();
    }

    protected async virtual Task CreateBucketIfNotExists(Client ossClient, BlobProviderArgs args, BucketAclType? bucketAcl = null)
    {
        if (!await BucketExistsAsync(ossClient, args))
        {
            var bucketName = GetBucketName(args);

            await ossClient.PutBucketAsync(
                new PutBucketRequest
                {
                    Bucket = bucketName,
                },
                cancellationToken: args.CancellationToken);

            if (bucketAcl.HasValue)
            {
                await ossClient.PutBucketAclAsync(
                    new PutBucketAclRequest
                    {
                        Bucket = bucketName,
                        Acl = bucketAcl.Value.GetString(),
                    },
                    cancellationToken: args.CancellationToken);
            }
        }
    }

    private async Task<bool> BlobExistsAsync(Client ossClient, BlobProviderArgs args, string blobName)
    {
        var bucketExists = await BucketExistsAsync(ossClient, args);
        if (bucketExists)
        {
            return await ossClient.IsObjectExistAsync(GetBucketName(args), blobName, cancellationToken: args.CancellationToken);
        }
        return false;
    }

    private async Task<bool> BucketExistsAsync(Client ossClient,  BlobProviderArgs args)
    {
        return await ossClient.IsBucketExistAsync(GetBucketName(args), args.CancellationToken);
    }

    private static string GetBucketName(BlobProviderArgs args)
    {
        var configuration = args.Configuration.GetAliyunConfiguration();
        return configuration.BucketName.IsNullOrWhiteSpace()
            ? args.ContainerName
            : configuration.BucketName;
    }
}
