using System;

namespace LINGYUN.Abp.ElsaNext.ApprovalFlow.FormDefinitions.Dtos;

public class FormFieldDto
{
    public Guid Id { get; set; }
    public string Key { get; set; } = "";
    public string Label { get; set; } = "";
    public string Type { get; set; } = FormFieldTypes.SingleLineText;
    public string Permission { get; set; } = FormFieldPermissions.Editable;
    public int Order { get; set; }
    public bool IsRequired { get; set; }
    public string? Hint { get; set; }
    public string? OptionsJson { get; set; }
}
