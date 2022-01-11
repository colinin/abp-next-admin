using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace LINGYUN.Abp.TaskManagement;

public interface IBackgroundJobLogRepository : IRepository<BackgroundJobLog, long>
{
    /// <summary>
    /// 获取过滤后的任务日志数量
    /// </summary>
    /// <param name="filter"></param>
    /// <param name="jobId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<int> GetCountAsync(
        BackgroundJobLogFilter filter,
        Guid? jobId = null,
        CancellationToken cancellationToken = default);
    /// <summary>
    /// 获取过滤后的任务日志列表
    /// </summary>
    /// <param name="filter"></param>
    /// <param name="jobId"></param>
    /// <param name="sorting"></param>
    /// <param name="maxResultCount"></param>
    /// <param name="skipCount"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<List<BackgroundJobLog>> GetListAsync(
        BackgroundJobLogFilter filter,
        Guid? jobId = null,
        string sorting = nameof(BackgroundJobLog.RunTime),
        int maxResultCount = 10,
        int skipCount = 0,
        CancellationToken cancellationToken = default);
}
