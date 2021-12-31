using COSXML;
using COSXML.Model.Bucket;
using COSXML.Model.Object;
using COSXML.Model.Service;
using LINGYUN.Abp.BlobStoring.Tencent;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Timing;

namespace LINGYUN.Abp.OssManagement.Tencent
{
    /// <summary>
    /// Oss容器的阿里云实现
    /// </summary>
    internal class TencentOssContainer : IOssContainer
    {
        protected IClock Clock { get; }
        protected ICurrentTenant CurrentTenant { get; }
        protected ICosClientFactory CosClientFactory { get; }
        public TencentOssContainer(
            IClock clock,
            ICurrentTenant currentTenant,
            ICosClientFactory cosClientFactory)
        {
            Clock = clock;
            CurrentTenant = currentTenant;
            CosClientFactory = cosClientFactory;
        }
        public virtual async Task BulkDeleteObjectsAsync(BulkDeleteObjectRequest request)
        {
            var ossClient = await CreateClientAsync();

            var path = GetBasePath(request.Path);
            var deleteRequest = new DeleteMultiObjectRequest(request.Bucket);
            deleteRequest.SetObjectKeys(request.Objects.Select(x => x += path).ToList());

            ossClient.DeleteMultiObjects(deleteRequest);
        }

        public virtual async Task<OssContainer> CreateAsync(string name)
        {
            var ossClient = await CreateClientAsync();

            if (BucketExists(ossClient, name))
            {
                throw new BusinessException(code: OssManagementErrorCodes.ContainerAlreadyExists);
            }

            var putBucketRequest = new PutBucketRequest(name);
            var bucketResult = ossClient.PutBucket(putBucketRequest);

            return new OssContainer(
                bucketResult.Key,
                Clock.Now,
                0L,
                Clock.Now,
                new Dictionary<string, string>
                {
                    { "Id", bucketResult.Key },
                    { "DisplayName", bucketResult.Key }
                });
        }

        public virtual async Task<OssObject> CreateObjectAsync(CreateOssObjectRequest request)
        {
            var ossClient = await CreateClientAsync();

            var objectPath = GetBasePath(request.Path);

            var objectName = objectPath.IsNullOrWhiteSpace()
                ? request.Object
                : objectPath + request.Object;

            if (!request.Overwrite && ObjectExists(ossClient, request.Bucket, objectName))
            {
                throw new BusinessException(code: OssManagementErrorCodes.ObjectAlreadyExists);
            }

            // 当一个对象名称是以 / 结尾时，不论该对象是否存有数据，都以目录的形式存在
            // 详情见:https://help.aliyun.com/document_detail/31910.html
            if (objectName.EndsWith("/") &&
                request.Content.IsNullOrEmpty())
            {
                var emptyStream = new MemoryStream();
                var emptyData = System.Text.Encoding.UTF8.GetBytes("");
                await emptyStream.WriteAsync(emptyData, 0, emptyData.Length);
                request.SetContent(emptyStream);
            }

            // 没有bucket则创建
            if (!BucketExists(ossClient, request.Bucket))
            {
                var putBucketRequest = new PutBucketRequest(request.Bucket);
                ossClient.PutBucket(putBucketRequest);
            }

            var contentLength = request.Content.Length;
            var putObjectRequest = new PutObjectRequest(request.Bucket, objectName, request.Content);

            var objectResult = ossClient.PutObject(putObjectRequest);

            if (objectResult.IsSuccessful() && request.ExpirationTime.HasValue)
            {
                var putBuckerLifeRequest = new PutBucketLifecycleRequest(request.Bucket);

                var rule = new COSXML.Model.Tag.LifecycleConfiguration.Rule();
                rule.id = "lfiecycleConfigureId";
                rule.status = "Enabled"; //Enabled，Disabled

                rule.filter = new COSXML.Model.Tag.LifecycleConfiguration.Filter();
                // TODO: 需要测试
                rule.filter.prefix = objectName;

                putBuckerLifeRequest.SetRule(rule);

                ossClient.PutBucketLifecycle(putBuckerLifeRequest);
            }

            var ossObject = new OssObject(
               !objectPath.IsNullOrWhiteSpace()
                    ? objectName.Replace(objectPath, "")
                    : objectName,
                objectPath,
                objectResult.eTag,
                DateTime.Now,
                contentLength,
                DateTime.Now,
                new Dictionary<string, string>(),
                objectName.EndsWith("/") // 名称结尾是 / 符号的则为目录：https://cloud.tencent.com/document/product/436/13324
                )
            {
                FullName = objectName
            };

            if (!Equals(request.Content, Stream.Null))
            {
                request.Content.Seek(0, SeekOrigin.Begin);
                ossObject.SetContent(request.Content);
            }

            return ossObject;
        }

