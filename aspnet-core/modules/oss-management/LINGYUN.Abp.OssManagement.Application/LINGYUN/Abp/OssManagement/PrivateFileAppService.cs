using LINGYUN.Abp.Features.LimitValidation;
using LINGYUN.Abp.OssManagement.Features;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Caching;
using Volo.Abp.Content;
using Volo.Abp.Features;
using Volo.Abp.IO;
using Volo.Abp.Users;

namespace LINGYUN.Abp.OssManagement
{
    /// <summary>
    /// 所有登录用户访问私有文件服务接口
    /// bucket限制在users
    /// path限制在用户id
    /// </summary>
    [Authorize]
    // 不对外公开，仅通过控制器调用
    //[RemoteService(IsMetadataEnabled = false)]
    public class PrivateFileAppService : FileAppServiceBase, IPrivateFileAppService
    {
        private readonly IDistributedCache<FileShareCacheItem> _shareCache;
        private readonly IDistributedCache<MyFileShareCacheItem> _myShareCache;
        public PrivateFileAppService(
             IDistributedCache<FileShareCacheItem> shareCache,
             IDistributedCache<MyFileShareCacheItem> myShareCache,
            IFileUploader fileUploader,
            IFileValidater fileValidater,
            IOssContainerFactory ossContainerFactory)
            : base(fileUploader, fileValidater, ossContainerFactory)
        {
            _shareCache = shareCache;
            _myShareCache = myShareCache;
        }

        [Authorize]
        [RequiresFeature(AbpOssManagementFeatureNames.OssObject.DownloadFile)]
        [RequiresLimitFeature(
            AbpOssManagementFeatureNames.OssObject.DownloadLimit,
            AbpOssManagementFeatureNames.OssObject.DownloadInterval,
            LimitPolicy.Month)]
        public override async Task<IRemoteStreamContent> GetAsync(GetPublicFileInput input)
        {
            var ossObjectRequest = new GetOssObjectRequest(
                 GetCurrentBucket(),
                 // 需要处理特殊字符
                 HttpUtility.UrlDecode(input.Name),
                 GetCurrentPath(HttpUtility.UrlDecode(input.Path)),
                 HttpUtility.UrlDecode(input.Process))
            {
                MD5 = true,
                // TODO: 用户访问自身目录可以实现无限制创建目录层级?
                CreatePathIsNotExists = true,
            };

            var ossContainer = OssContainerFactory.Create();
            var ossObject = await ossContainer.GetObjectAsync(ossObjectRequest);

            return new RemoteStreamContent(ossObject.Content);
        }

        [Authorize]
        public override async Task<ListResultDto<OssObjectDto>> GetListAsync(GetFilesInput input)
        {
            var ossContainer = OssContainerFactory.Create();
            var response = await ossContainer.GetObjectsAsync(
                GetCurrentBucket(),
                GetCurrentPath(HttpUtility.UrlDecode(input.Path)),
                skipCount: 0,
                maxResultCount: input.MaxResultCount,
                createPathIsNotExists: true); // TODO: 用户访问自身目录可以实现无限制创建目录层级?

            return new ListResultDto<OssObjectDto>(
                ObjectMapper.Map<List<OssObject>, List<OssObjectDto>>(response.Objects));
        }

        [Authorize]
        public override async Task<OssObjectDto> UploadAsync(UploadFileInput input)
        {
            return await base.UploadAsync(input);
        }

        [Authorize]
        public override async Task UploadAsync(UploadFileChunkInput input)
        {
            await base.UploadAsync(input);
        }

