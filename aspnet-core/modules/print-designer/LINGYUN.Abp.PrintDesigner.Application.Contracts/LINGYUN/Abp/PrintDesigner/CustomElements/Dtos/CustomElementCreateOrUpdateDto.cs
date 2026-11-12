using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Validation;

namespace LINGYUN.Abp.PrintDesigner.CustomElements.Dtos;

public abstract class CustomElementCreateOrUpdateDto
{
    /// <summary>
    /// 元素名称
    /// </summary>
    [Required]
    [DynamicStringLength(typeof(CustomElementConsts), nameof(CustomElementConsts.MaxNameLength))]
    public string Name { get; set; } = default!;
    /// <summary>
    /// 描述
    /// </summary>
    [DynamicStringLength(typeof(CustomElementConsts), nameof(CustomElementConsts.MaxDescriptionLength))]
    public string? Description { get; set; }
    /// <summary>
    /// 元素定义
    /// </summary>
    [Required]
    public Dictionary<string, object?> Element { get; set; } = new Dictionary<string, object?>();
    /// <summary>
    /// 元素扩展参数
    /// </summary>
    public Dictionary<string, object?> Ext { get; set; } = new Dictionary<string, object?>();
}
