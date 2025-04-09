using System.Threading.Tasks;

namespace LINGYUN.Abp.DataProtection;

public interface IDataAccessStrategyStateProvider
{
    Task<DataAccessStrategyState> GetOrNullAsync();
}
