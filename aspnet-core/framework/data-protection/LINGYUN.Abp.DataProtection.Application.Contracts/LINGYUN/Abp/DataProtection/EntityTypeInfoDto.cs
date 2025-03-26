namespace LINGYUN.Abp.DataProtection;

public class EntityTypeInfoDto
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
    public EntityPropertyInfoDto[] Properties { get; set; } = new EntityPropertyInfoDto[0];
}