        public virtual async Task DeleteAsync(string name)
        {
            // 阿里云oss在控制台设置即可，无需改变
            var ossClient = await CreateClientAsync();

            if (BucketExists(ossClient, name))
            {
                var deleteBucketRequest = new DeleteBucketRequest(name);
                ossClient.DeleteBucket(deleteBucketRequest);
            }
        }

        public virtual async Task DeleteObjectAsync(GetOssObjectRequest request)
        {
            var ossClient = await CreateClientAsync();

            var objectPath = GetBasePath(request.Path);

            var objectName = objectPath.IsNullOrWhiteSpace()
                ? request.Object
                : objectPath + request.Object;

            if (BucketExists(ossClient, request.Bucket) &&
                ObjectExists(ossClient, request.Bucket, objectName))
            {
                var getBucketRequest = new GetBucketRequest(request.Bucket);

                var getBucketResult = ossClient.GetBucket(getBucketRequest);
                if (getBucketResult.listBucket.commonPrefixesList.Any() ||
                    getBucketResult.listBucket.contentsList.Any())
                {
                    throw new BusinessException(code: OssManagementErrorCodes.ObjectDeleteWithNotEmpty);
                }
                var deleteObjectRequest = new DeleteObjectRequest(request.Bucket, objectName);
                ossClient.DeleteObject(deleteObjectRequest);
            }
        }

        public virtual async Task<bool> ExistsAsync(string name)
        {
            var ossClient = await CreateClientAsync();

            return BucketExists(ossClient, name);
        }

        public virtual async Task<OssContainer> GetAsync(string name)
        {
            var ossClient = await CreateClientAsync();
            if (!BucketExists(ossClient, name))
            {
                throw new BusinessException(code: OssManagementErrorCodes.ContainerNotFound);
                // throw new ContainerNotFoundException($"Can't not found container {name} in aliyun blob storing");
            }
            var getBucketRequest = new GetBucketRequest(name);
            var bucket = ossClient.GetBucket(getBucketRequest);

            return new OssContainer(
                bucket.Key,
                new DateTime(1970, 1, 1, 0, 0, 0), // TODO: 从header获取? 需要测试
                0L,
                null,
                new Dictionary<string, string>
                {
                    { "Id", bucket.Key },
                    { "DisplayName", bucket.Key }
                });
        }

        public virtual async Task<OssObject> GetObjectAsync(GetOssObjectRequest request)
        {
            var ossClient = await CreateClientAsync();
            if (!BucketExists(ossClient, request.Bucket))
            {
                throw new BusinessException(code: OssManagementErrorCodes.ContainerNotFound);
                // throw new ContainerNotFoundException($"Can't not found container {request.Bucket} in aliyun blob storing");
            }

            var objectPath = GetBasePath(request.Path);
            var objectName = objectPath.IsNullOrWhiteSpace()
                ? request.Object
                : objectPath + request.Object;

            if (!ObjectExists(ossClient, request.Bucket, objectName))
            {
                throw new BusinessException(code: OssManagementErrorCodes.ObjectNotFound);
                // throw new ContainerNotFoundException($"Can't not found object {objectName} in container {request.Bucket} with aliyun blob storing");
            }

            var getObjectRequest = new GetObjectBytesRequest(request.Bucket, objectName);
            if (!request.Process.IsNullOrWhiteSpace())
            {
                getObjectRequest.SetQueryParameter(request.Process, null);
            }
            var objectResult = ossClient.GetObject(getObjectRequest);
            var ossObject = new OssObject(
                !objectPath.IsNullOrWhiteSpace()
                    ? objectResult.Key.Replace(objectPath, "")
                    : objectResult.Key,
                request.Path,
                objectResult.eTag,
                null,
                objectResult.content.Length,
                null,
                new Dictionary<string, string>(),
                objectResult.Key.EndsWith("/"))
            {
                FullName = objectResult.Key
            };

            if (objectResult.content.Length > 0)
            {
                var memoryStream = new MemoryStream();
                await memoryStream.WriteAsync(objectResult.content, 0, objectResult.content.Length);
                memoryStream.Seek(0, SeekOrigin.Begin);
                ossObject.SetContent(memoryStream);
            }

            return ossObject;
        }

