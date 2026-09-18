using Lazy.Captcha.Core.Storage;
using Microsoft.Extensions.Caching.Distributed;
using System;
using Volo.Abp.Caching;

namespace LINGYUN.Abp.LazyCaptcha;

public class AbpCaptchaStorage(IDistributedCache<CaptchaCacheItem> _cache) : IStorage
{
    public virtual string? Get(string key)
    {
        var cacheItem = _cache.Get(key);
        return cacheItem?.Value;
    }

    public virtual void Remove(string key)
    {
        _cache.Remove(key);
    }

    public virtual void Set(string key, string value, DateTimeOffset absoluteExpiration)
    {
        _cache.Set(
            key,
            new CaptchaCacheItem(key, value),
            new DistributedCacheEntryOptions
            {
                AbsoluteExpiration = absoluteExpiration,
            });
    }
}
