using System.Threading;
using System.Threading.Tasks;

namespace LINGYUN.Abp.BackgroundTasks;

/// <summary>
/// 作业锁定提供者
/// </summary>
public interface IJobLockProvider
{
    Task<bool> TryLockAsync(
        string jobKey,
        int lockSeconds,
        CancellationToken cancellationToken = default);

    Task<bool> TryReleaseAsync(
        string jobKey, 
        CancellationToken cancellationToken = default);
}
