using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace LINGYUN.Abp.TaskManagement;

public interface IBackgroundJobInfoRepository : IRepository<BackgroundJobInfo, Guid>
{
    Task<bool> CheckNameAsync(
        string group,
        string name,
        CancellationToken cancellationToken = default);
    /// <summary>
    /// 获取过期任务列表
    /// </summary>
    /// <param name="maxResultCount"></param>
    /// <param name="jobExpiratime"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<List<BackgroundJobInfo>> GetExpiredJobsAsync(
        int maxResultCount,
        TimeSpan jobExpiratime,
        CancellationToken cancellationToken = default);
    /// <summary>
    /// 获取所有周期性任务
    /// 指定了Cron表达式的任务需要作为持续性任务交给任务引擎
    /// </summary>
    /// <returns></returns>
    Task<List<BackgroundJobInfo>> GetAllPeriodTasksAsync(
        CancellationToken cancellationToken = default);
    /// <summary>
    /// 获取等待入队的任务列表
    /// </summary>
    /// <param name="maxResultCount"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<List<BackgroundJobInfo>> GetWaitingListAsync(
        int maxResultCount,
        CancellationToken cancellationToken = default);
    /// <summary>
    /// 获取过滤后的任务数量
    /// </summary>
    /// <param name="filter"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<int> GetCountAsync(
        BackgroundJobInfoFilter filter,
        CancellationToken cancellationToken = default);
    /// <summary>
    /// 获取过滤后的任务列表
    /// </summary>
    /// <param name="filter"></param>
    /// <param name="sorting"></param>
    /// <param name="maxResultCount"></param>
    /// <param name="skipCount"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<List<BackgroundJobInfo>> GetListAsync(
        BackgroundJobInfoFilter filter,
        string sorting = nameof(BackgroundJobInfo.Name),
        int maxResultCount = 10,
        int skipCount = 0,
        CancellationToken cancellationToken = default);
}
