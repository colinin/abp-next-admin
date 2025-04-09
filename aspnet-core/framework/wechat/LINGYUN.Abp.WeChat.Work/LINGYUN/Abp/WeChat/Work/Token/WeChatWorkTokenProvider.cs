using LINGYUN.Abp.WeChat.Work.Settings;
using LINGYUN.Abp.WeChat.Work.Token.Models;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Settings;

namespace LINGYUN.Abp.WeChat.Work.Token;

public class WeChatWorkTokenProvider : IWeChatWorkTokenProvider, ISingletonDependency
{
    public ILogger<WeChatWorkTokenProvider> Logger { get; set; }
    protected ISettingProvider SettingProvider { get; }
    protected IHttpClientFactory HttpClientFactory { get; }
    protected IDistributedCache<WeChatWorkTokenCacheItem> WeChatWorkTokenCache { get; }
    public WeChatWorkTokenProvider(
        ISettingProvider settingProvider,
        IHttpClientFactory httpClientFactory,
        IDistributedCache<WeChatWorkTokenCacheItem> cache)
    {
        HttpClientFactory = httpClientFactory;
        SettingProvider = settingProvider;
        WeChatWorkTokenCache = cache;

        Logger = NullLogger<WeChatWorkTokenProvider>.Instance;
    }

    public async virtual Task<WeChatWorkToken> GetTokenAsync(CancellationToken cancellationToken = default)
    {
        var corpId = await SettingProvider.GetOrNullAsync(WeChatWorkSettingNames.Connection.CorpId);
        var agentId = await SettingProvider.GetOrNullAsync(WeChatWorkSettingNames.Connection.AgentId);
        var secret = await SettingProvider.GetOrNullAsync(WeChatWorkSettingNames.Connection.Secret);

        return await GetTokenAsync(corpId, agentId, secret, cancellationToken);
    }

    public async virtual Task<WeChatWorkToken> GetTokenAsync(
        string corpId, 
        string agentId, 
        string secret,
        CancellationToken cancellationToken = default)
    {
        return (await GetCacheItemAsync("WeChatWorkToken", corpId, agentId, secret, cancellationToken)).Token;
    }
    /// <summary>
    /// 获取缓存中的Token配置
    /// </summary>
    /// <param name="provider"></param>
    /// <param name="corpId"></param>
    /// <param name="agentId"></param>
    /// <param name="secret"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    protected async virtual Task<WeChatWorkTokenCacheItem> GetCacheItemAsync(
        string provider, 
        string corpId,
        string agentId,
        string secret,
        CancellationToken cancellationToken = default)
    {
        Check.NotNullOrEmpty(corpId, nameof(corpId));
        Check.NotNullOrEmpty(agentId, nameof(agentId));
        Check.NotNullOrEmpty(secret, nameof(secret));

        var cacheKey = WeChatWorkTokenCacheItem.CalculateCacheKey(provider, corpId, agentId);

        return await GetCacheItemAsync(cacheKey, provider, corpId, agentId, secret);
    }
    /// <summary>
    /// 获取或刷新分布式缓存中的Token配置
    /// </summary>
    /// <param name="cacheKey"></param>
    /// <param name="provider"></param>
    /// <param name="corpId"></param>
    /// <param name="agentId"></param>
    /// <param name="secret"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    protected async virtual Task<WeChatWorkTokenCacheItem> GetCacheItemAsync(
        string cacheKey,
        string provider,
        string corpId,
        string agentId,
        string secret,
        CancellationToken cancellationToken = default)
    {
        var cacheItem = await WeChatWorkTokenCache.GetAsync(cacheKey, token: cancellationToken);

        if (cacheItem != null)
        {
            Logger.LogDebug($"Found WeChatWorkToken in the cache: {cacheKey}");
            return cacheItem;
        }

        Logger.LogDebug($"Not found WeChatWorkToken in the cache, getting from the httpClient: {cacheKey}");

        var client = HttpClientFactory.CreateClient(AbpWeChatWorkGlobalConsts.ApiClient);

        var request = new WeChatWorkTokenRequest
        {
            CorpId = corpId,
            CorpSecret = secret,
        };

        using var response = await client.GetTokenAsync(request, cancellationToken);
        var tokenResponse = await response.DeserializeObjectAsync<WeChatWorkTokenResponse>();
        var token = tokenResponse.ToWeChatWorkToken();
        cacheItem = new WeChatWorkTokenCacheItem(corpId, agentId, token);

        Logger.LogDebug($"Setting the cache item: {cacheKey}");

        var cacheOptions = new DistributedCacheEntryOptions
        {
            // 设置绝对过期时间为Token有效期剩余的二分钟
            AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(token.ExpiresIn - 100),
        };

        await WeChatWorkTokenCache.SetAsync(cacheKey, cacheItem, cacheOptions, token: cancellationToken);

        Logger.LogDebug($"Finished setting the cache item: {cacheKey}");

        return cacheItem;
    }
}
