using System.Threading.Tasks;

namespace LINGYUN.Abp.DataProtection.Stores;

public interface IDataProtectedStrategyStateCache
{
    Task SetAsync(DataAccessStrategyState state);

    Task RemoveAsync(DataAccessStrategyState state);

    Task<DataAccessStrategyStateCacheItem> GetAsync(string subjectName, string subjectId);
}
