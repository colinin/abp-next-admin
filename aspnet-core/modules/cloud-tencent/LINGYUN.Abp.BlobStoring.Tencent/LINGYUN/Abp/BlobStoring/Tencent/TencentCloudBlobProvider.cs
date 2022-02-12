using COSXML;
using COSXML.Common;
using COSXML.Model.Bucket;
using COSXML.Model.Object;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Volo.Abp.BlobStoring;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.BlobStoring.Tencent;

public class TencentCloudBlobProvider : BlobProviderBase, ITransientDependency
{
    protected ICosClientFactory CosClientFactory { get; }
    protected ITencentBlobNameCalculator TencentBlobNameCalculator { get; }

    public TencentCloudBlobProvider(
        ICosClientFactory cosClientFactory,
        ITencentBlobNameCalculator tencentBlobNameCalculator)
    {
        CosClientFactory = cosClientFactory;
        TencentBlobNameCalculator = tencentBlobNameCalculator;
    }

    public override async Task<bool> DeleteAsync(BlobProviderDeleteArgs args)
    {
        var ossClient = await GetOssClientAsync(args);
        var blobName = TencentBlobNameCalculator.Calculate(args);

        if (await BlobExistsAsync(ossClient, args, blobName))
        {
            var request = new DeleteObjectRequest(GetBucketName(args), blobName);
            return ossClient.DeleteObject(request).IsSuccessful();
        }

        return false;
    }

    public override async Task<bool> ExistsAsync(BlobProviderExistsArgs args)
    {
        var ossClient = await GetOssClientAsync(args);
        var blobName = TencentBlobNameCalculator.Calculate(args);

        return await BlobExistsAsync(ossClient, args, blobName);
    }

    public override async Task<Stream> GetOrNullAsync(BlobProviderGetArgs args)
    {
        var ossClient = await GetOssClientAsync(args);
        var blobName = TencentBlobNameCalculator.Calculate(args);

        if (!await BlobExistsAsync(ossClient, args, blobName))
        {
            return null;
        }

        // TODO: 未经验证

        var request = new GetObjectBytesRequest(GetBucketName(args), blobName);
        var ossObject = ossClient.GetObject(request);
        var memoryStream = new MemoryStream();
        await memoryStream.WriteAsync(ossObject.content, 0, ossObject.content.Length);
        memoryStream.Seek(0, SeekOrigin.Begin);
        return memoryStream;
    }

    public override async Task SaveAsync(BlobProviderSaveArgs args)
    {
        var ossClient = await GetOssClientAsync(args);
        var blobName = TencentBlobNameCalculator.Calculate(args);
        var configuration = args.Configuration.GetTencentConfiguration();

        // 先检查Bucket
        if (configuration.CreateBucketIfNotExists)
        {
            await CreateBucketIfNotExists(ossClient, args, configuration.CreateBucketReferer);
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
                var deleteRequest = new DeleteObjectRequest(bucketName, blobName);
                ossClient.DeleteObject(deleteRequest);
            }
        }
        // 保存
        var putRequest = new PutObjectRequest(bucketName, blobName, args.BlobStream);
        ossClient.PutObject(putRequest);
    }

    protected virtual async Task<CosXml> GetOssClientAsync(BlobProviderArgs args)
    {
        var configuration = args.Configuration.GetTencentConfiguration();
        var ossClient = await CosClientFactory.CreateAsync(configuration);
        return ossClient;
    }

    protected virtual async Task CreateBucketIfNotExists(CosXml cos, BlobProviderArgs args, IList<string> refererList = null)
    {
        if (!await BucketExistsAsync(cos, args))
        {
            var bucketName = GetBucketName(args);

            var request = new PutBucketRequest(bucketName);
            // TODO: good! 这很Java
            request.SetCosACL(CosACL.PublicReadWrite);

            cos.PutBucket(request);

            if (refererList != null && refererList.Count > 0)
            {
                var srq = new PutBucketRefererRequest(bucketName);
                var refererConfig = new COSXML.Model.Tag.RefererConfiguration();
                foreach (var domain in refererList)
                {
                    refererConfig.domainList.AddDomain(domain);
                }
                srq.SetRefererConfiguration(refererConfig);

                cos.PutBucketReferer(srq);
            }
        }
    }

    private async Task<bool> BlobExistsAsync(CosXml cos, BlobProviderArgs args, string blobName)
    {
        var bucketExists = await BucketExistsAsync(cos, args);
        if (bucketExists)
        {
            var request = new DoesObjectExistRequest(GetBucketName(args), blobName);
            var objectExists = cos.DoesObjectExist(request);

            return objectExists;
        }
        return false;
    }

    private Task<bool> BucketExistsAsync(CosXml cos, BlobProviderArgs args)
    {
        var request = new DoesBucketExistRequest(GetBucketName(args));
        var bucketExists = cos.DoesBucketExist(request);

        return Task.FromResult(bucketExists);
    }

    private static string GetBucketName(BlobProviderArgs args)
    {
        var configuration = args.Configuration.GetTencentConfiguration();
        return configuration.BucketName.IsNullOrWhiteSpace()
            ? args.ContainerName
            : configuration.BucketName;
    }
}
