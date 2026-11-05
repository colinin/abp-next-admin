using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using Volo.Abp;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;
using Volo.Abp.DistributedLocking;
using Volo.Abp.Json;

namespace LINGYUN.Abp.OperationRateLimiting;

/// <summary>
/// Default <see cref="IOperationRateLimitingStateAccessor"/> based on ABP's distributed cache.
/// Updates are guarded by a distributed lock named after the counter.
/// </summary>
public class DistributedCacheOperationRateLimitingStateAccessor :
    IOperationRateLimitingStateAccessor,
    ITransientDependency
{
    protected IDistributedCache<OperationRateLimitingStateCacheItem> Cache { get; }

    protected IAbpDistributedLock DistributedLock { get; }

    protected IJsonSerializer JsonSerializer { get; }

    protected IOptions<AbpOperationRateLimitingOptions> Options { get; }

    public DistributedCacheOperationRateLimitingStateAccessor(
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

    public virtual async Task<TState?> GetStateAsync<TState>(
        string key,
        CancellationToken cancellationToken = default)
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
