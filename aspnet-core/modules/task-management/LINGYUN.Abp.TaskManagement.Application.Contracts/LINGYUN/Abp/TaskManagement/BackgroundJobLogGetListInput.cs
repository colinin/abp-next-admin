using System;
using Volo.Abp.Application.Dtos;

namespace LINGYUN.Abp.TaskManagement;

public class BackgroundJobLogGetListInput : PagedAndSortedResultRequestDto
{
    public Guid? JobId { get; set; }
    /// <summary>
    /// 其他过滤条件
    /// </summary>
    public string Filter { get; set; }
    /// <summary>
    /// 存在异常
    /// </summary>
    public bool? HasExceptions { get; set; }
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
    /// 开始触发时间
    /// </summary>
    public DateTime? BeginRunTime { get; set; }
    /// <summary>
    /// 结束触发时间
    /// </summary>
    public DateTime? EndRunTime { get; set; }
}
