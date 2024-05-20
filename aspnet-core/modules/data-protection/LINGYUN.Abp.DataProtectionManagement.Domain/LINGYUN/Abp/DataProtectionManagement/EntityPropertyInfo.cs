using System;
using Volo.Abp;
using Volo.Abp.Domain.Entities;

namespace LINGYUN.Abp.DataProtectionManagement;

public class EntityPropertyInfo : Entity<Guid>
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
    /// 类型全名
    /// </summary>
    public virtual string TypeFullName { get; protected set; }
    /// <summary>
    /// 数据值范围集合（主要针对枚举类型）
    /// </summary>
    public virtual string ValueRange { get; protected set; }
    /// <summary>
    /// 所属类型
    /// </summary>
    public virtual EntityTypeInfo TypeInfo { get; protected set; }
    /// <summary>
    /// 所属类型标识
    /// </summary>
    public virtual Guid TypeInfoId { get; protected set; }

    protected EntityPropertyInfo()
    {
    }

    public EntityPropertyInfo(
        Guid id,
        Guid typeInfoId,
        string name, 
        string displayName, 
        string typeFullName,
        string valueRange = null)
        : base(id)
    {
        Name = Check.NotNullOrWhiteSpace(name, nameof(name), EntityPropertyInfoConsts.MaxNameLength);
        DisplayName = Check.NotNullOrWhiteSpace(displayName, nameof(displayName), EntityPropertyInfoConsts.MaxDisplayNameLength);
        TypeFullName = Check.NotNullOrWhiteSpace(typeFullName, nameof(typeFullName), EntityPropertyInfoConsts.MaxTypeFullNameLength);
        ValueRange = Check.Length(valueRange, nameof(valueRange), EntityPropertyInfoConsts.MaxValueRangeLength);
        TypeInfoId = typeInfoId;
    }
}
