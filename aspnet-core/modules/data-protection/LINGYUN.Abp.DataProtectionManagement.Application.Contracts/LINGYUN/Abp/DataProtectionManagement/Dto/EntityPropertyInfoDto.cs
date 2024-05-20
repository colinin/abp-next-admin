using System;
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
    /// 数据值范围集合（主要针对枚举类型）
    /// </summary>
    public string[] ValueRange { get; set; }
}
