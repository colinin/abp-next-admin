using System.ComponentModel.DataAnnotations;
using Volo.Abp.Validation;

namespace LINGYUN.Abp.ElsaNext.ApprovalFlow.FormDefinitions.Dtos;

public class FormDefinitionCreateDto
{
    [Required]
    [DynamicStringLength(typeof(FormDefinitionConsts), nameof(FormDefinitionConsts.MaxNameLength))]
    public string Name { get; set; } = "";

    [Required]
    [DynamicStringLength(typeof(FormDefinitionConsts), nameof(FormDefinitionConsts.MaxCodeLength))]
    public string Code { get; set; } = "";

    public int Version { get; set; } = 1;

    [DynamicStringLength(typeof(FormDefinitionConsts), nameof(FormDefinitionConsts.MaxDescriptionLength))]
    public string? Description { get; set; }
}
