namespace LINGYUN.Abp.DataProtection.Models;

public class EntityPropertyInfoModel
{
    /// <summary>
    /// 名称
    /// </summary>
    public string Name { get; set; } = default!;
    /// <summary>
    /// 显示名称
    /// </summary>
    public string DisplayName { get; set; } = default!;
    /// <summary>
    /// 类型全名
    /// </summary>
    public string TypeFullName { get; set; } = default!;
    /// <summary>
    /// JavaScript类型
    /// </summary>
    public string JavaScriptType { get; set; } = default!;
    /// <summary>
    /// JavaScript名称
    /// </summary>
    public string JavaScriptName { get; set; } = default!;
    /// <summary>
    /// 枚举列表
    /// </summary>
    public EntityEnumInfoModel[]? Enums { get; set; }
    /// <summary>
    /// 允许的过滤操作列表
    /// </summary>
    public DataAccessFilterOperate[]? Operates { get; set; }
}
