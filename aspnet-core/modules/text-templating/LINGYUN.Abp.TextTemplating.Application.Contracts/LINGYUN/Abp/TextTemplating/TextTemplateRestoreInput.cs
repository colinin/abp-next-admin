using System.ComponentModel.DataAnnotations;
using Volo.Abp.Validation;

namespace LINGYUN.Abp.TextTemplating;

public class TextTemplateRestoreInput
{
    [Required]
    [DynamicStringLength(typeof(TextTemplateConsts), nameof(TextTemplateConsts.MaxNameLength))]
    public string Name { get; set; }

    [DynamicStringLength(typeof(TextTemplateConsts), nameof(TextTemplateConsts.MaxCultureLength))]
    public string Culture { get; set; }
}
