using System;
using System.Threading;
using System.Threading.Tasks;
using LINGYUN.Abp.OperationRateLimiting.Algorithms.Concurrency;
using LINGYUN.Abp.OperationRateLimiting.Algorithms.FixedWindow;
using LINGYUN.Abp.OperationRateLimiting.Algorithms.SlidingWindow;
using LINGYUN.Abp.OperationRateLimiting.Algorithms.TokenBucket;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using Volo.Abp;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;
using Volo.Abp.DistributedLocking;
using Volo.Abp.Json;

namespace LINGYUN.Abp.OperationRateLimiting;

/// <summary>
/// Default <see cref="IOperationRateLimitingStore"/> based on ABP's distributed cache.
/// </summary>
/// <remarks>
/// The built-in algorithms are executed by the executors of their algorithm folder while a
/// distributed lock (an <see cref="IAbpDistributedLock"/> named after the counter) is held.
/// This makes the update atomic when the distributed lock provider is shared by all instances.
/// For Redis deployments, replace it with
/// <c>LINGYUN.Abp.OperationRateLimiting.StackExchangeRedis</c> (Lua scripts, no lock).
/// </remarks>
public class DistributedCacheOperationRateLimitingStore : IOperationRateLimitingStore, ITransientDependency
{
    protected IDistributedCache<OperationRateLimitingStateCacheItem> Cache { get; }

    protected IAbpDistributedLock DistributedLock { get; }

    protected IJsonSerializer JsonSerializer { get; }

    protected IOptions<AbpOperationRateLimitingOptions> Options { get; }

    public DistributedCacheOperationRateLimitingStore(
        IDistributedCache<OperationRateLimitingStateCacheItem> cache,
        IAbpDistributedLock distributedLock,
        IJsonSerializer jsonSerializer,
        IOptions<AbpOperationRateLimitingOptions> options)
    {
        Cache = cache;
        DistributedLock = distributedLock;
        JsonSerializer = jsonSerializer;
        Options = options;
    }

    public virtual async Task<OperationRateLimitingStoreResult> ExecuteAsync(
        OperationRateLimitingStoreRequest request,
        CancellationToken cancellationToken = default)
    {
        Check.NotNull(request, nameof(request));
        Check.NotNullOrWhiteSpace(request.Key, nameof(request.Key));

        if (request.MaxCount <= 0)
        {
            return OperationRateLimitingStoreResult.Deny(0, 0, null, request.Duration, isPermanent: true);
        }

        switch (request.Algorithm)
        {
            case OperationRateLimitingAlgorithms.FixedWindow:
                return await FixedWindowRateLimitingExecutor.ExecuteAsync(this, request, cancellationToken);

            case OperationRateLimitingAlgorithms.SlidingWindow:
                return await SlidingWindowRateLimitingExecutor.ExecuteAsync(this, request, cancellationToken);

            case OperationRateLimitingAlgorithms.TokenBucket:
                return await TokenBucketRateLimitingExecutor.ExecuteAsync(this, request, cancellationToken);

            case OperationRateLimitingAlgorithms.Concurrency:
                return await ConcurrencyRateLimitingExecutor.ExecuteAsync(this, request, cancellationToken);

            default:
                throw new AbpException(
                    $"The distributed cache store does not implement the algorithm '{request.Algorithm}'. " +
                    "Custom algorithms must use IOperationRateLimitingStore.UpdateStateAsync in their rule implementation.");
        }
    }

    public virtual async Task ReleaseAsync(
        OperationRateLimitingStoreRequest request,
        CancellationToken cancellationToken = default)
    {
        Check.NotNull(request, nameof(request));

        if (request.Algorithm != OperationRateLimitingAlgorithms.Concurrency)
        {
            return;
        }

        await ConcurrencyRateLimitingExecutor.ReleaseAsync(this, request, cancellationToken);
    }

    public virtual async Task<TState?> GetStateAsync<TState>(string key, CancellationToken cancellationToken = default)
        where TState : class, new()
    {
        Check.NotNullOrWhiteSpace(key, nameof(key));

        var cacheItem = await Cache.GetAsync(key, token: cancellationToken);
        if (cacheItem == null || string.IsNullOrWhiteSpace(cacheItem.State))
        {
            return null;
        }

        return JsonSerializer.Deserialize<TState>(cacheItem.State);
    }

    public virtual async Task<TState?> UpdateStateAsync<TState>(
        string key,
        Func<TState?, TState?> updater,
        TimeSpan expiration,
        CancellationToken cancellationToken = default)
        where TState : class, new()
    {
        Check.NotNullOrWhiteSpace(key, nameof(key));
        Check.NotNull(updater, nameof(updater));

        var lockName = AbpOperationRateLimitingConsts.LockKeyPrefix + ":" + key;
        var handle = await DistributedLock.TryAcquireAsync(
            lockName,
            Options.Value.LockTimeout,
            cancellationToken);

        if (handle == null)
        {
            throw new AbpException(
                $"Could not acquire the distributed lock '{lockName}' within {Options.Value.LockTimeout} to update the operation rate limiting counter. " +
                "This is an infrastructure failure, not a rate limit violation.");
        }

        await using (handle)
        {
            var current = await GetStateAsync<TState>(key, cancellationToken);
            var updated = updater(current);

            if (updated == null)
            {
                await Cache.RemoveAsync(key, token: cancellationToken);
                return null;
            }

            await Cache.SetAsync(
                key,
                new OperationRateLimitingStateCacheItem
                {
                    State = JsonSerializer.Serialize(updated)
                },
                new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = NormalizeExpiration(expiration)
                },
                token: cancellationToken);

            return updated;
        }
    }

    public virtual async Task ResetAsync(string key, CancellationToken cancellationToken = default)
    {
        Check.NotNullOrWhiteSpace(key, nameof(key));

        var lockName = AbpOperationRateLimitingConsts.LockKeyPrefix + ":" + key;
        var handle = await DistributedLock.TryAcquireAsync(
            lockName,
            Options.Value.LockTimeout,
            cancellationToken);

        if (handle == null)
        {
            throw new AbpException(
                $"Could not acquire the distributed lock '{lockName}' within {Options.Value.LockTimeout} to reset the operation rate limiting counter.");
        }

        await using (handle)
        {
            await Cache.RemoveAsync(key, token: cancellationToken);
        }
    }

    protected static TimeSpan NormalizeExpiration(TimeSpan expiration)
    {
        return expiration > TimeSpan.Zero ? expiration : TimeSpan.FromMinutes(5);
    }
}
