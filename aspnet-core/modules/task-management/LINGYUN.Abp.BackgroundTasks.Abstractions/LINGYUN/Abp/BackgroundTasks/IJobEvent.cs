using System.Threading.Tasks;

namespace LINGYUN.Abp.BackgroundTasks;

/// <summary>
/// 挂载任务事件接口
/// </summary>
public interface IJobEvent
{
    /// <summary>
    /// 任务启动前事件
    /// </summary>
    /// <param name="jobEventData"></param>
    /// <returns></returns>
    Task OnJobBeforeExecuted(JobEventContext context);
    /// <summary>
    /// 任务完成后事件
    /// </summary>
    /// <param name="jobEventData"></param>
    /// <returns></returns>
    Task OnJobAfterExecuted(JobEventContext context);
}
