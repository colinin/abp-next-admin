using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.Guids;

namespace LINGYUN.Abp.DataProtectionManagement;
public class EntityTypeInfo : AuditedAggregateRoot<Guid>
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
    /// 是否启用数据审计
    /// </summary>
    public virtual bool IsAuditEnabled { get; protected set; }
    /// <summary>
    /// 实体属性列表
    /// </summary>
    public virtual ICollection<EntityPropertyInfo> Properties { get; protected set; }
    protected EntityTypeInfo()
    {
        Properties = new Collection<EntityPropertyInfo>();
    }

    public EntityTypeInfo(
        Guid id,
        string name, 
        string displayName, 
        string typeFullName, 
        bool isAuditEnabled = true)
        :base(id)
    {
        Name = Check.NotNullOrWhiteSpace(name, nameof(name), EntityTypeInfoConsts.MaxNameLength);
        DisplayName = Check.NotNullOrWhiteSpace(displayName, nameof(displayName), EntityTypeInfoConsts.MaxDisplayNameLength);
        TypeFullName = Check.NotNullOrWhiteSpace(typeFullName, nameof(typeFullName), EntityTypeInfoConsts.MaxTypeFullNameLength);
        IsAuditEnabled = isAuditEnabled;
        Properties = new Collection<EntityPropertyInfo>();
    }

    public EntityPropertyInfo FindProperty(string name)
    {
        return Properties.FirstOrDefault(x => x.Name == name);
    }

    public void RemoveProperty(string name)
    {
        Properties.RemoveAll(x => x.Name == name);
    }

    public EntityPropertyInfo AddProperty(
        IGuidGenerator guidGenerator,
        string name,
        string displayName,
        string typeFullName,
        string valueRange = null)
    {
        if (HasExistsProperty(name))
        {
            throw new BusinessException(DataProtectionManagementErrorCodes.EntityTypeInfo.DuplicateProperty)
                .WithData(nameof(EntityPropertyInfo.Name), name);
        }

        var propertyInfo = new EntityPropertyInfo(
            guidGenerator.Create(),
            Id,
            name,
            displayName,
            typeFullName,
            valueRange);

        Properties.Add(propertyInfo);

        return propertyInfo;
    }

    public bool HasExistsProperty(string name)
    {
        return Properties.Any(x => x.Name == name);
    }
}
