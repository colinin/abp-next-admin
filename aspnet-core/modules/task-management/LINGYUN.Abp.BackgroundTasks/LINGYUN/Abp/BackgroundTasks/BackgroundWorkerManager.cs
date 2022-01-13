using System;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.BackgroundWorkers;
using Volo.Abp.DependencyInjection;
using Volo.Abp.DynamicProxy;

namespace LINGYUN.Abp.BackgroundTasks;

[Dependency(ReplaceServices = true)]
public class BackgroundWorkerManager : IBackgroundWorkerManager, ISingletonDependency
{
    protected IJobStore JobStore { get; }
    protected IJobScheduler JobScheduler { get; }
    protected IServiceProvider ServiceProvider { get; }

    public BackgroundWorkerManager(
        IJobStore jobStore,
        IJobScheduler jobScheduler,
        IServiceProvider serviceProvider)
    {
        JobStore = jobStore;
        JobScheduler = jobScheduler;
        ServiceProvider = serviceProvider;
    }

    public async Task AddAsync(IBackgroundWorker worker)
    {
        var adapterType = typeof(BackgroundWorkerAdapter<>).MakeGenericType(ProxyHelper.GetUnProxiedType(worker));

        var workerAdapter = ServiceProvider.GetService(adapterType) as IBackgroundWorkerRunnable;

        var jobInfo = workerAdapter?.BuildWorker(worker);
        if (jobInfo == null)
        {
            return;
        }

        // 存储状态
        await JobStore.StoreAsync(jobInfo);

        // 手动入队
        await JobScheduler.QueueAsync(jobInfo);
    }

    public Task StartAsync(CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }
}
