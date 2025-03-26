using System;
using Volo.Abp.Application.Dtos;

namespace LINGYUN.Abp.DataProtectionManagement;

public class EntityEnumInfoDto : EntityDto<Guid>
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
    /// 枚举值
    /// </summary>
    public string Value { get; set; }
}
