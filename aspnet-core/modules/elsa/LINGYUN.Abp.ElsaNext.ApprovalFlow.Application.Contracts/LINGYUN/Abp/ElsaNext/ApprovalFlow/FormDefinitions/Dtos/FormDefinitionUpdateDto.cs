using System.ComponentModel.DataAnnotations;
using Volo.Abp.Validation;

namespace LINGYUN.Abp.ElsaNext.ApprovalFlow.FormDefinitions.Dtos;

public class FormDefinitionUpdateDto
{
    [Required]
    [DynamicStringLength(typeof(FormDefinitionConsts), nameof(FormDefinitionConsts.MaxNameLength))]
    public string Name { get; set; } = "";

    [DynamicStringLength(typeof(FormDefinitionConsts), nameof(FormDefinitionConsts.MaxDescriptionLength))]
    public string? Description { get; set; }
}
