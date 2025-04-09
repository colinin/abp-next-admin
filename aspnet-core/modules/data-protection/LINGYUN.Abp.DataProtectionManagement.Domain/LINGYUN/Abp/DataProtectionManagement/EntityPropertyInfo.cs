using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Volo.Abp;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Guids;

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
    /// Js类型
    /// </summary>
    public virtual string JavaScriptType { get; protected set; }
    /// <summary>
    /// 所属类型
    /// </summary>
    public virtual EntityTypeInfo TypeInfo { get; protected set; }
    /// <summary>
    /// 所属类型标识
    /// </summary>
    public virtual Guid TypeInfoId { get; protected set; }
    /// <summary>
    /// 枚举列表
    /// </summary>
    public virtual ICollection<EntityEnumInfo> Enums { get; protected set; }

    protected EntityPropertyInfo()
    {
        Enums = new Collection<EntityEnumInfo>();
    }

    public EntityPropertyInfo(
        Guid id,
        Guid typeInfoId,
        string name, 
        string displayName, 
        string typeFullName,
        string javaScriptType)
        : base(id)
    {
        Name = Check.NotNullOrWhiteSpace(name, nameof(name), EntityPropertyInfoConsts.MaxNameLength);
        DisplayName = Check.NotNullOrWhiteSpace(displayName, nameof(displayName), EntityPropertyInfoConsts.MaxDisplayNameLength);
        TypeFullName = Check.NotNullOrWhiteSpace(typeFullName, nameof(typeFullName), EntityPropertyInfoConsts.MaxTypeFullNameLength);
        JavaScriptType = Check.NotNullOrWhiteSpace(javaScriptType, nameof(javaScriptType), EntityPropertyInfoConsts.MaxTypeFullNameLength);
        TypeInfoId = typeInfoId;

        Enums = new Collection<EntityEnumInfo>();
    }

    public EntityEnumInfo FindEnum(string name)
    {
        return Enums.FirstOrDefault(x => x.Name == name);
    }

    public void RemoveEnum(string name)
    {
        Enums.RemoveAll(x => x.Name == name);
    }

    public EntityEnumInfo AddEnum(
        IGuidGenerator guidGenerator,
        string name,
        string displayName,
        string value)
    {
        if (HasExistsEnum(name))
        {
            throw new BusinessException(DataProtectionManagementErrorCodes.EntityTypeInfo.DuplicateEnum)
                .WithData(nameof(EntityEnumInfo.Name), name);
        }

        var enumInfo = new EntityEnumInfo(
            guidGenerator.Create(),
            Id,
            name,
            displayName,
            value);

        Enums.Add(enumInfo);

        return enumInfo;
    }

    public bool HasExistsEnum(string name)
    {
        return Enums.Any(x => x.Name == name);
    }
}
