using System.ComponentModel.DataAnnotations;
using Volo.Abp.Validation;

namespace LINGYUN.Abp.ElsaNext.ApprovalFlow.Templates.Dtos;

public abstract class TemplateCreateOrUpdateDto
{
    [Required]
    [DynamicStringLength(typeof(ApprovalTemplateConsts), nameof(ApprovalTemplateConsts.MaxDisplayNameLength))]
    public string DisplayName { get; set; } = "";

    [DynamicStringLength(typeof(ApprovalTemplateConsts), nameof(ApprovalTemplateConsts.MaxDescriptionLength))]
    public string? Description { get; set; }

    [DynamicStringLength(typeof(ApprovalTemplateConsts), nameof(ApprovalTemplateConsts.MaxCategoryLength))]
    public string? Category { get; set; }

    [DynamicStringLength(typeof(ApprovalTemplateConsts), nameof(ApprovalTemplateConsts.MaxIconLength))]
    public string? Icon { get; set; }

    [DynamicStringLength(typeof(ApprovalTemplateConsts), nameof(ApprovalTemplateConsts.MaxBusinessKeyHintLength))]
    public string? BusinessKeyHint { get; set; }
}
