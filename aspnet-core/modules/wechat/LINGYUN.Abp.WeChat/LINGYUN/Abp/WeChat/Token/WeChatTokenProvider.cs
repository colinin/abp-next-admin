using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.WeChat.Token
{
    public class WeChatTokenProvider : IWeChatTokenProvider, ISingletonDependency
    {
        public ILogger<WeChatTokenProvider> Logger { get; set; }
        protected IHttpClientFactory HttpClientFactory { get; }
        protected IDistributedCache<WeChatTokenCacheItem> Cache { get; }
        public WeChatTokenProvider(
            IHttpClientFactory httpClientFactory,
            IDistributedCache<WeChatTokenCacheItem> cache)
        {
            HttpClientFactory = httpClientFactory;

            Cache = cache;

            Logger = NullLogger<WeChatTokenProvider>.Instance;
        }

        public virtual async Task<WeChatToken> GetTokenAsync(
            string appId, 
            string appSecret, 
            CancellationToken cancellationToken = default)
        {
            return (await GetCacheItemAsync("WeChatToken", appId, appSecret, cancellationToken)).WeChatToken;
        }

        protected virtual async Task<WeChatTokenCacheItem> GetCacheItemAsync(
            string provider, 
            string appId,
            string appSecret, 
            CancellationToken cancellationToken = default)
        {
            var cacheKey = WeChatTokenCacheItem.CalculateCacheKey(provider, appId);

            Logger.LogDebug($"WeChatTokenProvider.GetCacheItemAsync: {cacheKey}");

            var cacheItem = await Cache.GetAsync(cacheKey, token: cancellationToken);

            if (cacheItem != null)
            {
                Logger.LogDebug($"Found in the cache: {cacheKey}");
                return cacheItem;
            }

            Logger.LogDebug($"Not found in the cache, getting from the httpClient: {cacheKey}");

            var client = HttpClientFactory.CreateClient(AbpWeChatGlobalConsts.HttpClient);

            var request = new WeChatTokenRequest
            {
                BaseUrl = client.BaseAddress.AbsoluteUri,
                AppSecret = appSecret,
                AppId = appId,
                GrantType = "client_credential"
            };

            var response = await client.RequestWeChatCodeTokenAsync(request, cancellationToken);
            var responseContent = await response.Content.ReadAsStringAsync();
            // 改为直接引用 Newtownsoft.Json
            var weChatTokenResponse = JsonConvert.DeserializeObject<WeChatTokenResponse>(responseContent);
            var weChatToken = weChatTokenResponse.ToWeChatToken();
            cacheItem = new WeChatTokenCacheItem(appId, weChatToken);

            Logger.LogDebug($"Setting the cache item: {cacheKey}");

            var cacheOptions = new DistributedCacheEntryOptions
            {
                // 设置绝对过期时间为Token有效期剩余的二分钟
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(weChatToken.ExpiresIn - 120)
            };

            await Cache.SetAsync(cacheKey, cacheItem, cacheOptions, token: cancellationToken);

            Logger.LogDebug($"Finished setting the cache item: {cacheKey}");

            return cacheItem;
        }
    }
}
