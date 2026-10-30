using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Abp.ElsaNext.ApprovalFlow.FormDefinitions;

public class FormDefinition : FullAuditedAggregateRoot<Guid>, IMultiTenant
{
    public Guid? TenantId { get; private set; }

    public string Name { get; private set; }

    public string Code { get; private set; }

    public int Version { get; private set; }

    public string? Description { get; private set; }

    public bool IsPublished { get; private set; }

    public DateTimeOffset? PublishedTime { get; private set; }

    public virtual ICollection<FormFieldDefinition> Fields { get; private set; }

    private FormDefinition()
    {
        Name = "";
        Code = "";

        Fields = new Collection<FormFieldDefinition>();
    }

    public FormDefinition(Guid id, string name, string code, int version = 1, string? description = null)
        : base(id)
    {
        Name = Check.NotNullOrWhiteSpace(
            name,
            nameof(name),
            FormDefinitionConsts.MaxNameLength);
        Code = Check.NotNullOrWhiteSpace(
            code,
            nameof(code),
            FormDefinitionConsts.MaxCodeLength);
        Version = Check.Positive(version, nameof(version));
        Description = Check.Length(
            description,
            nameof(description),
            FormDefinitionConsts.MaxDescriptionLength);

        Fields = new Collection<FormFieldDefinition>();
    }

    public void Update(string name, string? description)
    {
        EnsureDraft();
        Name = Check.NotNullOrWhiteSpace(
            name,
            nameof(name),
            FormDefinitionConsts.MaxNameLength);
        Description = Check.Length(
            description,
            nameof(description),
            FormDefinitionConsts.MaxDescriptionLength);
    }

    public FormFieldDefinition AddField(
        Guid fieldId,
        string key,
        string label,
        string type,
        string permission,
        int order,
        bool isRequired,
        string? hint = null,
        string? optionsJson = null)
    {
        EnsureDraft();

        if (Fields.Count >= FormDefinitionConsts.MaxFieldCount)
        {
            throw new BusinessException(ElsaNextApprovalFlowErrorCodes.FormField.KeyAlreadyExists)
                ;
        }
        if (Fields.Any(field => field.Key == key))
        {
            throw new BusinessException(ElsaNextApprovalFlowErrorCodes.FormField.KeyAlreadyExists)
                ;
        }

        var field = new FormFieldDefinition(
            fieldId,
            Id,
            key,
            label,
            type,
            permission,
            order,
            isRequired,
            hint,
            optionsJson);
        Fields.Add(field);
        return field;
    }

    public FormFieldDefinition UpdateField(
        Guid fieldId,
        string label,
        string permission,
        int order,
        bool isRequired,
        string? hint = null,
        string? optionsJson = null)
    {
        EnsureDraft();
        var field = FindField(fieldId);
        field.Update(label, permission, order, isRequired, hint, optionsJson);
        return field;
    }

    public void RemoveField(Guid fieldId)
    {
        EnsureDraft();
        Fields.Remove(FindField(fieldId));
    }

    public void Publish(DateTimeOffset publishedTime)
    {
        if (IsPublished)
        {
            throw new BusinessException(ElsaNextApprovalFlowErrorCodes.FormDefinition.AlreadyPublished);
        }
        if (Fields.Count == 0)
        {
            throw new BusinessException(ElsaNextApprovalFlowErrorCodes.FormDefinition.NoField);
        }

        IsPublished = true;
        PublishedTime = publishedTime;
    }

    private FormFieldDefinition FindField(Guid fieldId)
    {
        return Fields.FirstOrDefault(field => field.Id == fieldId)
            ?? throw new BusinessException(ElsaNextApprovalFlowErrorCodes.FormField.NotFound);
    }

    private void EnsureDraft()
    {
        if (IsPublished)
        {
            throw new BusinessException(ElsaNextApprovalFlowErrorCodes.FormDefinition.AlreadyPublished);
        }
    }
}
