using System.Threading.Tasks;

namespace LINGYUN.Abp.DataProtection;

public interface IDataAccessStrategyContributor
{
    string Name { get; }
    Task<DataAccessStrategyState> GetOrNullAsync(DataAccessStrategyContributorContext context);
}
