using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace LINGYUN.Abp.CachingManagement;

public interface ICacheAppService : IApplicationService
{
    Task<CacheKeysDto> GetKeysAsync(GetCacheKeysInput input);

    Task<CacheValueDto> GetValueAsync(CacheKeyInput input);

    Task SetAsync(CacheSetInput input);

    Task RefreshAsync(CacheRefreshInput input);

    Task RemoveAsync(CacheKeyInput input);
}