        [Authorize]
        public virtual async Task<ListResultDto<MyFileShareDto>> GetShareListAsync()
        {
            var cacheKey = MyFileShareCacheItem.CalculateCacheKey(CurrentUser.GetId());
            var cacheItem = await _myShareCache.GetAsync(cacheKey);
            if (cacheItem == null)
            {
                return new ListResultDto<MyFileShareDto>();
            }

            // 被动刷新用户共享缓存
            // 手动调用时清除一下应该被清理掉的缓存
            cacheItem.Items.RemoveAll(items => items.MaxAccessCount > 0 && items.AccessCount > items.MaxAccessCount);
            cacheItem.Items.RemoveAll(items => items.ExpirationTime < Clock.Now);

            DistributedCacheEntryOptions cacheOptions = null;
            var myShareCacheExpirationTime = cacheItem.GetLastExpirationTime();
            if (myShareCacheExpirationTime.HasValue)
            {
                cacheOptions = new DistributedCacheEntryOptions
                {
                    AbsoluteExpiration = DateTimeOffset.Now.Add(
                        Clock.Normalize(myShareCacheExpirationTime.Value) - Clock.Now),
                };
            }
            await _myShareCache.SetAsync(cacheKey, cacheItem, cacheOptions);

            return new ListResultDto<MyFileShareDto>(
                ObjectMapper.Map<List<FileShareCacheItem>, List<MyFileShareDto>>(cacheItem.Items));
        }

        [Authorize]
        public virtual async Task<FileShareDto> ShareAsync(FileShareInput input)
        {
            var ossObjectRequest = new GetOssObjectRequest(
                GetCurrentBucket(),
                // 需要处理特殊字符
                HttpUtility.UrlDecode(input.Name),
                GetCurrentPath(HttpUtility.UrlDecode(input.Path)))
            {
                MD5 = true,
            };
            var ossContainer = OssContainerFactory.Create();
            var ossObject = await ossContainer.GetObjectAsync(ossObjectRequest);

            var shareUrl = $"{GuidGenerator.Create():N}.{FileHelper.GetExtension(ossObject.Name)}";
            var cacheItem = new FileShareCacheItem(
                CurrentUser.GetId(),
                GetCurrentBucket(),
                ossObject.Name,
                ossObject.Prefix,
                ossObject.MD5,
                shareUrl,
                input.ExpirationTime ?? Clock.Now.AddDays(7),
                input.Roles,
                input.Users,
                input.MaxAccessCount);

            var cacheOptions = new DistributedCacheEntryOptions
            {
                // 不传递过期时间, 默认7天
                AbsoluteExpiration = DateTimeOffset.Now.Add(
                    Clock.Normalize(cacheItem.ExpirationTime) - Clock.Now),
            };

            await _shareCache.SetAsync(
                FileShareCacheItem.CalculateCacheKey(shareUrl),
                cacheItem,
                cacheOptions);

            #region 当前用户共享缓存

            // 被动刷新用户共享缓存
            var myShareCacheKey = MyFileShareCacheItem.CalculateCacheKey(CurrentUser.GetId());
            var myShareCacheItem = await _myShareCache.GetAsync(myShareCacheKey);
            if (myShareCacheItem == null)
            {
                myShareCacheItem = new MyFileShareCacheItem(
                    new List<FileShareCacheItem>() { cacheItem });
            }
            else
            {
                myShareCacheItem.Items.Add(cacheItem);
            }
            DistributedCacheEntryOptions myShareCacheOptions = null;
            var myShareCacheExpirationTime = myShareCacheItem.GetLastExpirationTime();
            if (myShareCacheExpirationTime.HasValue)
            {
                myShareCacheOptions = new DistributedCacheEntryOptions
                {
                    AbsoluteExpiration = DateTimeOffset.Now.Add(
                        Clock.Normalize(myShareCacheExpirationTime.Value) - Clock.Now),
                };
            }
            await _myShareCache.SetAsync(myShareCacheKey, myShareCacheItem, myShareCacheOptions);

            #endregion

            return new FileShareDto
            {
                Url = shareUrl,
                MaxAccessCount = input.MaxAccessCount,
                ExpirationTime = input.ExpirationTime,
            };
        }

        [Authorize]
        public override async Task DeleteAsync(GetPublicFileInput input)
        {
            await base.DeleteAsync(input);
        }

        protected override string GetCurrentBucket()
        {
            return "users";
        }

        protected override string GetCurrentPath(string path)
        {
            path = base.GetCurrentPath(path);
            var userId = CurrentUser.GetId().ToString("N");
            if (path.IsNullOrWhiteSpace())
            {
                return userId;
            }
            return path.StartsWith(userId) ? path : $"{userId}/{path}";
        }
    }
}
