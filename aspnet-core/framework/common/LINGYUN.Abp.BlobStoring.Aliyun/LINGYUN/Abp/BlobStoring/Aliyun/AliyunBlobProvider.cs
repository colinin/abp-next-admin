using Aliyun.OSS;
using LINGYUN.Abp.Aliyun.Features;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Volo.Abp.BlobStoring;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Features;

namespace LINGYUN.Abp.BlobStoring.Aliyun
{
    [RequiresFeature(AliyunFeatureNames.BlobStoring.Enable)]
    public class AliyunBlobProvider : BlobProviderBase, ITransientDependency
    {
        protected IOssClientFactory OssClientFactory { get; }
        protected IAliyunBlobNameCalculator AliyunBlobNameCalculator { get; }

        public AliyunBlobProvider(
            IOssClientFactory ossClientFactory,
            IAliyunBlobNameCalculator aliyunBlobNameCalculator)
        {
            OssClientFactory = ossClientFactory;
            AliyunBlobNameCalculator = aliyunBlobNameCalculator;
        }

        public override async Task<bool> DeleteAsync(BlobProviderDeleteArgs args)
        {
            var ossClient = await GetOssClientAsync(args);
            var blobName = AliyunBlobNameCalculator.Calculate(args);

            if (await BlobExistsAsync(ossClient, args, blobName))
            {
                return ossClient.DeleteObject(GetBucketName(args), blobName).DeleteMarker;
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

            var ossObject = ossClient.GetObject(GetBucketName(args), blobName);
            var memoryStream = new MemoryStream();
            await ossObject.Content.CopyToAsync(memoryStream);
            return memoryStream;
        }

        public override async Task SaveAsync(BlobProviderSaveArgs args)
        {
            var ossClient = await GetOssClientAsync(args);
            var blobName = AliyunBlobNameCalculator.Calculate(args);
            var configuration = args.Configuration.GetAliyunConfiguration();

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
                    ossClient.DeleteObject(bucketName, blobName);
                }
            }
            // 保存
            ossClient.PutObject(bucketName, blobName, args.BlobStream);
        }

        protected async virtual Task<IOss> GetOssClientAsync(BlobProviderArgs args)
        {
            var ossClient = await OssClientFactory.CreateAsync();
            return ossClient;
        }

        protected async virtual Task CreateBucketIfNotExists(IOss ossClient, BlobProviderArgs args, IList<string> refererList = null)
        {
            if (! await BucketExistsAsync(ossClient, args))
            {
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

        private async Task<bool> BlobExistsAsync(IOss ossClient, BlobProviderArgs args, string blobName)
        {
            var bucketExists = await BucketExistsAsync(ossClient, args);
            if (bucketExists)
            {
                var objectExists = ossClient.DoesObjectExist(GetBucketName(args), blobName);

                return objectExists;
            }
            return false;
        }

        private Task<bool> BucketExistsAsync(IOss ossClient,  BlobProviderArgs args)
        {
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
