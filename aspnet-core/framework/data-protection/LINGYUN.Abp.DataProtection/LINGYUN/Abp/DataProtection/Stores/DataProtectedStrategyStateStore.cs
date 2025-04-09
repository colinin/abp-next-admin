using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.DataProtection.Stores;

public class DataProtectedStrategyStateStore : IDataProtectedStrategyStateStore, ITransientDependency
{
    private readonly IDataProtectedStrategyStateCache _cache;

    public DataProtectedStrategyStateStore(IDataProtectedStrategyStateCache cache)
    {
        _cache = cache;
    }

    public async virtual Task<DataAccessStrategyState> GetOrNullAsync(string subjectName, string subjectId)
    {
        var cacheItem = await _cache.GetAsync(subjectName, subjectId);
        if (cacheItem == null )
        {
            return null;
        }

        return new DataAccessStrategyState(
            cacheItem.SubjectName,
            new string[] { cacheItem.SubjectId },
            cacheItem.Strategy);
    }

    public async virtual Task RemoveAsync(DataAccessStrategyState state)
    {
        await _cache.RemoveAsync(state);
    }

    public async virtual Task SetAsync(DataAccessStrategyState state)
    {
        await _cache.SetAsync(state);
    }
}
