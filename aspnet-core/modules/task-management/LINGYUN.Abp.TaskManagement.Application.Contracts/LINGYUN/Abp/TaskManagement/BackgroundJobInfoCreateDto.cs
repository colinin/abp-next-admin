using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Validation;

namespace LINGYUN.Abp.TaskManagement;

public class BackgroundJobInfoCreateDto : BackgroundJobInfoCreateOrUpdateDto
{
    /// <summary>
    /// 任务名称
    /// </summary>
    [Required]
    [DynamicStringLength(typeof(BackgroundJobInfoConsts), nameof(BackgroundJobInfoConsts.MaxNameLength))] 
    public string Name { get; set; }
    /// <summary>
    /// 任务分组
    /// </summary>
    [Required]
    [DynamicStringLength(typeof(BackgroundJobInfoConsts), nameof(BackgroundJobInfoConsts.MaxGroupLength))] 
    public string Group { get; set; }
    /// <summary>
    /// 任务类型
    /// </summary>
    [Required]
    [DynamicStringLength(typeof(BackgroundJobInfoConsts), nameof(BackgroundJobInfoConsts.MaxTypeLength))] 
    public string Type { get; set; }
    /// <summary>
    /// 开始时间
    /// </summary>
    [Required]
    public DateTime BeginTime { get; set; }
    /// <summary>
    /// 结束时间
    /// </summary>
    public DateTime? EndTime { get; set; }
}
