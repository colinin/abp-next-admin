using System.Threading.Tasks;

namespace LINGYUN.Abp.DataProtection.Stores;

public interface IDataProtectedStrategyStateStore
{
    Task SetAsync(DataAccessStrategyState state);

    Task RemoveAsync(DataAccessStrategyState state);

    Task<DataAccessStrategyState> GetOrNullAsync(string subjectName, string subjectId);
}
