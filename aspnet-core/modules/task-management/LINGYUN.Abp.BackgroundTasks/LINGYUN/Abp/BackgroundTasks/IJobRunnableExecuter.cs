using System.Threading.Tasks;

namespace LINGYUN.Abp.BackgroundTasks;

/// <summary>
/// 定义任务执行者接口
/// 可以通过它实现一些限制（例如分布式锁）
/// </summary>
public interface IJobRunnableExecuter
{
    Task ExecuteAsync(JobRunnableContext context);
}
