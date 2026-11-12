using System;
using Volo.Abp;
using Volo.Abp.Data;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Abp.PrintDesigner.Templates;
/// <summary>
/// 模板
/// </summary>
public class Template : AuditedAggregateRoot<Guid>, IMultiTenant
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
    /// 模板数据
    /// </summary>
    public virtual TemplateData? Data { get; private set; }

    protected Template()
    {
        ExtraProperties = new ExtraPropertyDictionary();
        this.SetDefaultsForExtraProperties();
    }

    public Template(
        Guid id,
        string name,
        string? description = null,
        Guid? tenantId = null)
        : base(id)
    {
        SetName(name);
        Description = Check.Length(description, nameof(description), TemplateConsts.MaxDescriptionLength);
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
        Name = Check.NotNullOrWhiteSpace(name, nameof(name), TemplateConsts.MaxNameLength);
    }

    /// <summary>
    /// 设置模板数据
    /// </summary>
    /// <param name="designerData"></param>
    /// <param name="extendedData"></param>
    public void SetData(string designerData, string? extendedData = null)
    {
        Data ??= new TemplateData(Id, TenantId);

        Data.SetDesignerData(designerData);
        Data.SetExtendedData(extendedData);
    }
}
