using System;
using System.Threading;
using System.Threading.Tasks;

namespace LINGYUN.Abp.OperationRateLimiting;

/// <summary>
/// Stores the counters of the rate limiting rules.
/// </summary>
/// <remarks>
/// The four built-in algorithms are executed by <see cref="ExecuteAsync"/>: an implementation is
/// responsible for running the whole read-modify-write atomically (the default distributed cache
/// implementation guards it with a distributed lock, the StackExchange.Redis implementation uses
/// Lua scripts).
/// Custom algorithms use <see cref="UpdateStateAsync{TState}"/> to read and write their own state.
/// </remarks>
public interface IOperationRateLimitingStore
{
    /// <summary>
    /// Executes one built-in algorithm atomically.
    /// </summary>
    Task<OperationRateLimitingStoreResult> ExecuteAsync(
        OperationRateLimitingStoreRequest request,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Releases a permit acquired by the concurrency algorithm (no-op for other algorithms).
    /// </summary>
    Task ReleaseAsync(
        OperationRateLimitingStoreRequest request,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Reads the custom state of a counter without modifying it.
    /// </summary>
    Task<TState?> GetStateAsync<TState>(string key, CancellationToken cancellationToken = default)
        where TState : class, new();

    /// <summary>
    /// Atomically reads, updates and stores the custom state of a counter.
    /// Returning null from <paramref name="updater"/> removes the state.
    /// </summary>
    Task<TState?> UpdateStateAsync<TState>(
        string key,
        Func<TState?, TState?> updater,
        TimeSpan expiration,
        CancellationToken cancellationToken = default)
        where TState : class, new();

    /// <summary>
    /// Removes the counter.
    /// </summary>
    Task ResetAsync(string key, CancellationToken cancellationToken = default);
}
