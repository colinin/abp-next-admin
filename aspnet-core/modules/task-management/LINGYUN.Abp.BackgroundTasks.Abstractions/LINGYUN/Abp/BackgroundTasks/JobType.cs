namespace LINGYUN.Abp.BackgroundTasks;
/// <summary>
/// 任务类别
/// </summary>
public enum JobType
{
    /// <summary>
    /// 一次性
    /// </summary>
    Once,
    /// <summary>
    /// 周期性
    /// </summary>
    Period,
    /// <summary>
    /// 持续性
    /// </summary>
    Persistent
}
