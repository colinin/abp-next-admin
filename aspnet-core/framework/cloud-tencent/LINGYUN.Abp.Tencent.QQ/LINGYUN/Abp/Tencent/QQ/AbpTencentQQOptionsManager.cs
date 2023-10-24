using LINGYUN.Abp.Tencent.QQ.Settings;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;
using Volo.Abp.Options;
using Volo.Abp.Settings;

namespace LINGYUN.Abp.Tencent.QQ;

public class AbpTencentQQOptionsManager : AbpDynamicOptionsManager<AbpTencentQQOptions>
{
    protected IMemoryCache TencentCache { get; }
    protected ISettingProvider SettingProvider { get; }
    public AbpTencentQQOptionsManager(
        IMemoryCache tencentCache,
        ISettingProvider settingProvider,
        IOptionsFactory<AbpTencentQQOptions> factory)
        : base(factory)
    {
        TencentCache = tencentCache;
        SettingProvider = settingProvider;
    }

    protected override async Task OverrideOptionsAsync(string name, AbpTencentQQOptions options)
    {
        var cacheItem = await GetCacheItemAsync();

        options.AppId = cacheItem.AppId;
        options.AppKey = cacheItem.AppKey;
        options.IsMobile = cacheItem.IsMobile;
    }

    protected async virtual Task<AbpTencentQQCacheItem> GetCacheItemAsync()
    {
        var cacheKey = AbpTencentQQCacheItem.CalculateCacheKey();

        var cacheItem = await TencentCache.GetOrCreateAsync(
            cacheKey,
            async (cache) =>
            {
                var appId = await SettingProvider.GetOrNullAsync(TencentQQSettingNames.QQConnect.AppId);
                var appKey = await SettingProvider.GetOrNullAsync(TencentQQSettingNames.QQConnect.AppKey);
                var isMobile = await SettingProvider.IsTrueAsync(TencentQQSettingNames.QQConnect.IsMobile);

                cache.SetAbsoluteExpiration(TimeSpan.FromMinutes(2d));

                return new AbpTencentQQCacheItem(appId, appKey, isMobile);
            });

        return cacheItem;
    }
}
