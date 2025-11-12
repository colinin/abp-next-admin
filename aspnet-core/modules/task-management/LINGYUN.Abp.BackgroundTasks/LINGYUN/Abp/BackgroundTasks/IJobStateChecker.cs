using System;
using System.Threading;
using System.Threading.Tasks;

namespace LINGYUN.Abp.BackgroundTasks;
/// <summary>
/// 作业状态检查接口
/// </summary>
public interface IJobStateChecker
{
    /// <summary>
    /// 清理过期作业
    /// </summary>
    /// <param name="tenantId">租户标识</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task CleanExpiredJobAsync(
        Guid? tenantId = null,
        CancellationToken cancellationToken = default);
    /// <summary>
    /// 检查运行中的作业
    /// </summary>
    /// <param name="tenantId">租户标识</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task CheckRuningJobAsync(
        Guid? tenantId = null,
        CancellationToken cancellationToken = default);
}
