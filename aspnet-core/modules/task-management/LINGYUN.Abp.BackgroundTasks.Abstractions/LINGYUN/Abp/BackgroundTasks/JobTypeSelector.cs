using System;
using Volo.Abp;

namespace LINGYUN.Abp.BackgroundTasks;
public class JobTypeSelector : NamedTypeSelector
{
    public JobTypeSelector(
        string name,
        Func<Type, bool> predicate,
        int lockTimeOut = 0, 
        string nodeName = null, 
        string cron = null,
        JobPriority priority = JobPriority.Normal, 
        int interval = 300,
        int tryCount = 0, 
        int maxTryCount = 50)
        : base(name, predicate)
    {
        LockTimeOut = lockTimeOut;
        NodeName = nodeName;
        Cron = cron;
        Priority = priority;
        Interval = interval;
        TryCount = tryCount;
        MaxTryCount = maxTryCount;
    }



    /// <summary>
    /// 任务独占超时时长（秒）
    /// 0或更小不生效
    /// </summary>
    public int LockTimeOut { get; set; }
    /// <summary>
    /// 指定运行节点
    /// </summary>
    public string NodeName { get; set; }
    /// <summary>
    /// 任务优先级
    /// </summary>
    public JobPriority Priority { get; set; } = JobPriority.Normal;
    /// <summary>
    /// Cron表达式，如果是周期性任务需要指定
    /// </summary>
    public string Cron { get; set; }
    /// <summary>
    /// 间隔时间，单位秒，与Cron表达式冲突
    /// 默认: 300
    /// </summary>
    public int Interval { get; set; } = 300;
    /// <summary>
    /// 失败重试次数
    /// </summary>
    public int TryCount { get; set; }
    /// <summary>
    /// 失败重试上限
    /// 默认：50
    /// </summary>
    public int MaxTryCount { get; set; } = 50;
}
