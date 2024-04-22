using System;
using System.Threading.Tasks;

namespace LINGYUN.Abp.Idempotent;

public class IdempotentGrantResult : IAsyncDisposable
{
    public bool Successed => Exception == null;
    public string IdempotentKey { get; }
    public Exception? Exception { get; }
    protected IAsyncDisposable? Disposable { get; }
    public IdempotentGrantResult(
        string idempotentKey,
        Exception? exception = null,
        IAsyncDisposable? disposable = null)
    {
        IdempotentKey = idempotentKey;
        Exception = exception;
        Disposable = disposable;
    }

    public ValueTask DisposeAsync()
    {
        if (Disposable != null)
        {
            return Disposable.DisposeAsync();
        }
        return default;
    }
}
