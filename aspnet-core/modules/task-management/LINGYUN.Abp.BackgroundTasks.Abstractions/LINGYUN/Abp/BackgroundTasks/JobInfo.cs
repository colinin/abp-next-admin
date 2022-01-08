using System;
using System.Collections.Generic;

namespace LINGYUN.Abp.BackgroundTasks;

public class JobInfo
{
    /// <summary>
    /// 任务标识
    /// </summary>
    public Guid Id { get; set; }
    /// <summary>
    /// 任务名称
    /// </summary>
    public string Name { get; set; }
    /// <summary>
    /// 任务分组
    /// </summary>
    public string Group { get; set; }
    /// <summary>
    /// 任务类型
    /// </summary>
    public string Type { get; set; }
    /// <summary>
    /// 返回参数
    /// </summary>
    public string Result { get; set; }
    /// <summary>
    /// 任务参数
    /// </summary>
    public IDictionary<string, object> Args { get; set; }
    /// <summary>
    /// 任务状态
    /// </summary>
    public JobStatus Status { get; set; } = JobStatus.None;
    /// <summary>
    /// 描述
    /// </summary>
    public string Description { get; set; }
    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreationTime { get; set; }
    /// <summary>
    /// 开始时间
    /// </summary>
    public DateTime BeginTime { get; set; }
    /// <summary>
    /// 结束时间
    /// </summary>
    public DateTime? EndTime { get; set; }
    /// <summary>
    /// 上次运行时间
    /// </summary>
    public DateTime? LastRunTime { get; set; }
    /// <summary>
    /// 下一次执行时间
    /// </summary>
    public DateTime? NextRunTime { get; set; }
    /// <summary>
    /// 任务类别
    /// </summary>
    public JobType JobType { get; set; } = JobType.Once;
    /// <summary>
    /// Cron表达式，如果是周期性任务需要指定
    /// </summary>
    public string Cron { get; set; }
    /// <summary>
    /// 触发次数
    /// </summary>
    public int TriggerCount { get; set; }
    /// <summary>
    /// 失败重试次数
    /// </summary>
    public int TryCount { get; set; }
    /// <summary>
    /// 失败重试上限
    /// 默认：50
    /// </summary>
    public int MaxTryCount { get; set; } = 50;
    /// <summary>
    /// 最大执行次数
    /// 默认：0, 无限制
    /// </summary>
    public int MaxCount { get; set; }
    /// <summary>
    /// 连续失败且不会再次执行
    /// </summary>
    public bool IsAbandoned { get; set; }
    /// <summary>
    /// 间隔时间，单位秒，与Cron表达式冲突
    /// 默认: 300
    /// </summary>
    public int Interval { get; set; } = 300;
    /// <summary>
    /// 任务优先级
    /// </summary>
    public JobPriority Priority { get; set; } = JobPriority.Normal;
    /// <summary>
    /// 任务独占超时时长（秒）
    /// 0或更小不生效
    /// </summary>
    public int LockTimeOut { get; set; }
}
