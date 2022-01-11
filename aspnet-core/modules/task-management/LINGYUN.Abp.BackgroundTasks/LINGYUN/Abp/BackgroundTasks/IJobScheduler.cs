using System.Threading.Tasks;
using System.Collections.Generic;

namespace LINGYUN.Abp.BackgroundTasks;
/// <summary>
/// 作业调度接口
/// </summary>
public interface IJobScheduler
{
    /// <summary>
    /// 任务入队
    /// </summary>
    /// <param name="job"></param>
    /// <returns></returns>
    Task<bool> QueueAsync(JobInfo job);
    /// <summary>
    /// 任务入队
    /// </summary>
    /// <param name="jobs"></param>
    /// <returns></returns>
    Task QueuesAsync(IEnumerable<JobInfo> jobs);
    /// <summary>
    /// 任务是否存在
    /// </summary>
    /// <param name="group"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    Task<bool> ExistsAsync(JobInfo job);
    /// <summary>
    /// 触发任务
    /// </summary>
    /// <param name="group"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    Task TriggerAsync(JobInfo job);
    /// <summary>
    /// 暂停任务
    /// </summary>
    /// <param name="group"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    Task PauseAsync(JobInfo job);
    /// <summary>
    /// 恢复暂停的任务
    /// </summary>
    /// <param name="group"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    Task ResumeAsync(JobInfo job);
    /// <summary>
    /// 移除任务
    /// </summary>
    /// <param name="group"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    Task<bool> RemoveAsync(JobInfo job);
    /// <summary>
    /// 启动任务协调器
    /// </summary>
    /// <returns></returns>
    Task<bool> StartAsync();
    /// <summary>
    /// 停止任务协调器
    /// </summary>
    /// <returns></returns>
    Task<bool> StopAsync();
    /// <summary>
    /// 释放任务协调器
    /// </summary>
    /// <returns></returns>
    Task<bool> ShutdownAsync();
}
