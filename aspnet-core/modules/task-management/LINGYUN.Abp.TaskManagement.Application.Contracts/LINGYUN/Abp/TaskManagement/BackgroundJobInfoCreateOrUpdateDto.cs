using LINGYUN.Abp.BackgroundTasks;
using System;
using Volo.Abp.Data;
using Volo.Abp.Validation;

namespace LINGYUN.Abp.TaskManagement;

public abstract class BackgroundJobInfoCreateOrUpdateDto
{
    /// <summary>
    /// 是否启用
    /// </summary>
    public bool IsEnabled { get; set; }
    /// <summary>
    /// 任务参数
    /// </summary>
    public ExtraPropertyDictionary Args { get; set; }
    /// <summary>
    /// 描述
    /// </summary>
    [DynamicStringLength(typeof(BackgroundJobInfoConsts), nameof(BackgroundJobInfoConsts.MaxDescriptionLength))]
    public string Description { get; set; }
    /// <summary>
    /// 任务类别
    /// </summary>
    public JobType JobType { get; set; }
    /// <summary>
    /// Cron表达式，如果是持续任务需要指定
    /// </summary>
    [DynamicStringLength(typeof(BackgroundJobInfoConsts), nameof(BackgroundJobInfoConsts.MaxCronLength))]
    public string Cron { get; set; }
    /// <summary>
    /// 失败重试上限
    /// 默认：50
    /// </summary>
    public int MaxTryCount { get; set; }
    /// <summary>
    /// 最大执行次数
    /// 默认：0, 无限制
    /// </summary>
    public int MaxCount { get; set; }
    /// <summary>
    /// 间隔时间，单位秒，与Cron表达式冲突
    /// 默认: 300
    /// </summary>
    public int Interval { get; set; } = 300;
    /// <summary>
    /// 任务优先级
    /// </summary>
    public JobPriority Priority { get; set; }
    /// <summary>
    /// 任务独占超时时长（秒）
    /// 0或更小不生效
    /// </summary>
    public int LockTimeOut { get; set; }
}
