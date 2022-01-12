using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.BackgroundTasks;

[Dependency(TryRegister = true)]
public class DefaultJobLockProvider : IJobLockProvider, ISingletonDependency
{
    private readonly ConcurrentDictionary<string, SemaphoreSlim> _localSyncObjects = new();

    public virtual Task<bool> TryLockAsync(string jobKey, int lockSeconds, CancellationToken cancellationToken = default)
    {
        Check.NotNullOrWhiteSpace(jobKey, nameof(jobKey));
        if (_localSyncObjects.ContainsKey(jobKey))
        {
            return Task.FromResult(false);
        }

        var semaphore = new SemaphoreSlim(1, 1);
        return Task.FromResult(_localSyncObjects.TryAdd(jobKey, semaphore));
    }

    public Task<bool> TryReleaseAsync(string jobKey, CancellationToken cancellationToken = default)
    {
        Check.NotNullOrWhiteSpace(jobKey, nameof(jobKey));

        if (_localSyncObjects.TryGetValue(jobKey, out var semaphore))
        {
            semaphore.Dispose();

            return Task.FromResult(_localSyncObjects.TryRemove(jobKey, out _));
        }

        return Task.FromResult(false);
    }
}
