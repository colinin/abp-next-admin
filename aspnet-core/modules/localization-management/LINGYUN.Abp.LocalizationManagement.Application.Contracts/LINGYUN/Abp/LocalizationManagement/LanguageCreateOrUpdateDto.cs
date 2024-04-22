using System.ComponentModel.DataAnnotations;
using Volo.Abp.Validation;

namespace LINGYUN.Abp.LocalizationManagement;
public abstract class LanguageCreateOrUpdateDto
{
    [Required]
    [DynamicStringLength(typeof(LanguageConsts), nameof(LanguageConsts.MaxDisplayNameLength))]
    public string DisplayName { get; set; }

    [DynamicStringLength(typeof(LanguageConsts), nameof(LanguageConsts.MaxFlagIconLength))]
    public string FlagIcon { get; set; }
}
