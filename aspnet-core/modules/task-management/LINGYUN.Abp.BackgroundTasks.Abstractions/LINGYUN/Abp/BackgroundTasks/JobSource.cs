namespace LINGYUN.Abp.BackgroundTasks;
/// <summary>
/// 作业来源
/// </summary>
public enum JobSource
{
    /// <summary>
    /// 未定义
    /// </summary>
    None = -1,
    /// <summary>
    /// 用户
    /// </summary>
    User = 0,
    /// <summary>
    /// 系统内置
    /// </summary>
    System = 10,
}