        public virtual async Task<GetOssContainersResponse> GetListAsync(GetOssContainersRequest request)
        {
            var ossClient = await CreateClientAsync();

            // TODO: 腾讯云直接返回所有列表?
            var getBucketRequest = new GetServiceRequest();
            var bucket = ossClient.GetService(getBucketRequest);

            return new GetOssContainersResponse(
                request.Prefix,
                request.Marker,
                null,
                bucket.listAllMyBuckets.buckets.Count,
                bucket.listAllMyBuckets.buckets
                       .Select(x => new OssContainer(
                           x.name,
                           DateTime.TryParse(x.createDate, out var time) ? time : new DateTime(1970, 1, 1),
                           0L,
                           null,
                           new Dictionary<string, string>
                           {
                               { "Id", x.name },
                               { "DisplayName", x.name }
                           }))
                       .ToList());
        }

        public virtual async Task<GetOssObjectsResponse> GetObjectsAsync(GetOssObjectsRequest request)
        {
            
            var ossClient = await CreateClientAsync();

            var objectPath = GetBasePath(request.Prefix);
            var marker = !objectPath.IsNullOrWhiteSpace() && !request.Marker.IsNullOrWhiteSpace()
                ? request.Marker.Replace(objectPath, "")
                : request.Marker;

            // TODO: 阿里云的分页差异需要前端来弥补,传递Marker, 按照Oss控制台的逻辑,直接把MaxKeys设置较大值就行了

            var getBucketRequest = new GetBucketRequest(request.BucketName);
            getBucketRequest.SetMarker(!marker.IsNullOrWhiteSpace() ? objectPath + marker : marker);
            getBucketRequest.SetMaxKeys(request.MaxKeys?.ToString() ?? "10");
            getBucketRequest.SetPrefix(objectPath);
            getBucketRequest.SetDelimiter(request.Delimiter);
            getBucketRequest.SetEncodingType(request.EncodingType);

            var getBucketResult = ossClient.GetBucket(getBucketRequest);

            var ossObjects = getBucketResult.listBucket.contentsList
                               .Where(x => !x.key.Equals(objectPath))// 过滤当前的目录返回值
                               .Select(x => new OssObject(
                                   !objectPath.IsNullOrWhiteSpace() && !x.key.Equals(objectPath)
                                    ? x.key.Replace(objectPath, "")
                                    : x.key, // 去除目录名称
                                   request.Prefix,
                                   x.eTag,
                                   DateTime.TryParse(x.lastModified, out var ctime) ? ctime : null,
                                   x.size,
                                   DateTime.TryParse(x.lastModified, out var mtime) ? mtime : null,
                                   new Dictionary<string, string>
                                   {
                                       { "Id", x.key },
                                       { "DisplayName", x.key }
                                   },
                                   x.key.EndsWith("/"))
                               {
                                   FullName = x.key
                               })
                               .ToList();
            // 当 Delimiter 为 / 时, objectsResponse.CommonPrefixes 可用于代表层级目录
            if (getBucketResult.listBucket.commonPrefixesList.Any())
            {
                ossObjects.InsertRange(0,
                    getBucketResult.listBucket.commonPrefixesList
                        .Select(x => new OssObject(
                            x.prefix.Replace(objectPath, ""),
                            request.Prefix,
                            "",
                            null,
                            0L,
                            null,
                            null,
                            true)));
            }
            // 排序
            // TODO: 是否需要客户端来排序
            ossObjects.Sort(new OssObjectComparer());

            return new GetOssObjectsResponse(
                getBucketResult.Key,
                request.Prefix,
                marker,
                !objectPath.IsNullOrWhiteSpace() && !getBucketResult.listBucket.nextMarker.IsNullOrWhiteSpace()
                    ? getBucketResult.listBucket.nextMarker.Replace(objectPath, "")
                    : getBucketResult.listBucket.nextMarker,
                getBucketResult.listBucket.delimiter,
                getBucketResult.listBucket.maxKeys,
                ossObjects);
        }

        protected virtual string GetBasePath(string path)
        {
            string objectPath = "";
            if (CurrentTenant.Id == null)
            {
                objectPath += "host/";
            }
            else
            {
                objectPath += "tenants/" + CurrentTenant.Id.Value.ToString("D");
            }

            objectPath += path ?? "";

            return objectPath.EnsureEndsWith('/');
        }

        protected virtual bool BucketExists(CosXml cos, string bucketName)
        {
            var request = new DoesBucketExistRequest(bucketName);
            return cos.DoesBucketExist(request);
        }

        protected virtual bool ObjectExists(CosXml cos, string bucketName, string objectName)
        {
            var request = new DoesObjectExistRequest(bucketName, objectName);
            return cos.DoesObjectExist(request);
        }

        protected virtual async Task<CosXml> CreateClientAsync()
        {
            return await CosClientFactory.CreateAsync<AbpOssManagementContainer>();
        }
    }
}
