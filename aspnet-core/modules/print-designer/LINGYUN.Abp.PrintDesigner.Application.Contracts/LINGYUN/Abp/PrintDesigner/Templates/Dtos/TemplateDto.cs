using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;

namespace LINGYUN.Abp.PrintDesigner.Templates.Dtos;

public class TemplateDto : AuditedEntityDto<Guid>, IHasConcurrencyStamp
{
    /// <summary>
    /// 名称
    /// </summary>
    public string Name { get; set; } = default!;
    /// <summary>
    /// 描述
    /// </summary>
    public string? Description { get; set; }
    /// <summary>
    /// 时间戳（毫秒）
    /// </summary>
    public long? UpdatedAt { get; set; }
    /// <summary>
    /// 模板设计数据
    /// </summary>
    public Dictionary<string, object?>? Data { get; set; }
    /// <summary>
    /// 扩展数据容器
    /// </summary>
    public Dictionary<string, object?> Ext { get; set; } = new Dictionary<string, object?>();
    /// <summary>
    /// 用于控制前端操作按钮权限
    /// </summary>
    public TemplatePermissionDto? Permissions { get; set; }
    /// <summary>
    /// 并发令牌
    /// </summary>
    public string ConcurrencyStamp { get; set; } = default!;
}
