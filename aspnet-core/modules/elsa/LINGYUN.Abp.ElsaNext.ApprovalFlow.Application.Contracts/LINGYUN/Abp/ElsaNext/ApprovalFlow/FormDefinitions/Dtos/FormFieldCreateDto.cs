using System.ComponentModel.DataAnnotations;
using Volo.Abp.Validation;

namespace LINGYUN.Abp.ElsaNext.ApprovalFlow.FormDefinitions.Dtos;

public class FormFieldCreateDto
{
    [Required]
    [DynamicStringLength(typeof(FormFieldDefinitionConsts), nameof(FormFieldDefinitionConsts.MaxKeyLength))]
    public string Key { get; set; } = "";

    [Required]
    [DynamicStringLength(typeof(FormFieldDefinitionConsts), nameof(FormFieldDefinitionConsts.MaxLabelLength))]
    public string Label { get; set; } = "";

    [Required]
    [DynamicStringLength(typeof(FormFieldDefinitionConsts), nameof(FormFieldDefinitionConsts.MaxTypeLength))]
    public string Type { get; set; } = FormFieldTypes.SingleLineText;

    [Required]
    [DynamicStringLength(typeof(FormFieldDefinitionConsts), nameof(FormFieldDefinitionConsts.MaxPermissionLength))]
    public string Permission { get; set; } = FormFieldPermissions.Editable;

    public int Order { get; set; }

    public bool IsRequired { get; set; }

    [DynamicStringLength(typeof(FormFieldDefinitionConsts), nameof(FormFieldDefinitionConsts.MaxHintLength))]
    public string? Hint { get; set; }

    [DynamicStringLength(typeof(FormFieldDefinitionConsts), nameof(FormFieldDefinitionConsts.MaxOptionsJsonLength))]
    public string? OptionsJson { get; set; }
}
