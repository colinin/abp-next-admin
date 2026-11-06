using System;
using System.Threading;
using System.Threading.Tasks;

namespace LINGYUN.Abp.OperationRateLimiting;

/// <summary>
/// Low level access to the state of a counter. Algorithms use it to read, update and reset their
/// own state; unlike the former store abstraction it does not execute or dispatch algorithms.
/// </summary>
public interface IOperationRateLimitingStateAccessor
{
    Task<TState?> GetStateAsync<TState>(string key, CancellationToken cancellationToken = default)
        where TState : class, new();

    Task<TState?> UpdateStateAsync<TState>(
        string key,
        Func<TState?, TState?> updater,
        TimeSpan expiration,
        CancellationToken cancellationToken = default)
        where TState : class, new();

    Task ResetAsync(string key, CancellationToken cancellationToken = default);
}
