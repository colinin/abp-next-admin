using LINGYUN.Abp.Dingtalk.Settings;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Caching;
using Volo.Abp.Settings;

namespace LINGYUN.Abp.Dingtalk;

public abstract class DingtalkClientFactory<TClient>
{
    protected ISettingProvider SettingProvider { get; }
    protected IDistributedCache<DingtalkAccessTokenCacheItem> TokenCache { get; }
    public DingtalkClientFactory(
        ISettingProvider settingProvider,
        IDistributedCache<DingtalkAccessTokenCacheItem> tokenCache)
    {
        TokenCache = tokenCache;
        SettingProvider = settingProvider;
    }

    public async virtual Task<TClient> CreateAsync()
    {
        var appKey = await SettingProvider.GetOrNullAsync(DingtalkSettingNames.AppKey);
        var appSecret = await SettingProvider.GetOrNullAsync(DingtalkSettingNames.AppSecret);

        Check.NotNullOrWhiteSpace(appKey, DingtalkSettingNames.AppKey);
        Check.NotNullOrWhiteSpace(appSecret, DingtalkSettingNames.AppSecret);

        var cacheItem = await GetCacheItemAsync(appKey, appSecret);

        return GetClient(appKey, appSecret, cacheItem.AccessToken);
    }

    protected abstract TClient GetClient(
        string appKey,
        string appSecret,
        string accessToken);

    protected async virtual Task<DingtalkAccessTokenCacheItem> GetCacheItemAsync(string appKey, string appSecret)
    {
        var cacheKey = $"{appKey}:{appSecret}".ToMd5();
        var cacheItem = await TokenCache.GetAsync(cacheKey);
        if (cacheItem == null)
        {
            var client = new AlibabaCloud.SDK.Dingtalkoauth2_1_0.Client(
                new AlibabaCloud.OpenApiClient.Models.Config
                {
                    Protocol = "https",
                    RegionId = "central",
                    DisableHttp2 = true,
                });

            var accessTokenRes = await client.GetAccessTokenAsync(
                new AlibabaCloud.SDK.Dingtalkoauth2_1_0.Models.GetAccessTokenRequest
                {
                    AppKey = appKey,
                    AppSecret = appSecret,
                });

            cacheItem = new DingtalkAccessTokenCacheItem(
                accessTokenRes.Body.AccessToken,
                accessTokenRes.Body.ExpireIn);

            var cacheOptions = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds((accessTokenRes.Body.ExpireIn ?? 7200) - 10),
            };

            await TokenCache.SetAsync(
                cacheKey,
                cacheItem,
                cacheOptions);
        }

        return cacheItem;
    }
}
