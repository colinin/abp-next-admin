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
    protected IJobPublisher JobPublisher { get; }

    public BackgroundWorkerManager(
        IJobStore jobStore,
        IJobPublisher jobPublisher)
    {
        JobStore = jobStore;
        JobPublisher = jobPublisher;
    }

    public async Task AddAsync(IBackgroundWorker worker)
    {
        var adapterType = typeof(BackgroundWorkerAdapter<>)
            .MakeGenericType(ProxyHelper.GetUnProxiedType(worker));

        var workerAdapter = Activator.CreateInstance(adapterType) as IBackgroundWorkerRunnable;

        var jobInfo = workerAdapter?.BuildWorker(worker);
        if (jobInfo == null)
        {
            return;
        }

        // 存储状态
        await JobStore.StoreAsync(jobInfo);

        // 发布作业
        await JobPublisher.PublishAsync(jobInfo);
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
