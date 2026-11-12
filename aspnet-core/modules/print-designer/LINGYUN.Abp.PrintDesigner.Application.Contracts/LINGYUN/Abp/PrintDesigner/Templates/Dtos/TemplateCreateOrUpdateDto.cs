using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Validation;

namespace LINGYUN.Abp.PrintDesigner.Templates.Dtos;

public abstract class TemplateCreateOrUpdateDto
{
    /// <summary>
    /// 名称
    /// </summary>
    [Required]
    [DynamicStringLength(typeof(TemplateConsts), nameof(TemplateConsts.MaxNameLength))]
    public string Name { get; set; } = default!;
    /// <summary>
    /// 描述
    /// </summary>
    [DynamicStringLength(typeof(TemplateConsts), nameof(TemplateConsts.MaxDescriptionLength))]
    public string? Description { get; set; }
    /// <summary>
    /// 模板设计数据
    /// </summary>
    [Required]
    public Dictionary<string, object?> Data { get; set; } = new Dictionary<string, object?>();
    /// <summary>
    /// 模板扩展参数容器
    /// </summary>
    public Dictionary<string, object?> Ext { get; set; } = new Dictionary<string, object?>();
}
