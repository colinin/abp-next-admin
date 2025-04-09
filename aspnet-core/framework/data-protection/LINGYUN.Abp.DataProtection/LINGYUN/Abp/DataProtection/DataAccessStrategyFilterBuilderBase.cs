using System;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Data;

namespace LINGYUN.Abp.DataProtection;

public abstract class DataAccessStrategyFilterBuilderBase : IDataAccessStrategyFilterBuilder
{
    private readonly IDataFilter _dataFilter;
    private readonly IDataAccessScope _dataAccessScope;
    private readonly IDataAccessStrategyStateProvider _strategyStateProvider;
    protected DataAccessStrategyFilterBuilderBase(
        IDataFilter dataFilter,
        IDataAccessScope dataAccessScope,
        IDataAccessStrategyStateProvider strategyStateProvider)
    {
        _dataFilter = dataFilter;
        _dataAccessScope = dataAccessScope;
        _strategyStateProvider = strategyStateProvider;
    }

    public async virtual Task<DataAccessStrategyFilterBuildResult<TEntity>> Build<TEntity, TKey, TEntityAuth>(IQueryable<TEntity> entity, IQueryable<TEntityAuth> entityAuth)
        where TEntityAuth : DataAuthBase<TEntity, TKey>
    {
        if (ShouldApplyFilter(typeof(TEntity), DataAccessOperation.Read))
        {

            var strategyState = await _strategyStateProvider.GetOrNullAsync();
            if (strategyState != null && strategyState.Strategy != DataAccessStrategy.Custom)
            {
                var newQueryable = Build<TEntity, TKey, TEntityAuth>(entity, entityAuth, strategyState);

                return new DataAccessStrategyFilterBuildResult<TEntity>(
                    strategyState.Strategy,
                    newQueryable);
            }
        }

        return null;
    }

    protected virtual bool ShouldApplyFilter(Type entityType, DataAccessOperation operation)
    {
        // TODO: 使用一个范围标志来确定当前需要禁用的数据权限操作
        if (!_dataFilter.IsEnabled<IDataProtected>())
        {
            return false;
        }

        if (entityType.IsDefined(typeof(DisableDataProtectedAttribute), true))
        {
            return false;
        }

        if (_dataAccessScope.Operations != null && !_dataAccessScope.Operations.Contains(operation))
        {
            return false;
        }

        return true;
    }

    protected abstract IQueryable<TEntity> Build<TEntity, TKey, TEntityAuth>(IQueryable<TEntity> entity, IQueryable<TEntityAuth> entityAuth, DataAccessStrategyState state)
        where TEntityAuth : DataAuthBase<TEntity, TKey>;
}
