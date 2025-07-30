using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using Volo.Abp.BackgroundWorkers;
using Volo.Abp.DistributedLocking;
using Volo.Abp.Threading;

namespace LINGYUN.Abp.Identity.Session;

public class IdentitySessionCleanupBackgroundWorker : AsyncPeriodicBackgroundWorkerBase
{
    protected IAbpDistributedLock DistributedLock { get; }
    protected IdentitySessionCleanupOptions Options { get; }
    public IdentitySessionCleanupBackgroundWorker(
        AbpAsyncTimer timer,
        IServiceScopeFactory serviceScopeFactory,
        IOptions<IdentitySessionCleanupOptions> options,
        IAbpDistributedLock distributedLock)
        : base(timer, serviceScopeFactory)
    {
        DistributedLock = distributedLock;
        Options = options.Value;
        timer.Period = Options.CleanupPeriod;
    }

    protected async override Task DoWorkAsync(PeriodicBackgroundWorkerContext workerContext)
    {
        if (!Options.IsCleanupEnabled)
        {
            return;
        }

        await using (var handle = await DistributedLock.TryAcquireAsync(nameof(IdentitySessionCleanupBackgroundWorker)))
        {
            Logger.LogInformation($"Lock is acquired for {nameof(IdentitySessionCleanupBackgroundWorker)}");

            if (handle != null)
            {
                await workerContext
                    .ServiceProvider
                    .GetRequiredService<IdentitySessionCleanupService>()
                    .CleanAsync();

                Logger.LogInformation($"Lock is released for {nameof(IdentitySessionCleanupBackgroundWorker)}");
                return;
            }

            Logger.LogInformation($"Handle is null because of the locking for : {nameof(IdentitySessionCleanupBackgroundWorker)}");
        }
    }
}
