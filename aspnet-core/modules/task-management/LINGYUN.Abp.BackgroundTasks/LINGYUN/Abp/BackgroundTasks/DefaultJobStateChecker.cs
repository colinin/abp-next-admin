using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Abp.BackgroundTasks;

public class DefaultJobStateChecker : IJobStateChecker, ITransientDependency
{
    private readonly ICurrentTenant _currentTenant;

    private readonly IJobStore _jobStore;
    private readonly IJobScheduler _jobScheduler;
    private readonly AbpBackgroundTasksOptions _options;
    private readonly ILogger<DefaultJobStateChecker> _logger;

    public DefaultJobStateChecker(
        ICurrentTenant currentTenant, 
        IJobStore jobStore,
        IJobScheduler jobScheduler, 
        IOptions<AbpBackgroundTasksOptions> options,
        ILogger<DefaultJobStateChecker> logger)
    {
        _currentTenant = currentTenant;
        _jobStore = jobStore;
        _jobScheduler = jobScheduler;
        _options = options.Value;
        _logger = logger;
    }

    public async virtual Task CheckRuningJobAsync(Guid? tenantId = null, CancellationToken cancellationToken = default)
    {
        try
        {
            using (_currentTenant.Change(tenantId))
            {
                var runingJobs = await _jobStore.GetRuningListAsync(
                        _options.MaxJobCheckCount,
                        _options.NodeName,
                        cancellationToken);

                if (runingJobs.Count == 0)
                {
                    return;
                }

                foreach (var runingJob in runingJobs)
                {
                    // 当标记为运行中的作业不在调度器中时，改变为已停止作业
                    if (!await _jobScheduler.ExistsAsync(runingJob, cancellationToken))
                    {
                        runingJob.Status = JobStatus.Stopped;

                        await _jobStore.StoreAsync(runingJob);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "check job status error.");
        }
    }

    public async virtual Task CleanExpiredJobAsync(Guid? tenantId = null, CancellationToken cancellationToken = default)
    {
        try
        {
            using (_currentTenant.Change(tenantId))
            {
                var expiredJobs = await _jobStore.CleanupAsync(
                    _options.MaxJobCleanCount,
                    _options.JobExpiratime,
                    _options.NodeName,
                    cancellationToken);

                foreach (var expiredJob in expiredJobs)
                {
                    // 从队列强制移除作业
                    await _jobScheduler.RemoveAsync(expiredJob, cancellationToken);
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "cleanup expired job error.");
        }
    }
}
