using Aliyun.OSS;
using LINGYUN.Abp.BlobStoring.Aliyun;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Abp.OssManagement.Aliyun
{
    /// <summary>
    /// Oss容器的阿里云实现
    /// </summary>
    internal class AliyunOssContainer : IOssContainer
    {
        protected ICurrentTenant CurrentTenant { get; }
        protected IOssClientFactory OssClientFactory { get; }
        public AliyunOssContainer(
            ICurrentTenant currentTenant,
            IOssClientFactory ossClientFactory)
        {
            CurrentTenant = currentTenant;
            OssClientFactory = ossClientFactory;
        }
        public virtual async Task BulkDeleteObjectsAsync(BulkDeleteObjectRequest request)
        {
            var ossClient = await CreateClientAsync();

            var path = GetBasePath(request.Path);
            var aliyunRequest = new DeleteObjectsRequest(request.Bucket, request.Objects.Select(x => x += path).ToList());

            ossClient.DeleteObjects(aliyunRequest);
        }

        public virtual async Task<OssContainer> CreateAsync(string name)
        {
            var ossClient = await CreateClientAsync();

            if (BucketExists(ossClient, name))
            {
                throw new BusinessException(code: OssManagementErrorCodes.ContainerAlreadyExists);
            }

            var bucket = ossClient.CreateBucket(name);

            return new OssContainer(
                bucket.Name,
                bucket.CreationDate,
                0L,
                bucket.CreationDate,
                new Dictionary<string, string>
                {
                    { "Id", bucket.Owner?.Id },
                    { "DisplayName", bucket.Owner?.DisplayName }
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
                ossClient.CreateBucket(request.Bucket);
            }

            var aliyunObjectRequest = new PutObjectRequest(request.Bucket, objectName, request.Content)
            {
                Metadata = new ObjectMetadata()
            };
            if (request.ExpirationTime.HasValue)
            {
                aliyunObjectRequest.Metadata.ExpirationTime = DateTime.Now.Add(request.ExpirationTime.Value);
            }

            var aliyunObject = ossClient.PutObject(aliyunObjectRequest);

            var ossObject = new OssObject(
               !objectPath.IsNullOrWhiteSpace()
                    ? objectName.Replace(objectPath, "")
                    : objectName,
                objectPath,
                aliyunObject.ETag,
                DateTime.Now,
                aliyunObject.ContentLength,
                DateTime.Now,
                aliyunObject.ResponseMetadata,
                objectName.EndsWith("/") // 名称结尾是 / 符号的则为目录：https://help.aliyun.com/document_detail/31910.html
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
                ossClient.DeleteBucket(name);
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
                var objectListing = ossClient.ListObjects(request.Bucket, objectName);
                if (objectListing.CommonPrefixes.Any() ||
                    objectListing.ObjectSummaries.Any())
                {
                    throw new BusinessException(code: OssManagementErrorCodes.ObjectDeleteWithNotEmpty);
                    // throw new ObjectDeleteWithNotEmptyException("00201", $"Can't not delete oss object {request.Object}, because it is not empty!");
                }
                ossClient.DeleteObject(request.Bucket, objectName);
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
            var bucket = ossClient.GetBucketInfo(name);

            return new OssContainer(
                bucket.Bucket.Name,
                bucket.Bucket.CreationDate,
                0L,
                bucket.Bucket.CreationDate,
                new Dictionary<string, string>
                {
                    { "Id", bucket.Bucket.Owner?.Id },
                    { "DisplayName", bucket.Bucket.Owner?.DisplayName }
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

            var aliyunOssObjectRequest = new GetObjectRequest(request.Bucket, objectName, request.Process);
            var aliyunOssObject = ossClient.GetObject(aliyunOssObjectRequest);
            var ossObject = new OssObject(
                !objectPath.IsNullOrWhiteSpace()
                    ? aliyunOssObject.Key.Replace(objectPath, "")
                    : aliyunOssObject.Key,
                request.Path,
                aliyunOssObject.Metadata.ETag,
                aliyunOssObject.Metadata.LastModified,
                aliyunOssObject.Metadata.ContentLength,
                aliyunOssObject.Metadata.LastModified,
                aliyunOssObject.Metadata.UserMetadata,
                aliyunOssObject.Key.EndsWith("/"))
            {
                FullName = aliyunOssObject.Key
            };

            if (aliyunOssObject.IsSetResponseStream())
            {
                var memoryStream = new MemoryStream();
                await aliyunOssObject.Content.CopyToAsync(memoryStream);
                memoryStream.Seek(0, SeekOrigin.Begin);
                ossObject.SetContent(memoryStream);
            }

            return ossObject;
        }

        public virtual async Task<GetOssContainersResponse> GetListAsync(GetOssContainersRequest request)
        {
            var ossClient = await CreateClientAsync();

            // TODO: 阿里云的分页差异需要前端来弥补,传递Marker, 按照Oss控制台的逻辑,直接把MaxKeys设置较大值就行了
            var aliyunRequest = new ListBucketsRequest
            {
                Marker = request.Marker,
                Prefix = request.Prefix,
                MaxKeys = request.MaxKeys
            };
            var bucketsResponse = ossClient.ListBuckets(aliyunRequest);

            return new GetOssContainersResponse(
                bucketsResponse.Prefix,
                bucketsResponse.Marker,
                bucketsResponse.NextMaker,
                bucketsResponse.MaxKeys ?? 0,
                bucketsResponse.Buckets
                       .Select(x => new OssContainer(
                           x.Name,
                           x.CreationDate,
                           0L,
                           x.CreationDate,
                           new Dictionary<string, string>
                           {
                               { "Id", x.Owner?.Id },
                               { "DisplayName", x.Owner?.DisplayName }
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
            var aliyunRequest = new ListObjectsRequest(request.BucketName)
            {
                Marker = !marker.IsNullOrWhiteSpace() ? objectPath + marker : marker,
                Prefix = objectPath,
                MaxKeys = request.MaxKeys,
                EncodingType = request.EncodingType,
                Delimiter = request.Delimiter
            };
            var objectsResponse = ossClient.ListObjects(aliyunRequest);

            var ossObjects = objectsResponse.ObjectSummaries
                               .Where(x => !x.Key.Equals(objectsResponse.Prefix))// 过滤当前的目录返回值
                               .Select(x => new OssObject(
                                   !objectPath.IsNullOrWhiteSpace() && !x.Key.Equals(objectPath)
                                    ? x.Key.Replace(objectPath, "")
                                    : x.Key, // 去除目录名称
                                   request.Prefix,
                                   x.ETag,
                                   x.LastModified,
                                   x.Size,
                                   x.LastModified,
                                   new Dictionary<string, string>
                                   {
                                       { "Id", x.Owner?.Id },
                                       { "DisplayName", x.Owner?.DisplayName }
                                   },
                                   x.Key.EndsWith("/"))
                               {
                                   FullName = x.Key
                               })
                               .ToList();
            // 当 Delimiter 为 / 时, objectsResponse.CommonPrefixes 可用于代表层级目录
            if (objectsResponse.CommonPrefixes.Any())
            {
                ossObjects.InsertRange(0,
                    objectsResponse.CommonPrefixes
                        .Select(x => new OssObject(
                            x.Replace(objectPath, ""),
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
                objectsResponse.BucketName,
                request.Prefix,
                marker,
                !objectPath.IsNullOrWhiteSpace() && !objectsResponse.NextMarker.IsNullOrWhiteSpace()
                    ? objectsResponse.NextMarker.Replace(objectPath, "")
                    : objectsResponse.NextMarker,
                objectsResponse.Delimiter,
                objectsResponse.MaxKeys,
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

        protected virtual bool BucketExists(IOss client, string bucketName)
        {
            return client.DoesBucketExist(bucketName);
        }

        protected virtual bool ObjectExists(IOss client, string bucketName, string objectName)
        {
            return client.DoesObjectExist(bucketName, objectName);
        }

        protected virtual async Task<IOss> CreateClientAsync()
        {
            return await OssClientFactory.CreateAsync<AbpOssManagementContainer>();
        }
    }
}
