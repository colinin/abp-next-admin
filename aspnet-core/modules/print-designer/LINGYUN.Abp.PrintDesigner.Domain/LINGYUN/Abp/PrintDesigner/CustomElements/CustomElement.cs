using System;
using Volo.Abp;
using Volo.Abp.Data;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Abp.PrintDesigner.CustomElements;
/// <summary>
/// 自定义元素
/// </summary>
public class CustomElement : AuditedAggregateRoot<Guid>, IMultiTenant
{
    public Guid? TenantId { get; private set; }
    /// <summary>
    /// 名称
    /// </summary>
    public string Name { get; private set; } = default!;
    /// <summary>
    /// 描述
    /// </summary>
    public string? Description { get; set; }
    /// <summary>
    /// 元素数据
    /// </summary>
    public virtual CustomElementData? Data { get; private set; }

    protected CustomElement()
    {
        ExtraProperties = new ExtraPropertyDictionary();
        this.SetDefaultsForExtraProperties();
    }

    public CustomElement(
        Guid id,
        string name,
        string? description = null,
        Guid? tenantId = null)
        : base(id)
    {
        SetName(name);
        Description = Check.Length(description, nameof(description), CustomElementConsts.MaxDescriptionLength);
        TenantId = tenantId;

        ExtraProperties = new ExtraPropertyDictionary();
        this.SetDefaultsForExtraProperties();
    }

    /// <summary>
    /// 设置名称
    /// </summary>
    /// <param name="name"></param>
    public void SetName(string name)
    {
        Name = Check.NotNullOrWhiteSpace(name, nameof(name), CustomElementConsts.MaxNameLength);
    }

    /// <summary>
    /// 设置元素数据
    /// </summary>
    /// <param name="elementData"></param>
    /// <param name="extendedData"></param>
    public void SetData(string elementData, string? extendedData = null)
    {
        Data ??= new CustomElementData(Id, TenantId);

        Data.SetDesignerData(elementData);
        Data.SetExtendedData(extendedData);
    }
}
