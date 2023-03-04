using Volo.Abp.Validation;

namespace LINGYUN.Abp.TextTemplating;

public class TextTemplateRestoreInput
{
    [DynamicStringLength(typeof(TextTemplateConsts), nameof(TextTemplateConsts.MaxCultureLength))]
    public string Culture { get; set; }
}
