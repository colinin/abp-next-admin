using System.ComponentModel.DataAnnotations;
using Volo.Abp.Validation;

namespace LINGYUN.Abp.LocalizationManagement;

public class RestoreDefaultTextInput
{
    [Required]
    [DynamicStringLength(typeof(ResourceConsts), nameof(ResourceConsts.MaxNameLength))]
    public string ResourceName { get; set; }

    [Required]
    [DynamicStringLength(typeof(TextConsts), nameof(TextConsts.MaxKeyLength))]
    public string Key { get; set; }

    [Required]
    [DynamicStringLength(typeof(LanguageConsts), nameof(LanguageConsts.MaxCultureNameLength))]
    public string CultureName { get; set; }
}
