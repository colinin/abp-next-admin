using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace LINGYUN.Abp.BackgroundTasks;

public interface IJobStore
{
    Task<List<JobInfo>> GetWaitingListAsync(
        int maxResultCount,
        CancellationToken cancellationToken = default);

    Task<List<JobInfo>> GetAllPeriodTasksAsync(
        CancellationToken cancellationToken = default);

    Task<JobInfo> FindAsync(
        string jobId, 
        CancellationToken cancellationToken = default);

    Task StoreAsync(
        JobInfo jobInfo, 
        CancellationToken cancellationToken = default);

    Task StoreLogAsync(JobEventData eventData);

    Task RemoveAsync(
        string jobId, 
        CancellationToken cancellationToken = default);

    Task<List<JobInfo>> CleanupAsync(
        int maxResultCount,
        TimeSpan jobExpiratime,
        CancellationToken cancellationToken = default);
}
