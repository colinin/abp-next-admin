using System;
using Volo.Abp;

namespace LINGYUN.Abp.BackgroundTasks;
public class JobTypeSelector : NamedTypeSelector
{
    public JobTypeSelector(
        string name,
        Func<Type, bool> predicate,
        int? lockTimeOut = null, 
        string nodeName = null, 
        string cron = null,
        JobPriority? priority = null, 
        int? interval = null,
        int? maxCount = null, 
        int? maxTryCount = null)
        : base(name, predicate)
    {
        LockTimeOut = lockTimeOut;
        NodeName = nodeName;
        Cron = cron;
        Priority = priority;
        Interval = interval;
        MaxCount = maxCount;
        MaxTryCount = maxTryCount;
    }

    /// <summary>
    /// 任务独占超时时长（秒）
    /// 0或更小不生效
    /// </summary>
    public int? LockTimeOut { get; set; }
    /// <summary>
    /// 指定运行节点
    /// </summary>
    public string NodeName { get; set; }
    /// <summary>
    /// 任务优先级
    /// </summary>
    public JobPriority? Priority { get; set; }
    /// <summary>
    /// Cron表达式，如果是周期性任务需要指定
    /// </summary>
    public string Cron { get; set; }
    /// <summary>
    /// 间隔时间，单位秒，与Cron表达式冲突
    /// </summary>
    public int? Interval { get; set; }
    /// <summary>
    /// 最大触发次数
    /// </summary>
    public int? MaxCount { get; set; }
    /// <summary>
    /// 失败重试上限
    /// 默认：50
    /// </summary>
    public int? MaxTryCount { get; set; }
}
