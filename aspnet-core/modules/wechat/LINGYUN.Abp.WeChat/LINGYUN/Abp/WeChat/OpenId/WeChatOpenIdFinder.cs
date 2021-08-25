using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Authorization;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Users;

namespace LINGYUN.Abp.WeChat.OpenId
{
    [Dependency(ServiceLifetime.Singleton, ReplaceServices = true)]
    [ExposeServices(typeof(IWeChatOpenIdFinder))]
    public class WeChatOpenIdFinder : IWeChatOpenIdFinder
    {
        public ILogger<WeChatOpenIdFinder> Logger { get; set; }
        protected ICurrentTenant CurrentTenant { get; }
        protected ICurrentUser CurrentUser { get; }
        protected IHttpClientFactory HttpClientFactory { get; }
        protected IDistributedCache<WeChatOpenIdCacheItem> Cache { get; }
        public WeChatOpenIdFinder(
            ICurrentUser currentUser,
            ICurrentTenant currentTenant,
            IHttpClientFactory httpClientFactory,
            IDistributedCache<WeChatOpenIdCacheItem> cache)
        {
            CurrentUser = currentUser;
            CurrentTenant = currentTenant;
            HttpClientFactory = httpClientFactory;

            Cache = cache;

            Logger = NullLogger<WeChatOpenIdFinder>.Instance;
        }

        public virtual async Task<WeChatOpenId> FindAsync(string appId)
        {
            if (!CurrentUser.IsAuthenticated)
            {
                throw new AbpAuthorizationException("Try to get wechat information when the user is not logged in!");
            }
            var cacheKey = WeChatOpenIdCacheItem.CalculateCacheKey(appId, CurrentUser.Id.Value);
            var openIdCache = await Cache.GetAsync(cacheKey);
            return openIdCache?.WeChatOpenId ??
                throw new AbpException("The wechat login session has expired. Use 'wx.login' result code to exchange the sessionKey");
        }

        public virtual async Task<WeChatOpenId> FindAsync(string code, string appId, string appSecret)
        {
            // TODO: 如果需要获取SessionKey的话呢，需要再以openid作为标识来缓存一下吗
            // 或者前端保存code,通过传递code来获取
            return (await GetCacheItemAsync(code, appId, appSecret)).WeChatOpenId;
        }

        protected virtual async Task<WeChatOpenIdCacheItem> GetCacheItemAsync(string code, string appId, string appSecret)
        {
            var cacheKey = WeChatOpenIdCacheItem.CalculateCacheKey(appId, code);

            Logger.LogDebug($"WeChatOpenIdFinder.GetCacheItemAsync: {cacheKey}");

            var cacheItem = await Cache.GetAsync(cacheKey);

            if (cacheItem != null)
            {
                Logger.LogDebug($"Found in the cache: {cacheKey}");
                return cacheItem;
            }

            Logger.LogDebug($"Not found in the cache, getting from the httpClient: {cacheKey}");

            var client = HttpClientFactory.CreateClient(AbpWeChatGlobalConsts.HttpClient);

            var request = new WeChatOpenIdRequest
            {
                BaseUrl = client.BaseAddress.AbsoluteUri,
                AppId = appId,
                Secret = appSecret,
                Code = code
            };

            var response = await client.RequestWeChatOpenIdAsync(request);
            var responseContent = await response.Content.ReadAsStringAsync();
            // 改为直接引用 Newtownsoft.Json
            var weChatOpenIdResponse = JsonConvert.DeserializeObject<WeChatOpenIdResponse>(responseContent);
            var weChatOpenId = weChatOpenIdResponse.ToWeChatOpenId();
            cacheItem = new WeChatOpenIdCacheItem(code, weChatOpenId);

            Logger.LogDebug($"Setting the cache item: {cacheKey}");

            var cacheOptions = new DistributedCacheEntryOptions
            {
                // 微信官方文档表示 session_key的有效期是3天
                // https://developers.weixin.qq.com/community/develop/doc/000c2424654c40bd9c960e71e5b009
                AbsoluteExpiration = DateTimeOffset.Now.AddDays(3).AddSeconds(-120)
                // SlidingExpiration = TimeSpan.FromDays(3),
            };


            await Cache.SetAsync(cacheKey, cacheItem, cacheOptions);

            if (CurrentUser.IsAuthenticated)
            {
                await Cache.SetAsync(WeChatOpenIdCacheItem.CalculateCacheKey(appId, CurrentUser.Id.Value), cacheItem, cacheOptions);
            }

            Logger.LogDebug($"Finished setting the cache item: {cacheKey}");

            return cacheItem;
        }
    }
}
