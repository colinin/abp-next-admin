using LINGYUN.Abp.PushPlus.Settings;
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
using Volo.Abp.Json;
using Volo.Abp.Settings;

namespace LINGYUN.Abp.PushPlus.Token;

public class PushPlusTokenProvider : IPushPlusTokenProvider, ITransientDependency
{
    public ILogger<PushPlusTokenProvider> Logger { protected get; set; }

    protected IDistributedCache<PushPlusTokenCacheItem> Cache { get; }
    protected IJsonSerializer JsonSerializer { get; }
    protected ISettingProvider SettingProvider { get; }
    protected IHttpClientFactory HttpClientFactory { get; }

    public PushPlusTokenProvider(
        IDistributedCache<PushPlusTokenCacheItem> cache,
        IJsonSerializer jsonSerializer,
        ISettingProvider settingProvider,
        IHttpClientFactory httpClientFactory)
    {
        Cache = cache;
        JsonSerializer = jsonSerializer;
        SettingProvider = settingProvider;
        HttpClientFactory = httpClientFactory;

        Logger = NullLogger<PushPlusTokenProvider>.Instance;
    }

    public async virtual Task<PushPlusToken> GetTokenAsync(CancellationToken cancellationToken = default)
    {
        var token = await SettingProvider.GetOrNullAsync(PushPlusSettingNames.Security.Token);
        var secretKey = await SettingProvider.GetOrNullAsync(PushPlusSettingNames.Security.SecretKey);

        Check.NotNullOrEmpty(token, PushPlusSettingNames.Security.Token);
        Check.NotNullOrEmpty(secretKey, PushPlusSettingNames.Security.SecretKey);

        return await GetTokenAsync(token, secretKey, cancellationToken);
    }

    public async virtual Task<PushPlusToken> GetTokenAsync(
            string token,
            string secretKey,
            CancellationToken cancellationToken = default)
    {
        var cacheItem = await GetCacheItemAsync(token, secretKey, cancellationToken);

        return new PushPlusToken(cacheItem.AccessKey, cacheItem.ExpiresIn);
    }

    protected async virtual Task<PushPlusTokenCacheItem> GetCacheItemAsync(
        string token,
        string secretKey,
        CancellationToken cancellationToken = default)
    {
        var cacheKey = PushPlusTokenCacheItem.CalculateCacheKey(token, secretKey);

        Logger.LogDebug($"PushPlusTokenProvider.GetCacheItemAsync: {cacheKey}");

        var cacheItem = await Cache.GetAsync(cacheKey, token: cancellationToken);

        if (cacheItem != null)
        {
            Logger.LogDebug($"Found in the cache: {cacheKey}");
            return cacheItem;
        }

        Logger.LogDebug($"Not found in the cache, getting from the httpClient: {cacheKey}");

        var client = HttpClientFactory.GetPushPlusClient();

        var content = await client.GetTokenContentAsync(token, secretKey, cancellationToken);

        var pushPlusResponse = JsonSerializer.Deserialize<PushPlusResponse<PushPlusToken>>(content);
        var pushPlusToken = pushPlusResponse.GetData();

        cacheItem = new PushPlusTokenCacheItem(
            pushPlusToken.AccessKey,
            pushPlusToken.ExpiresIn);

        await Cache.SetAsync(
            cacheKey,
            cacheItem,
            new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(pushPlusToken.ExpiresIn - 120)
            },
            token: cancellationToken);

        Logger.LogDebug($"Finished setting the cache item: {cacheKey}");

        return cacheItem;
    }
}
