using LINGYUN.Abp.OssManagement.Permissions;
using Microsoft.AspNetCore.Authorization;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Web;
using Volo.Abp;
using Volo.Abp.Caching;
using Volo.Abp.Content;
using Volo.Abp.Json;

namespace LINGYUN.Abp.OssManagement;

public class OssObjectAppService : OssManagementApplicationServiceBase, IOssObjectAppService
{
    protected FileUploadMerger Merger { get; }
    protected IOssContainerFactory OssContainerFactory { get; }
    protected IDistributedCache<OssObjectUrlCacheItem> UrlCache { get; }
    protected IJsonSerializer JsonSerializer { get; }

    public OssObjectAppService(
        FileUploadMerger merger,
        IJsonSerializer jsonSerializer,
        IOssContainerFactory ossContainerFactory,
        IDistributedCache<OssObjectUrlCacheItem> urlCache)
    {
        Merger = merger;
        JsonSerializer = jsonSerializer;
        OssContainerFactory = ossContainerFactory;
        UrlCache = urlCache;
    }

    [Authorize(AbpOssManagementPermissions.OssObject.Create)]
    public async virtual Task<OssObjectDto> CreateAsync(CreateOssObjectInput input)
    {
        // 内容为空时建立目录
        if (input.File == null || !input.File.ContentLength.HasValue)
        {
            var oss = CreateOssContainer();
            var request = new CreateOssObjectRequest(
                HttpUtility.UrlDecode(input.Bucket),
                HttpUtility.UrlDecode(input.FileName),
                Stream.Null,
                HttpUtility.UrlDecode(input.Path));
            var ossObject = await oss.CreateObjectAsync(request);

            return ObjectMapper.Map<OssObject, OssObjectDto>(ossObject);
        } 
        else
        {
            var ossObject = await Merger.MergeAsync(input);

            return ObjectMapper.Map<OssObject, OssObjectDto>(ossObject);
        }
    }

    [Authorize(AbpOssManagementPermissions.OssObject.Delete)]
    public async virtual Task BulkDeleteAsync(BulkDeleteOssObjectInput input)
    {
        var oss = CreateOssContainer();

        await oss.BulkDeleteObjectsAsync(input.Bucket, input.Objects, input.Path);
    }

    [Authorize(AbpOssManagementPermissions.OssObject.Delete)]
    public async virtual Task DeleteAsync(GetOssObjectInput input)
    {
        var oss = CreateOssContainer();

        await oss.DeleteObjectAsync(input.Bucket, input.Object, input.Path);
    }

    [Authorize(AbpOssManagementPermissions.OssObject.Default)]
    public async virtual Task<OssObjectDto> GetAsync(GetOssObjectInput input)
    {
        var oss = CreateOssContainer();

        var ossObject = await oss.GetObjectAsync(input.Bucket, input.Object, input.Path, input.MD5);

        return ObjectMapper.Map<OssObject, OssObjectDto>(ossObject);
    }

    [Authorize(AbpOssManagementPermissions.OssObject.Download)]
    public async virtual Task<IRemoteStreamContent> GetContentAsync(GetOssObjectInput input)
    {
        var oss = CreateOssContainer();

        var ossObject = await oss.GetObjectAsync(input.Bucket, input.Object, input.Path, input.MD5);

        return new RemoteStreamContent(ossObject.Content, ossObject.Name);
    }

    protected virtual IOssContainer CreateOssContainer()
    {
        return OssContainerFactory.Create();
    }

    [Authorize(AbpOssManagementPermissions.OssObject.Download)]
    public async virtual Task<string> GenerateUrlAsync(GetOssObjectInput input)
    {
        var cacheKey = JsonSerializer.Serialize(input).ToMd5();
        var cacheItem = await UrlCache.GetAsync(cacheKey);
        if (cacheItem == null)
        {
            var url = $"/api/oss-management/objects/download/{cacheKey}"; 
            cacheItem = new OssObjectUrlCacheItem(url, input.Bucket, input.Path, input.Object);

            await UrlCache.SetAsync(cacheKey, cacheItem);
        }

        return cacheItem.Url;
    }

    public async virtual Task<IRemoteStreamContent> DownloadAsync(string urlKey)
    {
        var cacheItem = await UrlCache.GetAsync(urlKey) ?? 
            throw new BusinessException(OssManagementErrorCodes.ObjectUrlKeyHasExpired);

        await UrlCache.RemoveAsync(urlKey);

        var oss = CreateOssContainer();

        var ossObject = await oss.GetObjectAsync(cacheItem.Bucket, cacheItem.Object, cacheItem.Path);

        return new RemoteStreamContent(ossObject.Content, ossObject.Name);
    }
}
