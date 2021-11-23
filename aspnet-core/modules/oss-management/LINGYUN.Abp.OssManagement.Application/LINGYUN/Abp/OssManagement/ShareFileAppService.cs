using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Caching;

namespace LINGYUN.Abp.OssManagement
{
    public class ShareFileAppService : OssManagementApplicationServiceBase, IShareFileAppService
    {
        private readonly IDistributedCache<FileShareCacheItem> _shareCache;
        private readonly IDistributedCache<MyFileShareCacheItem> _myShareCache;
        private readonly IOssContainerFactory _ossContainerFactory;

        public ShareFileAppService(
            IDistributedCache<FileShareCacheItem> shareCache,
            IDistributedCache<MyFileShareCacheItem> myShareCache,
            IOssContainerFactory ossContainerFactory)
        {
            _shareCache = shareCache;
            _myShareCache = myShareCache;
            _ossContainerFactory = ossContainerFactory;
        }

        public virtual async Task<GetFileShareDto> GetAsync(string url)
        {
            var cacheKey = FileShareCacheItem.CalculateCacheKey(url);
            var cacheItem = await _shareCache.GetAsync(cacheKey);
            if (cacheItem == null)
            {
                return new GetFileShareDto(url);
            }

            // 最大访问次数
            cacheItem.AccessCount += 1;

            // 被动刷新用户共享缓存
            await RefreshUserShareAsync(cacheItem);

            if (cacheItem.MaxAccessCount > 0 && cacheItem.AccessCount > cacheItem.MaxAccessCount)
            {
                await _shareCache.RemoveAsync(cacheKey);

                return new GetFileShareDto(url);
            }

            // 共享用户
            if (cacheItem.Users != null && cacheItem.Users.Any())
            {
                if (cacheItem.Users.Any((userName) => !userName.Equals(CurrentUser.UserName)))
                {
                    return new GetFileShareDto(url);
                }
            }

            // 共享角色
            if (cacheItem.Roles != null && cacheItem.Roles.Any())
            {
                if (cacheItem.Roles.Any((role) => !CurrentUser.Roles.Contains(role)))
                {
                    return new GetFileShareDto(url);
                }
            }

            var ossObjectRequest = new GetOssObjectRequest(
                cacheItem.Bucket,
                cacheItem.Name,
                cacheItem.Path)
            {
                MD5 = true,
            };

            var ossContainer = _ossContainerFactory.Create();
            var ossObject = await ossContainer.GetObjectAsync(ossObjectRequest);

            var cacheOptions = new DistributedCacheEntryOptions
            {
                // 不传递过期时间, 默认7天
                AbsoluteExpiration = DateTimeOffset.Now.Add(
                    Clock.Normalize(cacheItem.ExpirationTime) - Clock.Now),
            };
            // 改变共享次数
            await _shareCache.SetAsync(
                cacheKey,
                cacheItem,
                cacheOptions);

            return new GetFileShareDto(ossObject.Name, ossObject.Content);
        }

        protected virtual async Task RefreshUserShareAsync(FileShareCacheItem shareCacheItem)
        {
            // 清除当前用户共享缓存
            var myShareCacheKey = MyFileShareCacheItem.CalculateCacheKey(shareCacheItem.UserId);
            var myShareCacheItem = await _myShareCache.GetAsync(myShareCacheKey);
            if (myShareCacheItem != null)
            {
                myShareCacheItem.Items.RemoveAll(item => item.Url.Equals(shareCacheItem.Url));
                if (shareCacheItem.MaxAccessCount == 0 || shareCacheItem.AccessCount < shareCacheItem.MaxAccessCount)
                {
                    myShareCacheItem.Items.Add(shareCacheItem);
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
            }
        }
    }
}
