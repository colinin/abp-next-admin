using Microsoft.Extensions.Options;
using System;
using System.Linq;
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
    protected AbpBackgroundTasksOptions TasksOptions { get; }

    public BackgroundWorkerManager(
        IClock clock,
        IJobStore jobStore,
        IJobPublisher jobPublisher,
        ICurrentTenant currentTenant,
        IOptions<AbpBackgroundTasksOptions> options,
        IOptions<AbpBackgroundTasksOptions> taskOptions)
    {
        Clock = clock;
        JobStore = jobStore;
        JobPublisher = jobPublisher;
        CurrentTenant = currentTenant;
        Options = options.Value;
        TasksOptions = taskOptions.Value;
    }

    public async Task AddAsync(IBackgroundWorker worker, CancellationToken cancellationToken = default)
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

        var workerType = ProxyHelper.GetUnProxiedType(worker);
        if (workerType != null && TasksOptions.JobDispatcherSelectors.IsMatch(workerType))
        {
            var selector = TasksOptions
                .JobDispatcherSelectors
                .FirstOrDefault(x => x.Predicate(workerType));

            jobInfo.Interval = selector.Interval;
            jobInfo.LockTimeOut = selector.LockTimeOut;
            jobInfo.Priority = selector.Priority;
            jobInfo.TryCount = selector.TryCount;
            jobInfo.MaxTryCount = selector.MaxTryCount;

            if (!selector.NodeName.IsNullOrWhiteSpace())
            {
                jobInfo.NodeName = selector.NodeName;
            }

            if (!selector.Cron.IsNullOrWhiteSpace())
            {
                jobInfo.Cron = selector.Cron;
            }
        }

        // 存储状态
        await JobStore.StoreAsync(jobInfo, cancellationToken);

        // 发布作业
        await JobPublisher.PublishAsync(jobInfo, cancellationToken);
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
