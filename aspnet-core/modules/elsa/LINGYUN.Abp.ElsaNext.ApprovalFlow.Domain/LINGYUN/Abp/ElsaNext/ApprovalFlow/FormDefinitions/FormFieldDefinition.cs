using System;
using Volo.Abp;
using Volo.Abp.Domain.Entities;

namespace LINGYUN.Abp.ElsaNext.ApprovalFlow.FormDefinitions;

public class FormFieldDefinition : Entity<Guid>
{
    public Guid FormDefinitionId { get; private set; }

    public string Key { get; private set; }

    public string Label { get; private set; }

    public string Type { get; private set; }

    public string Permission { get; private set; }

    public int Order { get; private set; }

    public bool IsRequired { get; private set; }

    public string? Hint { get; private set; }

    public string? OptionsJson { get; private set; }

    private FormFieldDefinition()
    {
        Key = "";
        Label = "";
        Type = FormFieldTypes.SingleLineText;
        Permission = FormFieldPermissions.Editable;
    }

    internal FormFieldDefinition(
        Guid id,
        Guid formDefinitionId,
        string key,
        string label,
        string type,
        string permission,
        int order,
        bool isRequired,
        string? hint,
        string? optionsJson)
        : base(id)
    {
        FormDefinitionId = formDefinitionId;
        Key = Check.NotNullOrWhiteSpace(
            key,
            nameof(key),
            FormFieldDefinitionConsts.MaxKeyLength);
        Label = Check.NotNullOrWhiteSpace(
            label,
            nameof(label),
            FormFieldDefinitionConsts.MaxLabelLength);
        Type = Check.NotNullOrWhiteSpace(
            type,
            nameof(type),
            FormFieldDefinitionConsts.MaxTypeLength);
        Permission = Check.NotNullOrWhiteSpace(
            permission,
            nameof(permission),
            FormFieldDefinitionConsts.MaxPermissionLength);
        Order = Check.Range(order, nameof(order), 0, int.MaxValue);
        IsRequired = isRequired;
        Hint = Check.Length(
            hint,
            nameof(hint),
            FormFieldDefinitionConsts.MaxHintLength);
        OptionsJson = Check.Length(
            optionsJson,
            nameof(optionsJson),
            FormFieldDefinitionConsts.MaxOptionsJsonLength);

        CheckTypeAndPermission();
    }

    internal void Update(
        string label,
        string permission,
        int order,
        bool isRequired,
        string? hint,
        string? optionsJson)
    {
        Label = Check.NotNullOrWhiteSpace(
            label,
            nameof(label),
            FormFieldDefinitionConsts.MaxLabelLength);
        Permission = Check.NotNullOrWhiteSpace(
            permission,
            nameof(permission),
            FormFieldDefinitionConsts.MaxPermissionLength);
        Order = Check.Range(order, nameof(order), 0, int.MaxValue);
        IsRequired = isRequired;
        Hint = Check.Length(
            hint,
            nameof(hint),
            FormFieldDefinitionConsts.MaxHintLength);
        OptionsJson = Check.Length(
            optionsJson,
            nameof(optionsJson),
            FormFieldDefinitionConsts.MaxOptionsJsonLength);

        CheckTypeAndPermission();
    }

    private void CheckTypeAndPermission()
    {
        if (!FormFieldTypes.IsKnown(Type))
        {
            throw new BusinessException(ElsaNextApprovalFlowErrorCodes.FormField.InvalidType);
        }
        if (!FormFieldPermissions.IsKnown(Permission))
        {
            throw new BusinessException(ElsaNextApprovalFlowErrorCodes.FormField.InvalidPermission);
        }
    }
}
