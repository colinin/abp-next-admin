using LINGYUN.Abp.BackgroundTasks;
using System;
using System.Collections.Generic;
using Volo.Abp;
using Volo.Abp.Data;
using Volo.Abp.Domain.Entities.Auditing;

namespace LINGYUN.Abp.TaskManagement;

public class BackgroundJobInfo : AuditedAggregateRoot<Guid>
{
    /// <summary>
    /// 任务名称
    /// </summary>
    public virtual string Name { get; protected set; }
    /// <summary>
    /// 任务分组
    /// </summary>
    public virtual string Group { get; protected set; }
    /// <summary>
    /// 任务类型
    /// </summary>
    public virtual string Type { get; protected set; }
    /// <summary>
    /// 上一次执行结果
    /// </summary>
    public virtual string Result { get; protected set; }
    /// <summary>
    /// 任务参数
    /// </summary>
    public virtual ExtraPropertyDictionary Args { get; protected set; }
    /// <summary>
    /// 任务状态
    /// </summary>
    public virtual JobStatus Status { get; protected set; }
    /// <summary>
    /// 是否启用
    /// </summary>
    public virtual bool IsEnabled { get; set; }
    /// <summary>
    /// 描述
    /// </summary>
    public virtual string Description { get; set; }
    /// <summary>
    /// 任务独占超时时长（秒）
    /// 0或更小不生效
    /// </summary>
    public virtual int LockTimeOut { get; set; }
    /// <summary>
    /// 开始时间
    /// </summary>
    public virtual DateTime BeginTime { get; protected set; }
    /// <summary>
    /// 结束时间
    /// </summary>
    public virtual DateTime? EndTime { get; protected set; }
    /// <summary>
    /// 上次执行时间
    /// </summary>
    public virtual DateTime? LastRunTime { get; protected set; }
    /// <summary>
    /// 下次执行时间
    /// </summary>
    public virtual DateTime? NextRunTime { get; protected set; }
    /// <summary>
    /// 任务类别
    /// </summary>
    public virtual JobType JobType { get; protected set; }
    /// <summary>
    /// Cron表达式，如果是持续任务需要指定
    /// </summary>
    public virtual string Cron { get; protected set; }
    /// <summary>
    /// 任务优先级
    /// </summary>
    public virtual JobPriority Priority { get; protected set; }
    /// <summary>
    /// 触发次数
    /// </summary>
    public virtual int TriggerCount { get; set; }
    /// <summary>
    /// 失败重试次数
    /// </summary>
    public virtual int TryCount { get; set; }
    /// <summary>
    /// 失败重试上限
    /// 默认：50
    /// </summary>
    public virtual int MaxTryCount { get; set; }
    /// <summary>
    /// 最大执行次数
    /// 默认：0, 无限制
    /// </summary>
    public virtual int MaxCount { get; set; }
    /// <summary>
    /// 间隔时间，单位秒，与Cron表达式冲突
    /// 默认: 300
    /// </summary>
    public virtual int Interval { get; protected set; }
    /// <summary>
    /// 连续失败且不会再次执行
    /// </summary>
    public virtual bool IsAbandoned { get; set; }

    protected BackgroundJobInfo() { }

    public BackgroundJobInfo(
        Guid id,
        string name,
        string group,
        string type,
        IDictionary<string, object> args,
        DateTime beginTime,
        DateTime? endTime = null,
        JobPriority priority = JobPriority.Normal,
        int maxCount = 0,
        int maxTryCount = 50) : base(id)
    {
        Name = Check.NotNullOrWhiteSpace(name, nameof(name), BackgroundJobInfoConsts.MaxNameLength);
        Group = Check.NotNullOrWhiteSpace(group, nameof(group), BackgroundJobInfoConsts.MaxGroupLength);
        Type = Check.NotNullOrWhiteSpace(type, nameof(type), BackgroundJobInfoConsts.MaxTypeLength);
        Priority = priority;
        BeginTime = beginTime;
        EndTime = endTime;

        MaxCount = maxCount;
        MaxTryCount = maxTryCount;

        Status = JobStatus.Running;

        // TODO: 是否需要将参数挪到另一个实体? 
        // 任务参数的建议是尽量最小化, 仅存储关键信息
        Args = new ExtraPropertyDictionary();
        Args.AddIfNotContains(args);
    }

    public void SetPeriodJob(string cron)
    {
        Cron = Check.NotNullOrWhiteSpace(cron, nameof(cron), BackgroundJobInfoConsts.MaxCronLength);
        JobType = JobType.Period;
    }

    public void SetOnceJob(int interval)
    {
        Interval = interval;
        JobType = JobType.Once;
    }

    public void SetPersistentJob(int interval)
    {
        Interval = interval;
        JobType = JobType.Persistent;
    }

    public void SetLastRunTime(DateTime? lastRunTime)
    {
        LastRunTime = lastRunTime;
    }

    public void SetNextRunTime(DateTime? nextRunTime)
    {
        NextRunTime = nextRunTime;
    }

    public void SetResult(string result)
    {
        if (result.IsNullOrWhiteSpace())
        {
            return;
        }
        if (result.Length > BackgroundJobInfoConsts.MaxResultLength)
        {
            result = result.Substring(0, BackgroundJobInfoConsts.MaxResultLength - 1);
        }

        Result = result;
    }

    public void SetStatus(JobStatus status)
    {
        Status = status;
    }

    public void SetPriority(JobPriority priority)
    {
        Priority = priority;
    }
}
