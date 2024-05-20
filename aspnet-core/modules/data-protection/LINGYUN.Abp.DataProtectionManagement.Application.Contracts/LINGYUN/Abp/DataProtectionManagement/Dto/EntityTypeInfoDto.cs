using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace LINGYUN.Abp.DataProtectionManagement;
public class EntityTypeInfoDto : AuditedEntityDto<Guid>
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
    /// 是否启用数据审计
    /// </summary>
    public bool IsAuditEnabled { get; set; }
    /// <summary>
    /// 实体属性列表
    /// </summary>
    public virtual List<EntityPropertyInfoDto> Properties { get; set; } = new List<EntityPropertyInfoDto>();
}
