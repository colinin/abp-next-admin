using LINGYUN.Abp.BlobManagement;
using Microsoft.Extensions.Options;
using Volo.Abp.BackgroundWorkers;
using Volo.Abp.DistributedLocking;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Threading;

namespace LINGYUN.Abp.MicroService.PlatformService.BackgroundWorkers;

public class BlobTempCleanupBackgroundWorker : AsyncPeriodicBackgroundWorkerBase
{
    protected ICurrentTenant CurrentTenant { get; }
    protected IAbpDistributedLock DistributedLock { get; }
    protected ITenantStore TenantStore { get; }

    public BlobTempCleanupBackgroundWorker(
        AbpAsyncTimer timer,
        IServiceScopeFactory serviceScopeFactory,
        IOptionsMonitor<AbpBlobManagementOptions> cleanupOptions,
        IAbpDistributedLock distributedLock,
        ICurrentTenant currentTenant,
        ITenantStore tenantStore)
        : base(timer, serviceScopeFactory)
    {
        CurrentTenant = currentTenant;
        DistributedLock = distributedLock;
        TenantStore = tenantStore;
        timer.Period = cleanupOptions.CurrentValue.CleanupPeriod;
    }

    protected async override Task DoWorkAsync(PeriodicBackgroundWorkerContext workerContext)
    {
        await using var handle = await DistributedLock.TryAcquireAsync(nameof(BlobTempCleanupBackgroundWorker));

        Logger.LogInformation($"Lock is acquired for {nameof(BlobTempCleanupBackgroundWorker)}");

        if (handle != null)
        {
            using (CurrentTenant.Change(null))
            {
                await ExecuteCleanService(workerContext);

                var allActiveTenants = (await TenantStore.GetListAsync()).Where(x => x.IsActive);

                foreach (var activeTenant in allActiveTenants)
                {
                    using (CurrentTenant.Change(activeTenant.Id, activeTenant.Name))
                    {
                        await ExecuteCleanService(workerContext);
                    }
                }
            }

            Logger.LogInformation($"Lock is released for {nameof(BlobTempCleanupBackgroundWorker)}");
            return;
        }

        Logger.LogInformation($"Handle is null because of the locking for : {nameof(BlobTempCleanupBackgroundWorker)}");
    }

    private async static Task ExecuteCleanService(PeriodicBackgroundWorkerContext workerContext)
    {
        await workerContext
            .ServiceProvider
            .GetRequiredService<BlobTempCleanupService>()
            .CleanAsync();
    }
}
