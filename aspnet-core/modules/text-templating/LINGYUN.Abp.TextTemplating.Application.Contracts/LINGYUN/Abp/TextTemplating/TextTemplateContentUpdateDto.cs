using System.ComponentModel.DataAnnotations;
using Volo.Abp.Validation;

namespace LINGYUN.Abp.TextTemplating;

public class TextTemplateContentUpdateDto
{
    [DynamicStringLength(typeof(TextTemplateConsts), nameof(TextTemplateConsts.MaxCultureLength))]
    public string Culture { get; set; }

    [Required]
    [DynamicStringLength(typeof(TextTemplateConsts), nameof(TextTemplateConsts.MaxContentLength))]
    public string Content { get; set; }
}
