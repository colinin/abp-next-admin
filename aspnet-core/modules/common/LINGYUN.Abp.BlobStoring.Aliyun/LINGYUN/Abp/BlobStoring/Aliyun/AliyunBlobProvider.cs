using Aliyun.OSS;
using LINGYUN.Abp.Aliyun.Authorization;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Volo.Abp.BlobStoring;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.BlobStoring.Aliyun
{
    public class AliyunBlobProvider : BlobProviderBase, ITransientDependency
    {
        protected AbpAliyunOptions Options { get; }
        protected IAliyunBlobNameCalculator AliyunBlobNameCalculator { get; }

        public AliyunBlobProvider(
            IOptions<AbpAliyunOptions> options,
            IAliyunBlobNameCalculator aliyunBlobNameCalculator)
        {
            Options = options.Value;
            AliyunBlobNameCalculator = aliyunBlobNameCalculator;
        }

        public override async Task<bool> DeleteAsync(BlobProviderDeleteArgs args)
        {
            var blobName = AliyunBlobNameCalculator.Calculate(args);

            if (await BlobExistsAsync(args, blobName))
            {
                var ossClient = GetOssClient(args);

                return ossClient.DeleteObject(GetBucketName(args), blobName).DeleteMarker;
            }

            return false;
        }

        public override async Task<bool> ExistsAsync(BlobProviderExistsArgs args)
        {
            var blobName = AliyunBlobNameCalculator.Calculate(args);

            return await BlobExistsAsync(args, blobName);
        }

        public override async Task<Stream> GetOrNullAsync(BlobProviderGetArgs args)
        {
            var blobName = AliyunBlobNameCalculator.Calculate(args);

            if (!await BlobExistsAsync(args, blobName))
            {
                return null;
            }

            var ossClient = GetOssClient(args);
            var ossObject = ossClient.GetObject(GetBucketName(args), blobName);
            var memoryStream = new MemoryStream();
            await ossObject.Content.CopyToAsync(memoryStream);
            return memoryStream;
        }

        public override async Task SaveAsync(BlobProviderSaveArgs args)
        {
            var blobName = AliyunBlobNameCalculator.Calculate(args);
            var configuration = args.Configuration.GetAliyunConfiguration();

            // 先检查Bucket
            if (configuration.CreateBucketIfNotExists)
            {
                await CreateBucketIfNotExists(args, configuration.CreateBucketReferer);
            }

            var bucketName = GetBucketName(args);
            var ossClient = GetOssClient(args);

            // 是否已存在
            if (await BlobExistsAsync(args, blobName))
            {
                // 是否覆盖
                if (!args.OverrideExisting)
                {
                    throw new BlobAlreadyExistsException($"Saving BLOB '{args.BlobName}' does already exists in the bucketName '{GetBucketName(args)}'! Set {nameof(args.OverrideExisting)} if it should be overwritten.");
                }
                else
                {
                    // 删除原文件
                    ossClient.DeleteObject(bucketName, blobName);
                }
            }
            // 保存
            ossClient.PutObject(bucketName, blobName, args.BlobStream);
        }

        protected virtual OssClient GetOssClient(BlobProviderArgs args)
        {
            var configuration = args.Configuration.GetAliyunConfiguration();
            var ossClient = new OssClient(configuration.Endpoint, Options.AccessKeyId, Options.AccessKeySecret);
            return ossClient;
        }

        protected virtual async Task CreateBucketIfNotExists(BlobProviderArgs args, IList<string> refererList = null)
        {
            if (! await BucketExistsAsync(args))
            {
                var ossClient = GetOssClient(args);
                var bucketName = GetBucketName(args);

                var request = new CreateBucketRequest(bucketName)
                {
                    //设置存储空间访问权限ACL。
                    ACL = CannedAccessControlList.PublicReadWrite,
                    //设置数据容灾类型。
                    DataRedundancyType = DataRedundancyType.ZRS
                };

                ossClient.CreateBucket(request);

                if (refererList != null && refererList.Count > 0)
                {
                    var srq = new SetBucketRefererRequest(bucketName, refererList);
                    ossClient.SetBucketReferer(srq);
                }
            }
        }

        private async Task<bool> BlobExistsAsync(BlobProviderArgs args, string blobName)
        {
            var ossClient = GetOssClient(args);
            var bucketExists = await BucketExistsAsync(args);
            if (bucketExists)
            {
                var objectExists = ossClient.DoesObjectExist(GetBucketName(args), blobName);

                return objectExists;
            }
            return false;
        }

        private Task<bool> BucketExistsAsync(BlobProviderArgs args)
        {
            var ossClient = GetOssClient(args);
            var bucketExists = ossClient.DoesBucketExist(GetBucketName(args));

            return Task.FromResult(bucketExists);
        }

        private static string GetBucketName(BlobProviderArgs args)
        {
            var configuration = args.Configuration.GetAliyunConfiguration();
            return configuration.BucketName.IsNullOrWhiteSpace()
                ? args.ContainerName
                : configuration.BucketName;
        }
    }
}
