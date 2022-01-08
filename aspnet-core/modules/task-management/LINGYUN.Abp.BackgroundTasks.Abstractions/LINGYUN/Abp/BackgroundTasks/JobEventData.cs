using System;

namespace LINGYUN.Abp.BackgroundTasks;

public class JobEventData
{
    /// <summary>
    /// 任务类别
    /// </summary>
    public Type Type { get; }
    /// <summary>
    /// 任务组别
    /// </summary>
    public string Group { get; }
    /// <summary>
    /// 任务名称
    /// </summary>
    public string Name { get; }
    /// <summary>
    /// 任务标识
    /// </summary>
    public Guid Key { get; }
    /// <summary>
    /// 任务状态
    /// </summary>
    public JobStatus Status { get; set; }
    /// <summary>
    /// 执行者租户
    /// </summary>
    public Guid? TenantId { get; set; }
    /// <summary>
    /// 错误明细
    /// </summary>
    public Exception Exception { get; }
    /// <summary>
    /// 任务描述
    /// </summary>
    public string Description { get; set; }
    /// <summary>
    /// 返回参数
    /// </summary>
    public string Result { get; set; }
    /// <summary>
    /// 触发次数
    /// </summary>
    public int Triggered { get; set; }
    /// <summary>
    /// 最大可执行次数
    /// </summary>
    public int RepeatCount { get; set; }
    /// <summary>
    /// 失败重试上限
    /// 默认：50
    /// </summary>
    public int TryCount { get; set; }
    /// <summary>
    /// 最大执行次数
    /// 默认：0, 无限制
    /// </summary>
    public int MaxCount { get; set; }
    /// <summary>
    /// 运行时间
    /// </summary>
    public DateTime RunTime { get; set; }
    /// <summary>
    /// 上次运行时间
    /// </summary>
    public DateTime? LastRunTime { get; set; }
    /// <summary>
    /// 下次运行时间
    /// </summary>
    public DateTime? NextRunTime { get; set; }
    /// <summary>
    /// 连续失败且不会再次执行
    /// </summary>
    public bool IsAbandoned { get; set; }
    public JobEventData(
        Guid key,
        Type type, 
        string group, 
        string name,
        Exception exception = null)
    {
        Key = key;
        Type = type;
        Group = group;
        Name = name;
        Exception = exception;
    }
}
