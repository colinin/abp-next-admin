using System;
using Volo.Abp;
using Volo.Abp.Data;
using Volo.Abp.Domain.Entities;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Abp.PrintDesigner.Templates;

public class TemplateData : AggregateRoot<long>, IMultiTenant
{
    public Guid? TenantId { get; private set; }
    /// <summary>
    /// 模板Id
    /// </summary>
    public Guid TemplateId { get; private set; }
    /// <summary>
    /// 模板设计数据
    /// </summary>
    public string? DesignerData { get; private set; }
    /// <summary>
    /// 扩展字段
    /// </summary>
    public string? ExtendedData { get; private set; }

    protected TemplateData()
    {
        ExtraProperties = new ExtraPropertyDictionary();
        this.SetDefaultsForExtraProperties();
    }

    internal TemplateData(
        Guid templateId,
        Guid? tenantId = null)
    {
        TemplateId = templateId;
        TenantId = tenantId;
    }

    /// <summary>
    /// 设置模板设计数据
    /// </summary>
    /// <param name="designerData"></param>
    public void SetDesignerData(string designerData)
    {
        DesignerData = Check.NotNullOrWhiteSpace(designerData, nameof(designerData));
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
