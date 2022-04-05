using Microsoft.Extensions.Options;
using System;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.BackgroundWorkers;
using Volo.Abp.DependencyInjection;
using Volo.Abp.DynamicProxy;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Timing;

namespace LINGYUN.Abp.BackgroundTasks;

[Dependency(ReplaceServices = true)]
public class BackgroundWorkerManager : IBackgroundWorkerManager, ISingletonDependency
{
    protected IClock Clock { get; }
    protected IJobStore JobStore { get; }
    protected IJobPublisher JobPublisher { get; }
    protected ICurrentTenant CurrentTenant { get; }
    protected AbpBackgroundTasksOptions Options { get; }

    public BackgroundWorkerManager(
        IClock clock,
        IJobStore jobStore,
        IJobPublisher jobPublisher,
        ICurrentTenant currentTenant,
        IOptions<AbpBackgroundTasksOptions> options)
    {
        Clock = clock;
        JobStore = jobStore;
        JobPublisher = jobPublisher;
        CurrentTenant = currentTenant;
        Options = options.Value;
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

        jobInfo.NodeName = Options.NodeName;
        jobInfo.BeginTime = Clock.Now;
        jobInfo.CreationTime = Clock.Now;
        jobInfo.TenantId = CurrentTenant.Id;
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
