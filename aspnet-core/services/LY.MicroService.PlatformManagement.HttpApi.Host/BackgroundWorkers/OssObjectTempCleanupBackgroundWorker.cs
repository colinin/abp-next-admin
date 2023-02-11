using LINGYUN.Abp.OssManagement;
using LY.MicroService.PlatformManagement.MultiTenancy;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using Volo.Abp.BackgroundWorkers;
using Volo.Abp.DistributedLocking;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Threading;

namespace LY.MicroService.PlatformManagement.BackgroundWorkers;

public class OssObjectTempCleanupBackgroundWorker : AsyncPeriodicBackgroundWorkerBase
{
    protected ICurrentTenant CurrentTenant { get; }
    protected IAbpDistributedLock DistributedLock { get; }
    protected ITenantConfigurationCache TenantConfigurationCache { get; }

    public OssObjectTempCleanupBackgroundWorker(
        AbpAsyncTimer timer,
        IServiceScopeFactory serviceScopeFactory,
        IOptionsMonitor<AbpOssManagementOptions> cleanupOptions,
        IAbpDistributedLock distributedLock,
        ICurrentTenant currentTenant,
        ITenantConfigurationCache tenantConfigurationCache)
        : base(timer, serviceScopeFactory)
    {
        CurrentTenant = currentTenant;
        DistributedLock = distributedLock;
        TenantConfigurationCache = tenantConfigurationCache;
        timer.Period = cleanupOptions.CurrentValue.CleanupPeriod;
    }

    protected async override Task DoWorkAsync(PeriodicBackgroundWorkerContext workerContext)
    {
        await using var handle = await DistributedLock.TryAcquireAsync(nameof(OssObjectTempCleanupBackgroundWorker));

        Logger.LogInformation($"Lock is acquired for {nameof(OssObjectTempCleanupBackgroundWorker)}");

        if (handle != null)
        {
            using (CurrentTenant.Change(null))
            {
                await ExecuteCleanService(workerContext);

                var allActiveTenants = await TenantConfigurationCache.GetTenantsAsync();

                foreach (var activeTenant in allActiveTenants)
                {
                    using (CurrentTenant.Change(activeTenant.Id, activeTenant.Name))
                    {
                        await ExecuteCleanService(workerContext);
                    }
                }
            }

            Logger.LogInformation($"Lock is released for {nameof(OssObjectTempCleanupBackgroundWorker)}");
            return;
        }

        Logger.LogInformation($"Handle is null because of the locking for : {nameof(OssObjectTempCleanupBackgroundWorker)}");
    }

    private async static Task ExecuteCleanService(PeriodicBackgroundWorkerContext workerContext)
    {
        await workerContext
            .ServiceProvider
            .GetRequiredService<OssObjectTempCleanupService>()
            .CleanAsync();
    }
}
