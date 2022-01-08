namespace LINGYUN.Abp.BackgroundTasks;
/// <summary>
/// 任务优先级
/// </summary>
/// <remarks>
/// 与框架保持一致
/// </remarks>
public enum JobPriority
{
    Low = 5,
    BelowNormal = 10,
    Normal = 0xF,
    AboveNormal = 20,
    High = 25
}
