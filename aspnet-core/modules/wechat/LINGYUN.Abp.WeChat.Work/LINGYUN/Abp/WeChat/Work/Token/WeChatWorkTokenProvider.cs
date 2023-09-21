using LINGYUN.Abp.WeChat.Work.Settings;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Settings;

namespace LINGYUN.Abp.WeChat.Work.Token
{
    public class WeChatWorkTokenProvider : IWeChatWorkTokenProvider, ISingletonDependency
    {
        public ILogger<WeChatWorkTokenProvider> Logger { get; set; }
        protected ISettingProvider SettingProvider { get; }
        protected IHttpClientFactory HttpClientFactory { get; }
        protected IDistributedCache<WeChatWorkTokenCacheItem> Cache { get; }
        protected WeChatWorkOptions WeChatWorkOptions { get; }
        public WeChatWorkTokenProvider(
            ISettingProvider settingProvider,
            IHttpClientFactory httpClientFactory,
            IDistributedCache<WeChatWorkTokenCacheItem> cache,
            IOptionsMonitor<WeChatWorkOptions> weChatWorkOptions)
        {
            HttpClientFactory = httpClientFactory;
            SettingProvider = settingProvider;
            Cache = cache;
            WeChatWorkOptions = weChatWorkOptions.CurrentValue;

            Logger = NullLogger<WeChatWorkTokenProvider>.Instance;
        }

        public async virtual Task<WeChatWorkToken> GetTokenAsync(string agentId, CancellationToken cancellationToken = default)
        {
            var corpId = await SettingProvider.GetOrNullAsync(WeChatWorkSettingNames.Connection.CorpId);

            return await GetTokenAsync(corpId, agentId, cancellationToken);
        }

        public async virtual Task<WeChatWorkToken> GetTokenAsync(
            string corpId, 
            string agentId, 
            CancellationToken cancellationToken = default)
        {
            return (await GetCacheItemAsync("WeChatWorkToken", corpId, agentId, cancellationToken)).Token;
        }

        protected async virtual Task<WeChatWorkTokenCacheItem> GetCacheItemAsync(
            string provider, 
            string corpId,
            string agentId, 
            CancellationToken cancellationToken = default)
        {
            Check.NotNullOrEmpty(corpId, nameof(corpId));
            Check.NotNullOrEmpty(agentId, nameof(agentId));

            var cacheKey = WeChatWorkTokenCacheItem.CalculateCacheKey(provider, corpId, agentId);

            Logger.LogDebug($"WeChatWorkTokenProvider.GetCacheItemAsync: {cacheKey}");

            var cacheItem = await Cache.GetAsync(cacheKey, token: cancellationToken);

            if (cacheItem != null)
            {
                Logger.LogDebug($"Found WeChatWorkToken in the cache: {cacheKey}");
                return cacheItem;
            }

            Logger.LogDebug($"Not found WeChatWorkToken in the cache, getting from the httpClient: {cacheKey}");

            var client = HttpClientFactory.CreateClient(AbpWeChatWorkGlobalConsts.ApiClient);
            var applicationConfiguration = WeChatWorkOptions.Applications.GetConfiguration(agentId);

            var request = new WeChatWorkTokenRequest
            {
                CorpId = corpId,
                CorpSecret = applicationConfiguration.Secret,
            };

            using var response = await client.GetTokenAsync(request, cancellationToken);
            var responseContent = await response.Content.ReadAsStringAsync();
            // 改为直接引用 Newtownsoft.Json
            var tokenResponse = JsonConvert.DeserializeObject<WeChatWorkTokenResponse>(responseContent);
            var token = tokenResponse.ToWeChatWorkToken();
            cacheItem = new WeChatWorkTokenCacheItem(corpId, agentId, token);

            Logger.LogDebug($"Setting the cache item: {cacheKey}");

            var cacheOptions = new DistributedCacheEntryOptions
            {
                // 设置绝对过期时间为Token有效期剩余的二分钟
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(token.ExpiresIn - 120),
            };

            await Cache.SetAsync(cacheKey, cacheItem, cacheOptions, token: cancellationToken);

            Logger.LogDebug($"Finished setting the cache item: {cacheKey}");

            return cacheItem;
        }
    }
}
