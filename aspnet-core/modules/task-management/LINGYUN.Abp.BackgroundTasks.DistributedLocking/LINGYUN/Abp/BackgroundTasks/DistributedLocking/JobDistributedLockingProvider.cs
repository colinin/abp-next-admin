using Microsoft.Extensions.Caching.Memory;
using System;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.DistributedLocking;

namespace LINGYUN.Abp.BackgroundTasks.DistributedLocking;

[Dependency(ReplaceServices = true)]
public class JobDistributedLockingProvider : IJobLockProvider, ISingletonDependency
{
    protected IMemoryCache LockCache { get; }
    protected IAbpDistributedLock DistributedLock { get; }

    public JobDistributedLockingProvider(
        IMemoryCache lockCache,
        IAbpDistributedLock distributedLock)
    {
        LockCache = lockCache;
        DistributedLock = distributedLock;
    }

    public virtual async Task<bool> TryLockAsync(string jobKey, int lockSeconds)
    {
        var handle = await DistributedLock.TryAcquireAsync(jobKey);
        if (handle != null)
        {
            await LockCache.GetOrCreateAsync(jobKey, (entry) =>
            {
                entry.SetAbsoluteExpiration(TimeSpan.FromSeconds(lockSeconds));
                entry.RegisterPostEvictionCallback(async (object key, object value, EvictionReason reason, object state) =>
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

    public virtual async Task<bool> TryReleaseAsync(string jobKey)
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
