using System.Threading;

namespace LINGYUN.Abp.DataProtection;

public class AsyncLocalCurrentDataAccessAccessor : ICurrentDataAccessAccessor
{
    public static AsyncLocalCurrentDataAccessAccessor Instance { get; } = new();

    public DataAccessOperation[] Current
    {
        get => _currentScope.Value;
        set => _currentScope.Value = value;
    }

    private readonly AsyncLocal<DataAccessOperation[]> _currentScope;
    private AsyncLocalCurrentDataAccessAccessor()
    {
        _currentScope = new AsyncLocal<DataAccessOperation[]>();
    }
}
