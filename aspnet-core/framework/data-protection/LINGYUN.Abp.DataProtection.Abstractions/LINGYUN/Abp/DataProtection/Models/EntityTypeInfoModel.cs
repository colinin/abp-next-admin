using System.Collections.Generic;

namespace LINGYUN.Abp.DataProtection.Models;

public class EntityTypeInfoModel
{
    /// <summary>
    /// 实体名称
    /// </summary>
    public string Name { get; set; }
    /// <summary>
    /// 显示名称
    /// </summary>
    public string DisplayName { get; set; }
    /// <summary>
    /// 可访问属性列表
    /// </summary>
    public List<EntityPropertyInfoModel> Properties { get; set; } = new List<EntityPropertyInfoModel>();
}
