using System;
using Volo.Abp;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.DataProtection;

public class DataAccessScope : IDataAccessScope, ITransientDependency
{
    public DataAccessOperation[] Operations => _currentDataAccessAccessor.Current;

    private readonly ICurrentDataAccessAccessor _currentDataAccessAccessor;
    public DataAccessScope(ICurrentDataAccessAccessor currentDataAccessAccessor)
    {
        _currentDataAccessAccessor = currentDataAccessAccessor;
    }

    public IDisposable BeginScope(DataAccessOperation[] operations = null)
    {
        var parentScope = _currentDataAccessAccessor.Current;
        _currentDataAccessAccessor.Current = operations;

        return new DisposeAction<ValueTuple<ICurrentDataAccessAccessor, DataAccessOperation[]>>(static (state) =>
        {
            var (currentDataAccessAccessor, parentScope) = state;
            currentDataAccessAccessor.Current = parentScope;
        }, (_currentDataAccessAccessor, parentScope));
    }
}
