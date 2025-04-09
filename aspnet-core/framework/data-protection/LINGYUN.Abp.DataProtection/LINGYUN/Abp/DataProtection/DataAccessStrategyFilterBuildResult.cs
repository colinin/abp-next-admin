using System.Linq;

namespace LINGYUN.Abp.DataProtection;

public class DataAccessStrategyFilterBuildResult<TEntity>
{
    public DataAccessStrategy Strategy { get; }
    public IQueryable<TEntity> Queryable { get; }
    public DataAccessStrategyFilterBuildResult(
        DataAccessStrategy strategy,
        IQueryable<TEntity> queryable)
    {
        Strategy = strategy;
        Queryable = queryable;
    }
}
