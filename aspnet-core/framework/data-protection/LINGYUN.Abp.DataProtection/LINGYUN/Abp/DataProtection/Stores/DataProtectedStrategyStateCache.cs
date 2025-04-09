using Microsoft.Extensions.Caching.Distributed;
using System.Threading.Tasks;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.DataProtection.Stores;

public class DataProtectedStrategyStateCache : IDataProtectedStrategyStateCache, ITransientDependency
{
    private readonly IDistributedCache<DataAccessStrategyStateCacheItem> _cache;

    public DataProtectedStrategyStateCache(IDistributedCache<DataAccessStrategyStateCacheItem> cache)
    {
        _cache = cache;
    }

    public async virtual Task<DataAccessStrategyStateCacheItem> GetAsync(string subjectName, string subjectId)
    {
        var cacheKey = DataAccessStrategyStateCacheItem.CalculateCacheKey(subjectName, subjectId);
        var cacheItem = await _cache.GetAsync(cacheKey);
        return cacheItem;
    }

    public async virtual Task RemoveAsync(DataAccessStrategyState state)
    {
        foreach (var subjectKey in state.SubjectKeys)
        {
            var cacheKey = DataAccessStrategyStateCacheItem.CalculateCacheKey(state.SubjectName, subjectKey);
            await _cache.RemoveAsync(cacheKey);
        }
    }

    public async virtual Task SetAsync(DataAccessStrategyState state)
    {
        foreach (var subjectKey in state.SubjectKeys)
        {
            var cacheKey = DataAccessStrategyStateCacheItem.CalculateCacheKey(state.SubjectName, subjectKey);
            var cacheItem = new DataAccessStrategyStateCacheItem(state.SubjectName, subjectKey, state.Strategy);
            await _cache.SetAsync(cacheKey, cacheItem, new DistributedCacheEntryOptions());
        }
    }
}
