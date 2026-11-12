using System;
using Volo.Abp;
using Volo.Abp.Data;
using Volo.Abp.Domain.Entities;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Abp.PrintDesigner.CustomElements;

public class CustomElementData : AggregateRoot<long>, IMultiTenant
{
    public Guid? TenantId { get; private set; }
    /// <summary>
    /// 元素Id
    /// </summary>
    public Guid ElementId { get; private set; }
    /// <summary>
    /// 元素数据
    /// </summary>
    public string? ElementData { get; private set; }
    /// <summary>
    /// 扩展字段
    /// </summary>
    public string? ExtendedData { get; private set; }

    protected CustomElementData()
    {
        ExtraProperties = new ExtraPropertyDictionary();
        this.SetDefaultsForExtraProperties();
    }

    internal CustomElementData(
        Guid elementId,
        Guid? tenantId = null)
    {
        ElementId = elementId;
        TenantId = tenantId;
    }

    /// <summary>
    /// 设置元素数据
    /// </summary>
    /// <param name="elementData"></param>
    public void SetDesignerData(string elementData)
    {
        ElementData = Check.NotNullOrWhiteSpace(elementData, nameof(elementData));
    }

    /// <summary>
    /// 设置扩展字段容器数据
    /// </summary>
    /// <param name="extendedData"></param>
    public void SetExtendedData(string? extendedData = null)
    {
        ExtendedData = extendedData;
    }
}
