using System.ComponentModel.DataAnnotations;
using Volo.Abp.Validation;

namespace LINGYUN.Abp.TextTemplating;

public class TextTemplateDefinitionCreateDto : TextTemplateDefinitionCreateOrUpdateDto
{
    [Required]
    [DynamicStringLength(typeof(TextTemplateDefinitionConsts), nameof(TextTemplateDefinitionConsts.MaxNameLength))]
    public string Name { get; set; }
}
