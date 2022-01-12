using Microsoft.Extensions.Caching.Memory;
using System;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.DistributedLocking;

namespace LINGYUN.Abp.BackgroundTasks;

[Dependency(ReplaceServices = true)]
public class JobLockProvider : IJobLockProvider, ISingletonDependency
{
    protected IMemoryCache LockCache { get; }
    protected IAbpDistributedLock DistributedLock { get; }

    public JobLockProvider(
        IMemoryCache lockCache,
        IAbpDistributedLock distributedLock)
    {
        LockCache = lockCache;
        DistributedLock = distributedLock;
    }

    public virtual async Task<bool> TryLockAsync(string jobKey, int lockSeconds, CancellationToken cancellationToken = default)
    {
        var handle = await DistributedLock.TryAcquireAsync(jobKey, cancellationToken: cancellationToken);
        if (handle != null)
        {
            await LockCache.GetOrCreateAsync(jobKey, (entry) =>
            {
                entry.SetAbsoluteExpiration(TimeSpan.FromSeconds(lockSeconds));
                entry.RegisterPostEvictionCallback(async (key, value, reason, state) =>
                {
                    if (reason == EvictionReason.Expired && value is IAbpDistributedLockHandle handleValue)
                    {
                        await handleValue.DisposeAsync();
                    }
                });
                entry.SetValue(handle);

                return Task.FromResult(handle);
            });

            return true;
        }
        return false;
    }

    public virtual async Task<bool> TryReleaseAsync(string jobKey, CancellationToken cancellationToken = default)
    {
        if (LockCache.TryGetValue<IAbpDistributedLockHandle>(jobKey, out var handle))
        {
            await handle.DisposeAsync();

            LockCache.Remove(jobKey);

            return true;
        }
        return false;
    }
}
