using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace LINGYUN.Abp.DataProtectionManagement;
public class EntityPropertyInfoDto : EntityDto<Guid>
{
    /// <summary>
    /// 名称
    /// </summary>
    public string Name { get; set; }
    /// <summary>
    /// 显示名称
    /// </summary>
    public string DisplayName { get; set; }
    /// <summary>
    /// 类型全名
    /// </summary>
    public string TypeFullName { get; set; }
    /// <summary>
    /// Js类型
    /// </summary>
    public string JavaScriptType { get; set; }
    /// <summary>
    /// 枚举列表
    /// </summary>
    public virtual List<EntityEnumInfoDto> Enums { get; set; } = new List<EntityEnumInfoDto>();
}
