using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Specifications;

namespace LINGYUN.Abp.TaskManagement;

public interface IBackgroundJobLogRepository : IRepository<BackgroundJobLog, long>
{
    /// <summary>
    /// 获取过滤后的任务日志数量
    /// </summary>
    /// <param name="specification"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<int> GetCountAsync(
        ISpecification<BackgroundJobLog> specification,
        CancellationToken cancellationToken = default);
    /// <summary>
    /// 获取过滤后的任务日志列表
    /// </summary>
    /// <param name="specification"></param>
    /// <param name="sorting"></param>
    /// <param name="maxResultCount"></param>
    /// <param name="skipCount"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<List<BackgroundJobLog>> GetListAsync(
        ISpecification<BackgroundJobLog> specification,
        string sorting = $"{nameof(BackgroundJobLog.RunTime)} DESC",
        int maxResultCount = 10,
        int skipCount = 0,
        CancellationToken cancellationToken = default);
}
