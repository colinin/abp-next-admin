namespace LINGYUN.Abp.DataProtection;

public class EntityPropertyInfoDto
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
    /// JavaScript类型
    /// </summary>
    public string JavaScriptType { get; set; }
    /// <summary>
    /// 枚举列表
    /// </summary>
    public EntityEnumInfoDto[] Enums { get; set; } = new EntityEnumInfoDto[0];
    /// <summary>
    /// 允许的过滤操作列表
    /// </summary>
    public DataAccessFilterOperate[] Operates { get; set; }
}
