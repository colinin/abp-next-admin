using System.Threading.Tasks;

namespace LINGYUN.Abp.BackgroundTasks;

/// <summary>
/// 作业锁定提供者
/// </summary>
public interface IJobLockProvider
{
    Task<bool> TryLockAsync(
        string jobKey,
        int lockSeconds);

    Task<bool> TryReleaseAsync(
        string jobKey);
}
