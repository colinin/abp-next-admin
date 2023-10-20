using Dapr.Client;
using Microsoft.Extensions.Options;
using System;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.DistributedLocking;
using Volo.Abp.Threading;

namespace LINGYUN.Abp.DistributedLocking.Dapr;

[Dependency(ReplaceServices = true)]
public class DaprAbpDistributedLock : IAbpDistributedLock, ITransientDependency
{
    protected ILockOwnerFinder LockOwnerFinder { get; }
    protected IDaprClientFactory DaprClientFactory { get; }
    protected AbpDistributedLockingDaprOptions Options { get; }
    protected ICancellationTokenProvider CancellationTokenProvider { get; }

    public DaprAbpDistributedLock(
        ILockOwnerFinder lockOwnerFinder,
        IDaprClientFactory daprClientFactory,
        ICancellationTokenProvider cancellationTokenProvider,
        IOptions<AbpDistributedLockingDaprOptions> options)
    {
        Options = options.Value;
        LockOwnerFinder = lockOwnerFinder;
        DaprClientFactory = daprClientFactory;
        CancellationTokenProvider = cancellationTokenProvider;
    }

    public async virtual Task<IAbpDistributedLockHandle> TryAcquireAsync(string name, TimeSpan timeout = default, CancellationToken cancellationToken = default)
    {
        if (timeout == default)
        {
            timeout = Options.DefaultTimeout;
        }
        var client = DaprClientFactory.CreateClient();
        var lockOwner = await LockOwnerFinder.FindAsync();

        var res = await client.Lock(
            Options.StoreName, 
            name,
            lockOwner, 
            (int)timeout.TotalSeconds,
            GetCancellationToken(cancellationToken));

        if (res == null || !res.Success)
        {
            return null;
        }

        return new DaprAbpDistributedLockHandle(Options.StoreName, name, lockOwner, client);
    }

    protected virtual CancellationToken GetCancellationToken(CancellationToken cancellationToken = default)
    {
        return CancellationTokenProvider.FallbackToProvider(cancellationToken);
    }
}
