using LINGYUN.Abp.CachingManagement.Localization;
using LINGYUN.Abp.CachingManagement.Permissions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;

namespace LINGYUN.Abp.CachingManagement;

[Controller]
[Authorize(CachingManagementPermissionNames.Cache.Default)]
[RemoteService(Name = AbpCachingManagementRemoteServiceConsts.RemoteServiceName)]
[Area(AbpCachingManagementRemoteServiceConsts.ModuleName)]
[Route("api/caching-management/cache")]
public class CacheController : AbpControllerBase, ICacheAppService
{
    protected ICacheAppService CacheAppService { get; }

    public CacheController(ICacheAppService cacheAppService)
    {
        CacheAppService = cacheAppService;

        LocalizationResource = typeof(CacheResource);
    }

    [HttpGet]
    [Route("keys")]
    public virtual Task<CacheKeysDto> GetKeysAsync(GetCacheKeysInput input)
    {
        return CacheAppService.GetKeysAsync(input);
    }

    [HttpGet]
    [Route("value")]
    public virtual Task<CacheValueDto> GetValueAsync(CacheKeyInput input)
    {
        return CacheAppService.GetValueAsync(input);
    }

    [HttpPut]
    [Route("set")]
    [Authorize(CachingManagementPermissionNames.Cache.ManageValue)]
    public virtual Task SetAsync(CacheSetInput input)
    {
        return CacheAppService.SetAsync(input);
    }

    [HttpPut]
    [Route("refresh")]
    [Authorize(CachingManagementPermissionNames.Cache.Refresh)]
    public virtual Task RefreshAsync(CacheRefreshInput input)
    {
        return CacheAppService.RefreshAsync(input);
    }

    [HttpDelete]
    [Route("remove")]
    [Authorize(CachingManagementPermissionNames.Cache.Delete)]
    public virtual Task RemoveAsync(CacheKeyInput input)
    {
        return CacheAppService.RemoveAsync(input);
    }
}
