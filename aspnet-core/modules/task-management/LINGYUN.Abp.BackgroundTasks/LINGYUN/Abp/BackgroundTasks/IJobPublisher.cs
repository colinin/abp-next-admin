using System.Threading;
using System.Threading.Tasks;

namespace LINGYUN.Abp.BackgroundTasks;
/// <summary>
/// 作业发布接口
/// </summary>
/// <remarks>
/// 使用场景: 发布作业到作业调度器(发布作业到当前运行节点)
/// </remarks>
public interface IJobPublisher
{
    Task<bool> PublishAsync(JobInfo job, CancellationToken cancellationToken = default);
}
