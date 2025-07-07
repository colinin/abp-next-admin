using System.Threading.Tasks;

namespace LINGYUN.Abp.BackgroundTasks.Activities;
/// <summary>
/// 作业行为过滤器
/// </summary>
public interface IJobActionFilter
{
    /// <summary>
    /// 是否允许触发后操作
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    Task<bool> CanAfterExecuted(JobEventContext context);
    /// <summary>
    /// 是否允许触发前操作
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    Task<bool> CanBeforeExecuted(JobEventContext context);
}
