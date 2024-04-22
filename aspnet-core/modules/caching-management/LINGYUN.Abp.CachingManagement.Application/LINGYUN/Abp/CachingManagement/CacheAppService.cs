using LINGYUN.Abp.CachingManagement.Localization;
using LINGYUN.Abp.CachingManagement.Permissions;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace LINGYUN.Abp.CachingManagement;

[Authorize(CachingManagementPermissionNames.Cache.Default)]
public class CacheAppService : ApplicationService, ICacheAppService
{
    protected ICacheManager CacheManager { get; }

    public CacheAppService(ICacheManager cacheManager)
    {
        CacheManager = cacheManager;

        LocalizationResource = typeof(CacheResource);
    }

    public async virtual Task<CacheKeysDto> GetKeysAsync(GetCacheKeysInput input)
    {
        var res = await CacheManager.GetKeysAsync(
            input.Prefix,
            input.Filter,
            input.Marker);

        return new CacheKeysDto
        {
            NextMarker = res.NextMarker,
            Keys = res.Keys.ToList(),
        };
    }

    public async virtual Task<CacheValueDto> GetValueAsync(CacheKeyInput input)
    {
        var res = await CacheManager.GetValueAsync(input.Key);

        var value = new CacheValueDto
        {
            Size = res.Size,
            Type = res.Type,
            Values = res.Values,
        };
        if (res.Ttl.HasValue)
        {
            value.Expiration = Clock.Now.Add(res.Ttl.Value);
        }

        return value;
    }

    [Authorize(CachingManagementPermissionNames.Cache.ManageValue)]
    public async virtual Task SetAsync(CacheSetInput input)
    {
        TimeSpan? absExpir = null;
        TimeSpan? sldExpr = null;

        if (input.AbsoluteExpiration.HasValue && input.AbsoluteExpiration.Value > Clock.Now)
        {
            absExpir = input.AbsoluteExpiration.Value - Clock.Now;
        }
        if (input.SlidingExpiration.HasValue && input.SlidingExpiration.Value > Clock.Now)
        {
            sldExpr = input.SlidingExpiration.Value - Clock.Now;
        }

        await CacheManager.SetAsync(input.Key, input.Value, absExpir, sldExpr);
    }

    [Authorize(CachingManagementPermissionNames.Cache.Refresh)]
    public async virtual Task RefreshAsync(CacheRefreshInput input)
    {
        TimeSpan? absExpir = null;
        TimeSpan? sldExpr = null;

        if (input.AbsoluteExpiration.HasValue && input.AbsoluteExpiration.Value > Clock.Now)
        {
            absExpir = input.AbsoluteExpiration.Value - Clock.Now;
        }
        if (input.SlidingExpiration.HasValue && input.SlidingExpiration.Value > Clock.Now)
        {
            sldExpr = input.SlidingExpiration.Value - Clock.Now;
        }

        await CacheManager.RefreshAsync(input.Key, absExpir, sldExpr);
    }

    [Authorize(CachingManagementPermissionNames.Cache.Delete)]
    public async virtual Task RemoveAsync(CacheKeyInput input)
    {
        await CacheManager.RemoveAsync(input.Key);
    }
}
