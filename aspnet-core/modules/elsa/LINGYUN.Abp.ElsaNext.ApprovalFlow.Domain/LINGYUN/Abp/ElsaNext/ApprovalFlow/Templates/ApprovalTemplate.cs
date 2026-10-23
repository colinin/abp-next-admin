using Volo.Abp;
using System;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Abp.ElsaNext.ApprovalFlow.Templates;

public class ApprovalTemplate : FullAuditedAggregateRoot<Guid>, IMultiTenant
{
    public Guid? TenantId { get; private set; }

    public string Name { get; private set; }

    public string DisplayName { get; private set; }

    public string? Description { get; private set; }

    public string? Category { get; private set; }

    public string? Icon { get; private set; }

    public bool IsPublished { get; private set; }

    public bool IsEnabled { get; private set; }

    public string? ElsaDefinitionId { get; private set; }

    public string? ElsaDefinitionKey { get; private set; }

    public int? ElsaDefinitionVersion { get; private set; }

    public Guid? FormDefinitionId { get; private set; }

    public string? BusinessKeyHint { get; private set; }

    public string? FlowDefinitionJson { get; private set; }

    private ApprovalTemplate()
    {
        Name = "";
        DisplayName = "";
    }

    public ApprovalTemplate(
        Guid id,
        string name,
        string displayName,
        string? description = null,
        string? category = null,
        string? icon = null,
        string? businessKeyHint = null)
        : base(id)
    {
        Name = Check.NotNullOrWhiteSpace(name, nameof(name), ApprovalTemplateConsts.MaxNameLength);
        DisplayName = Check.NotNullOrWhiteSpace(displayName, nameof(displayName), ApprovalTemplateConsts.MaxDisplayNameLength);
        Description = Check.Length(description, nameof(description), ApprovalTemplateConsts.MaxDescriptionLength);
        Category = Check.Length(category, nameof(category), ApprovalTemplateConsts.MaxCategoryLength);
        Icon = Check.Length(icon, nameof(icon), ApprovalTemplateConsts.MaxIconLength);
        BusinessKeyHint = Check.Length(businessKeyHint, nameof(businessKeyHint), ApprovalTemplateConsts.MaxBusinessKeyHintLength);
        IsEnabled = true;
        IsPublished = false;
    }

    public void Update(
        string displayName,
        string? description,
        string? category,
        string? icon,
        string? businessKeyHint)
    {
        DisplayName = Check.NotNullOrWhiteSpace(displayName, nameof(displayName), ApprovalTemplateConsts.MaxDisplayNameLength);
        Description = Check.Length(description, nameof(description), ApprovalTemplateConsts.MaxDescriptionLength);
        Category = Check.Length(category, nameof(category), ApprovalTemplateConsts.MaxCategoryLength);
        Icon = Check.Length(icon, nameof(icon), ApprovalTemplateConsts.MaxIconLength);
        BusinessKeyHint = Check.Length(businessKeyHint, nameof(businessKeyHint), ApprovalTemplateConsts.MaxBusinessKeyHintLength);
    }

    public void Publish(string elsaDefinitionId, string? elsaDefinitionKey, int version)
    {
        ElsaDefinitionId = Check.NotNullOrWhiteSpace(elsaDefinitionId, nameof(elsaDefinitionId), ApprovalTemplateConsts.MaxElsaDefinitionIdLength);
        ElsaDefinitionKey = elsaDefinitionKey;
        ElsaDefinitionVersion = version;
        IsPublished = true;
        IsEnabled = true;
    }

    public void SetFlowDefinitionJson(string? flowDefinitionJson)
    {
        FlowDefinitionJson = flowDefinitionJson;
    }

    public void Disable()
    {
        IsEnabled = false;
    }

    public void Enable()
    {
        IsEnabled = true;
    }

    public void BindFormDefinition(Guid? formDefinitionId)
    {
        FormDefinitionId = formDefinitionId;
    }

    public bool CanStart()
    {
        return IsPublished && IsEnabled && !string.IsNullOrWhiteSpace(ElsaDefinitionId);
    }
}
