using System;
using Volo.Abp;
using Volo.Abp.Domain.Entities;

namespace LINGYUN.Abp.DataProtectionManagement;

public class EntityEnumInfo : Entity<Guid>
{
    /// <summary>
    /// 名称
    /// </summary>
    public virtual string Name { get; protected set; }
    /// <summary>
    /// 显示名称
    /// </summary>
    public virtual string DisplayName { get; protected set; }
    /// <summary>
    /// 枚举值
    /// </summary>
    public virtual string Value { get; protected set; }
    /// <summary>
    /// 所属属性
    /// </summary>
    public virtual EntityPropertyInfo PropertyInfo { get; protected set; }
    /// <summary>
    /// 所属属性标识
    /// </summary>
    public virtual Guid PropertyInfoId { get; protected set; }
    protected EntityEnumInfo()
    {

    }

    public EntityEnumInfo(
        Guid id,
        Guid propertyInfoId,
        string name, 
        string displayName, 
        string value)
        : base(id)
    {
        PropertyInfoId = propertyInfoId;
        Name = Check.NotNullOrWhiteSpace(name, nameof(name), EntityEnumInfoConsts.MaxNameLength);
        DisplayName = Check.NotNullOrWhiteSpace(displayName, nameof(displayName), EntityEnumInfoConsts.MaxDisplayNameLength);
        Value = Check.NotNullOrWhiteSpace(value, nameof(value), EntityEnumInfoConsts.MaxValueLength);
    }
}
