using System.ComponentModel.DataAnnotations;
using Volo.Abp.Validation;

namespace LINGYUN.Abp.ElsaNext.ApprovalFlow.Templates.Dtos;

public class TemplateCreateDto : TemplateCreateOrUpdateDto
{
    [Required]
    [DynamicStringLength(typeof(ApprovalTemplateConsts), nameof(ApprovalTemplateConsts.MaxNameLength))]
    public string Name { get; set; } = "";
}
