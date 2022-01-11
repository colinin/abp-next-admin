namespace LINGYUN.Abp.BackgroundTasks;

public enum JobStatus
{
    /// <summary>
    /// 未知
    /// </summary>
    None = -1,
    /// <summary>
    /// 已完成
    /// </summary>
    Completed = 0,
    /// <summary>
    /// 运行中
    /// </summary>
    Running = 10,
    /// <summary>
    /// 失败重试
    /// </summary>
    FailedRetry = 15,
    /// <summary>
    /// 已暂停
    /// </summary>
    Paused = 20,
    /// <summary>
    /// 已停止
    /// </summary>
    Stopped = 30
}
