using System;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Data;

namespace LINGYUN.Abp.TaskManagement;

public class BackgroundJobActionDto : EntityDto<Guid>
{
    /// <summary>
    /// 作业标识
    /// </summary>
    public string JobId { get; set; }
    /// <summary>
    /// 名称
    /// </summary>
    public string Name { get; set; }
    /// <summary>
    /// 是否启用
    /// </summary>
    public bool IsEnabled { get; set; }
    /// <summary>
    /// 参数
    /// </summary>
    public ExtraPropertyDictionary Paramters { get; set; }
}
